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

            Location.Add(this);
        }

    }
}
