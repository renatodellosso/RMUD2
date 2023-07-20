using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations
{
    public class Woods : Location
    {

        protected override string Description => "The dark trees surround you, with the leaves and animals casting shadows.";

        public Woods()
        {
            id = "woods";
            name = "The Woods";
            status = "In the Woods";

            safe = true;

            objects.Add(new WorldObjects.CraftingStation("grove", "Small Grove", id, RecipeLists.GROVE));
        }

        public override void AddExits()
        {
            Exit.AddExit(this, Get("grotto"), "E", true);
        }

    }
}