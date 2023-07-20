using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations
{
    public class Grotto : Location
    {

        protected override string Description => "A small, damp cave. Something feels wrong here.";

        public Grotto()
        {
            id = "grotto";
            name = Utils.Style("The Grotto", "midnightblue");
            status = "In a Forsaken Place";

            safe = false;

            objects.Add(new WorldObjects.CraftingStation("unholyalter", Utils.Style("Unholy Altar", "midnightblue"), id, RecipeLists.UNHOLY_ALTAR));
        }

    }
}