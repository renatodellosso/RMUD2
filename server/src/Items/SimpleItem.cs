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
        public override int SellValue => sellValue;

        public SimpleItem(string id, string name, float weight, string description = "No description provided", int sellValue = 0) : base(id, name, weight, description)
        {
            this.sellValue = sellValue;
        }
    }
}
