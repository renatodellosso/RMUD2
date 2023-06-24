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

        public SimpleMonster(string id, string name, int maxHealth, Weapon weapon, int attackInterval = 3, Table<Func<ItemHolder<Item>>>? drops = null, int minDrops = 1,
            int maxDrops = 1, int xp = 0)
            : base(id, name, nameColor: "red", maxHealth: maxHealth, onTick: (data) => ((SimpleMonster)data.self).OnTick(), drops: drops, minDrops: minDrops, maxDrops: maxDrops,
                  xp: xp)
        {
            attackable = true;
            this.weapon = weapon;
            this.attackInterval = attackInterval;
        }

        void OnTick()
        {
            try
            {
                if (Weapon != null && Utils.tickCount % attackInterval == 0)
                {
                    //Attack a random player
                    List<Player> players = Location.Players.ToList();
                    if (players.Count > 0)
                    {
                        Utils.Log($"{name} attacks!");
                        Player player = players[Utils.RandInt(players.Count)];
                        Attack(player, Weapon);
                    }
                }

                if (Utils.tickCount % 3 == 0 && Utils.RandFloat() < .05f)
                    MoveThroughRandomExit();

                //Flavor messages
                FlavorMessage();
            } catch(Exception e)
            {
                Utils.Log(e.Message + "\n" + e.StackTrace);
            }
        }

    }
}
