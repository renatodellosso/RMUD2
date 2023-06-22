using ItemTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldObjects
{
    public class Corpse : Container
    {
        public Corpse(Creature creature) : base(creature.baseId + "corpse", $"Corpse of {creature.FormattedName}", new ItemHolder<Item>[] { new("spear") })
        {

        }
    }
}
