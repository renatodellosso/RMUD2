using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations
{
    public class DeepWoods : Location
    {

        protected override string Description => "The deep, dark part of the forest. Nearly no light reaches to the forest floor here.";

        public DeepWoods()
        {
            id = "deepwoods";
            name = Utils.Style("Deep Woods", "maroon");
            status = "Deep in the Woods";

            safe = true;

            objects.Add(new WorldObjects.CraftingStation("demonstatue", Utils.Style("Demon Statue", "maroon"), id, RecipeLists.DEMON_STATUE));
        }

    }
}