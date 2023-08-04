using ItemTypes;
using Items;
using Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations
{
    public class GeneralStore : Location
    {

        protected override string Description => "A small fire faintly illuminates the cramped stone walls around you, its dancing flames reveal all manner of goods on shelves.";

        Recipe[] shop = new Recipe[]
        {
            new("pickaxe"),
            new("log"),
            new("bottle"),
            new("cloth"),
            new("returnscroll")
        };

        public GeneralStore()
        {
            id = "generalstore";
            name = "General Store";
            status = "In Town";

            safe = true;

            creatures.Add(new Creatures.Trader("tarel", "Tarel, Shopkeeper", "What can I do for you today?", shop));

            objects.Add(new WorldObjects.CraftingStation("loom", "Loom", id, RecipeLists.LOOM));
        }

    }
}