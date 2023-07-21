using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations
{
    public class TownSquare : Location
    {

        protected override string Description => "A rocky and overgrown town center lies around you." +
            "s<img src=\"https://cdn.cloudflare.steamstatic.com/steam/apps/489830/ss_5d19c69d33abca6f6271d75f371d4241c0d6b2d1.1920x1080.jpg?t=1650909796\" alt=\"Flowers in Chania\">";

        public TownSquare()
        {
            id = "townsquare";
            name = "Town Square";
            status = "In Town";

            safe = true;

            objects.Add(new WorldObjects.CraftingStation("campfire", "Campfire", id, RecipeLists.CAMPFIRE));
        }

        public override void AddExits()
        {
            Exit.AddExit(this, Get("dungeonentrance"), "E", true);
            Exit.AddExit(this, Get("generalstore"), "N", true);
            Exit.AddExit(this, Get("blacksmith"), "N", true);
            Exit.AddExit(this, Get("inn"), "S", true);
            Exit.AddExit(this, Get("woods"), "N", true);
            Exit.AddExit(this, Get("bank"), "W", true);
        }

    }
}