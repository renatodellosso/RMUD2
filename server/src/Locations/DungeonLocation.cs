using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations
{
    public class DungeonLocation : Location
    {

        public Vector2 position;

        public DungeonLocation(Floor floor, Vector2 position)
        {
            this.position = position;

            id = floor.PosToId(position);
            name = floor.PosToName(position);

            Add(this);

            if (Utils.RandFloat() < Config.DungeonGeneration.MONSTER_CHANCE)
                creatures.Add(Creatures.MonsterList.monsters.Get()()); //Monsters is a list of Funcs, so we call the func that gets returned
        }

    }
}
