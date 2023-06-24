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
        public SimpleItem(string id, string name, float weight, string description = "No description provided") : base(id, name, weight, description)
        {

        }
    }
}
