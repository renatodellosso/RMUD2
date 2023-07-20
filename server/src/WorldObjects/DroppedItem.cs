using ItemTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldObjects
{
    public class DroppedItem : Container
    {

        ItemHolder<Item> item;

        protected override bool RemoveIfEmpty => true;

        public DroppedItem(ItemHolder<Item> item, string location) : base(item.Item?.id + item.amt, $"{item.FormattedName} x{item.amt}", location, 
            new ItemHolder<Item>[] { item })
        {
            this.item = item;
        }

        public override string GetOverview(Player player)
        {
            return item.Overview();
        }
    }
}
