using Events;
using ItemTypes;
using Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldObjects;

namespace Creatures
{
    public class SimpleNPC : Creature
    {

        public Action<OnCreatureTickEventData>? onTick;

        int maxHealth;
        public override int MaxHealth => maxHealth;

        Table<Func<ItemHolder<Item>>>? drops;
        int minDrops = 1, maxDrops = 1;

        int defense = 0;

        public SimpleNPC(string id, string name, string nameColor = "", Func<Session, DialogueMenu, Input[]>? talkInputs = null, 
            Action<Session, ClientAction, DialogueMenu>? talkHandler = null,  Action<Session>? talkStart = null, int maxHealth = 0, 
            Action<OnCreatureTickEventData>? onTick = null, Table<Func<ItemHolder<Item>>>? drops = null,  int minDrops = 1, int maxDrops = 1, int xp = 0, bool actual = true, 
            Dictionary<DamageType, int>? resistances = null, int defense = 0)
            : base(id, name, actual)
        {
            this.nameColor = nameColor;

            attackable = false;

            this.talkInputs = talkInputs;
            this.talkHandler = talkHandler;
            this.talkStart = talkStart;

            this.maxHealth = maxHealth;
            health = maxHealth;

            this.onTick = onTick;

            this.drops = drops;
            this.minDrops = minDrops;
            this.maxDrops = maxDrops;

            xpValue = xp;

            if(resistances != null)
                this.resistances = resistances;

            this.defense = defense;

            //Utils.Log($"Created {baseId}");
        }

        public override void Tick(int tickCount)
        {
            base.Tick(tickCount);

            //Run onTick, as long as it's not null
            onTick?.Invoke(new(this));
        }

        public void FlavorMessage()
        {
            Location? location = Location;
            if (location != null)
            {
                Player[] players = location.Players;
                if (players.Any() && Utils.RandFloat() < Config.Gameplay.FLAVOR_MSG_CHANCE) //Don't send flavor messages if there are no players to receive them
                {
                    AI.FlavorMessage(this);
                }
            }
        }

        protected override void OnDie(CreatureDeathEventData data)
        {
            //Create a corpse
            Corpse corpse = new Corpse(this);
            Location.objects.Add(corpse);
            
            //Add drops to the corpse
            if (drops != null && minDrops > 0 && maxDrops > 0)
            {
                int dropCount = Utils.RandInt(minDrops, maxDrops+1);
                for (int i = 0; i < dropCount; i++)
                    corpse.inventory.Add(drops.Get()());
            }

            base.OnDie(data);
        }

        public override int GetDefense(DamageType? damageType = null)
        {
            return base.GetDefense(damageType) + defense;
        }

    }
}
