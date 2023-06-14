using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creatures
{
    public class SimpleMonster : SimpleNPC
    {

        public SimpleMonster(string id, string name, int maxHealth) : base(id, name, nameColor: "red", maxHealth: maxHealth, onTick: (data) => ((SimpleMonster)data.self).OnTick())
        {
            attackable = true;
        }

        void OnTick()
        {

            if (Utils.tickCount % 3 == 0 && Utils.RandFloat() < .05f)
                MoveThroughRandomExit();

            //Flavor messages
            FlavorMessage();
        }

    }
}
