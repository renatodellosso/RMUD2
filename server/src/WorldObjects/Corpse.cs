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

        Creature creature;

        public Corpse(Creature creature) : base(creature.baseId + "corpse", $"Corpse of {creature.FormattedName}", creature.location, Array.Empty<ItemHolder<Item>>())
        {
            this.creature = creature;
        }

        public override string GetOverview(Player player)
        {
            return $"The body of a dead {creature.FormattedName}";
        }
    }
}
