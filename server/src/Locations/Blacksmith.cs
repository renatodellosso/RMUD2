using ItemTypes;
using Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations
{
    public class Blacksmith : Location
    {

        protected override string Description => "You feel the furnace's heat warm you against the chill of the wind.";

        Recipe[] shop = new Recipe[]
        {
            new("ironbar", 2),
            new("axe"),
            new("spear"),
            new("pickaxe"),
            new("longsword"),
            new("mace"),
            new("breastplate"),
            new("chainmail"),
            new("platearmor")
        };

        public Blacksmith()
        {
            id = "blacksmith";
            name = "Blacksmith";
            status = "In Town";

            safe = true;

            objects.Add(new WorldObjects.CraftingStation("forge", "Forge", id, RecipeLists.FORGE));
            objects.Add(new WorldObjects.CraftingStation("loom", "Loom", id, RecipeLists.LOOM));
            objects.Add(new WorldObjects.Anvil(this));

            creatures.Add(new Creatures.Trader("daes", "Daes, Smith", "'Ello", shop));
        }

    }
}