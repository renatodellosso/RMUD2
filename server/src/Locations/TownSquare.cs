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
        }

        public override void AddExits()
        {
            Exit.AddExit(this, Get("dungeonentrance"), "E", true);
            Exit.AddExit(this, Get("generalstore"), "N", true);
        }

    }
}