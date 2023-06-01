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
                if (count % 3 == 0 && Utils.RandFloat() < .1f)
                {
                    Player[]? players = self.Location?.Players;

                    if (players != null)
                        Array.ForEach(players, p => p?.session?.Log($"{self.name} groans.")); //Send to all players in location
                }

                if(count % 4 == 0 && Utils.RandFloat() < .1f)
                {
                    Player[]? players = self.Location?.Players;

                    if (players != null)
                        Array.ForEach(players, p => p?.session?.Log($"{self.name} shuffles aimlessly.")); //Send to all players in location
                }

                if(count % 5 == 0 && Utils.RandFloat() < .1f)
                {
                    Player[]? players = self.Location?.Players;

                    if (players != null)
                        Array.ForEach(players, p => p?.session?.Log($"{self.name} stares at you.")); //Send to all players in location
                }
            })),

            //Skeleton
            new(1, () => new("skeleton", "Skeleton", nameColor: "red", maxHealth: 5, onTick: (self, count) =>
            {
                if(count % 3 == 0 && Utils.RandFloat() < .05f)
                    self.MoveThroughRandomExit();

                //Flavor messages
                if (count % 3 == 0 && Utils.RandFloat() < .1f)
                {
                    Player[]? players = self.Location?.Players;

                    if (players != null)
                        Array.ForEach(players, p => p?.session?.Log($"{self.name}'s bones rattle as it rotates itself.")); //Send to all players in location
                }

                if (count % 4 == 0 && Utils.RandFloat() < .1f)
                {
                    Player[]? players = self.Location?.Players;

                    if (players != null)
                        Array.ForEach(players, p => p?.session?.Log($"{self.name} shuffles aimlessly.")); //Send to all players in location
                }

                if (count % 5 == 0 && Utils.RandFloat() < .1f)
                {
                    Player[]? players = self.Location?.Players;

                    if (players != null)
                        Array.ForEach(players, p => p?.session?.Log($"{self.name} stares at you.")); //Send to all players in location
                }
            }))

        );

    }
}
