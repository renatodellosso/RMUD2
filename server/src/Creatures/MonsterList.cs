using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creatures
{
    public static class MonsterList
    {

        public static Table<Func<SimpleNPC>> monsters = new(

            //Zombie
            new(1, () => new("zombie", "Zombie", nameColor: "red", maxHealth: 10, onTick: (self, count) =>
            {
                if (count % 3 == 0 && Utils.RandFloat() < .05f)
                    self.MoveThroughRandomExit();

                //Flavor messages
                Location? location = self.Location;
                if (location != null)
                {
                    Player[] players = location.Players;
                    if (players.Any() && Utils.RandFloat() < Config.Gameplay.FLAVOR_MSG_CHANCE) //Don't send flavor messages if there are no players to receive them
                    {
                        AI.FlavorMessage(self);
                    }
                }
            })),

            //Skeleton
            new(1, () => new("skeleton", "Skeleton", nameColor: "red", maxHealth: 5, onTick: (self, count) =>
            {
                if(count % 3 == 0 && Utils.RandFloat() < .05f)
                    self.MoveThroughRandomExit();

                //Flavor messages
                Location? location = self.Location;
                if (location != null)
                {
                    Player[] players = location.Players;
                    if (players.Any() && Utils.RandFloat() < Config.Gameplay.FLAVOR_MSG_CHANCE) //Don't send flavor messages if there are no players to receive them
                    {
                        AI.FlavorMessage(self);
                    }
                }
            }))

        );

    }
}
