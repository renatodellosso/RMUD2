using Items;
using Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Items
{
    public class DungeonTeleportationScroll : SimpleConsumable
    {

        int floorDepth;

        public DungeonTeleportationScroll(int floor)
            : base(GetId(floor), $"Teleport Scroll ({floor+1})", 2, null, "Use", $"Teleports the user to floor {floor+1}.", 45 * (floor + 1), 1, "wheat")
        {
            floorDepth = floor;
            onUse = OnUse;
        }

        void OnUse(Session session)
        {
            Floor floor = Dungeon.floors[new(floorDepth, 0)];
            DungeonLocation location = floor.rooms[Utils.RandInt(floor.rooms.Count)];

            Player player = session.Player!;
            location.Enter(player, player.Location);
        }

        public static string GetId(int floor)
        {
            return $"teleportscroll{floor}";
        }
    }
}
