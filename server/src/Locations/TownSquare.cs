using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations
{
    public class TownSquare : Location
    {

        protected override string Description => "A rocky and overgrown town center lies around you.";

        public TownSquare()
        {
            id = "townsquare";
            name = "Town Square";
            status = "In Town";

            safe = true;

            objects.Add(new WorldObjects.CraftingStation("campfire", "Campfire", id, RecipeLists.CAMPFIRE));

            creatures.Add(new Creatures.SimpleNPC("firetender", "Fire Tender", "firebrick",
                talkStart: (session) => session.Log($"The {Utils.Style("Fire Tender", "firebrick")} silently burns logs in the campfire."),
                talkInputs: (session, menu) => new Input[] { new("leave", "Goodbye") },
                talkHandler: (session, action, menu) => menu.state = "exit"));
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