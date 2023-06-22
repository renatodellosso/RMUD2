using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemTypes
{
    public class Item
    {

        public string id, name, description;

        public virtual string FormattedName => name;

        public virtual float Weight => 0; //Using virtual methods for stats is probably the way to go, since it allows for more adaptability

        public virtual string Overview => $"{FormattedName}:<br/>{description}";

        public Item(string id, string name, string description = "No description provided")
        {
            this.id = id;
            this.name = name;
            this.description = description;
        }

    }
}