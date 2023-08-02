using ItemTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Items
{
    public class SimpleItem : Item
    {
        int sellValue = 0;
        public override int SellValue(ItemHolder<Item>? item) => sellValue;

        string color = "";
        public override string FormattedName => color == "" ? name : Utils.Style(name, color);

        public SimpleItem(string id, string name, float weight, string description = "No description provided", int sellValue = 0, string color = "")
            : base(id, name, weight, description, color)
        {
            this.sellValue = sellValue;
            this.color = color;
        }
    }
}
