using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations
{
    public class WizardHouse : Location
    {

        protected override string Description => "A deceptively spacious stone house. Racks of unknown reagents line the walls.";

        readonly Recipe[] SHOP = new Recipe[]
        {
            new("returnscroll"),
            new(Items.DungeonTeleportationScroll.GetId(3)),
            new(Items.DungeonTeleportationScroll.GetId(6)),
            new(Items.DungeonTeleportationScroll.GetId(9)),
            new("returnamulet"),
            new("volcano")
        };

        public WizardHouse()
        {
            id = "wizardhouse";
            name = "Wizard's House";
            status = "At the Wizard";

            safe = true;

            creatures.Add(new Creatures.Trader("wizard", "Aelades, Wizard", "What brings you here?", SHOP, "purple"));
        }

    }
}