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

        public virtual int SellValue => 0;

        public Item(string id, string name, float weight, string description = "No description provided")
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.weight = weight;
        }

        public virtual string Overview(Dictionary<string, object> data)
        {
            int amt = (int)data.GetValueOrDefault("amt", 0);

            return $"{FormattedName} x{data["amt"]}:<br/>" +
                $"Worth {Utils.Coins(amt * SellValue)} total, {Utils.Coins(SellValue)} each ({Utils.Coins(SellValue / Weight)}/lb.)<br>" +
                $"{Utils.Round(amt * Weight, 1)} lbs. total, {Weight} lbs. each<br>" +
                $"{description}";
        }

        public virtual List<Input> GetInputs(Session session, ItemHolder<Item> item)
        {
            //IMPORTANT: All actions 
            return new List<Input>();
        }

        public virtual void HandleInput(Session session, ClientAction action, ItemHolder<Item> item, ref string state, ref bool addStateToPrev)
        {

        }

    }
}