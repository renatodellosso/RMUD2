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
            new(1, () => new("zombie", "Zombie", maxHealth: 10, onTick: (self) =>
            {
                //onTick
                //Utils.Log($"Ticked {self.baseId}");
            })),

            //Skeleton
            new(1, () => new("skeleton", "Skeleton", maxHealth: 5, onTick: (self) =>
            {
                //onTick
                //Utils.Log($"Ticked {self.baseId}");
            }))

        );

    }
}
