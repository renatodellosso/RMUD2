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
                for (int i = 0; i < Utils.RandInt(Config.DungeonGeneration.MIN_MONSTERS, Config.DungeonGeneration.MAX_MONSTERS); i++)
                    //Spawn a monster
                    Enter(Creatures.MonsterList.monsters.Get()(), null); //Monsters is a list of Funcs, so we call the func that gets returned
        }

    }
}
