using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    public class CreatureDeathEventData
    {
        public object killer;

        public CreatureDeathEventData(object killer)
        {
            this.killer = killer;
        }
    }
}
