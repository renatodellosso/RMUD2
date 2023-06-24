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

        float weight = 0;
        public virtual float Weight => weight; //Using virtual methods for stats is probably the way to go, since it allows for more adaptability

        public Item(string id, string name, float weight, string description = "No description provided")
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.weight = weight;
        }

        public virtual string Overview(Dictionary<string, object> data)
        {
            return $"{FormattedName} x{data["amt"]}:<br/>" +
                $"{(int)data["amt"] * Weight} lbs. total, {Weight} lbs. each<br>" +
                $"{description}";
        }

    }
}