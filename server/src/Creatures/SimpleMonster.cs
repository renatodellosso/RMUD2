using Events;
using ItemTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creatures
{
    public class SimpleMonster : SimpleNPC
    {

        int attackInterval = 3; //Attacks when tickCount % attackInterval == 0

        Weapon weapon;
        public override Weapon? Weapon => weapon;

        new Action<OnCreatureTickEventData>? onTick;

        Func<Floor, float>? scaleTableWeight;

        public SimpleMonster(string id, string name, int maxHealth, Weapon weapon, int attackInterval = 3, Table<Func<ItemHolder<Item>>>? drops = null, int minDrops = 1,
            int maxDrops = 1, int xp = 0, Action<OnCreatureTickEventData>? onTick = null, Func<Floor, float>? scaleTableWeight = null, bool actual = true,
            int strength = 0, int dexterity = 0, int constitution = 0, int agility = 0, int endurance = 0, int intelligence = 0, int wisdom = 0, int charisma = 0,
            Dictionary<DamageType, int>? resistances = null, int defense = 0)
            : base(id, name, nameColor: "red", maxHealth: maxHealth, onTick: null, drops: drops, minDrops: minDrops, maxDrops: maxDrops,
                  xp: xp, actual: actual, resistances: resistances, defense: defense)
        {
            attackable = true;
            this.weapon = weapon;
            this.attackInterval = attackInterval;

            this.onTick = onTick;
            this.scaleTableWeight = scaleTableWeight;

            tags.Add("hostile");

            abilityScores = new()
            {
                { AbilityScore.Strength, strength },
                { AbilityScore.Dexterity, dexterity },
                { AbilityScore.Constitution, constitution },
                { AbilityScore.Agility, agility },
                { AbilityScore.Endurance, endurance },
                { AbilityScore.Intelligence, intelligence },
                { AbilityScore.Wisdom, wisdom },
                { AbilityScore.Charisma, charisma }
            };
        }

        public override void Tick(int tickCount)
        {
            base.Tick(tickCount);

            try
            {
                onTick?.Invoke(new(this));

                if (Utils.tickCount % attackInterval == 0) {
                    Attack[] attacks = GetAttacks();

                    if (attacks.Any())
                    {
                        Attack attack = attacks[Utils.RandInt(attacks.Length)];

                        //Attack a random player
                        List<Player> players = Location?.Players.ToList();
                        if (players!= null && players.Count > 0)
                        {
                            Utils.Log($"{name} attacks! Stamina: {stamina}/{MaxStamina}");
                            Player player = players[Utils.RandInt(players.Count)];
                            attack.execute(this, player, null);
                        }
                    }
                }

                if (Utils.tickCount % 3 == 0 && Utils.RandFloat() < .05f)
                    MoveThroughRandomExit(Config.Gameplay.MAX_ENEMIES_IN_ROOM);
            } catch(Exception e)
            {
                Utils.Log(e.Message + "\n" + e.StackTrace);
            }
        }

        public override float ScaleTableWeight(Floor floor)
        {
            return (scaleTableWeight ?? base.ScaleTableWeight)(floor); //If scaleTableWeight is null, call base.ScaleTableWeight. I know it looks kinda weird
        }

    }
}
