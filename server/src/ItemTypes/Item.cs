using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemTypes
{
    public class Item
    {

        public string id, name;

        public virtual string FormattedName => name;

        public Item(string id, string name)
        {
            this.id = id;
            this.name = name;
        }

    }
}