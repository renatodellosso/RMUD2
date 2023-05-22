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

        public DungeonLocation(Vector2 floorCoords, Vector2 position)
        {
            this.position = position;

            id = $"dungeon.{floorCoords.x}.{floorCoords.y}.{position.x}.{position.y}";
            name = $"Floor {floorCoords.x+1}-{floorCoords.y+1}, Room {position.x+1}-{position.y+1}";

            Location.Add(this);
        }

    }
}
