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

        public WizardHouse()
        {
            id = "wizardhouse";
            name = "Wizard's House";
            status = "At the Wizard";

            safe = true;
        }

    }
}