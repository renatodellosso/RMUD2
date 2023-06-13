using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    public class OnCreatureTickEventData
    {

        public Creature self;

        public int tickCount;

        public OnCreatureTickEventData(Creature self)
        {
            this.self = self;
            tickCount = Utils.tickCount;
        }

    }
}
