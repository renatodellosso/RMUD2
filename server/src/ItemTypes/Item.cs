using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemTypes
{
    public class Item
    {

        public string id, name, description, color;

        public virtual string FormattedName => Utils.Style(name,color);

        float weight = 0;
        public virtual float Weight => weight; //Using virtual methods for stats is probably the way to go, since it allows for more adaptability

        public virtual int SellValue => 0;

        public Item(string id, string name, float weight, string description = "No description provided", string color = "white")
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.weight = weight;
            this.color = color;
        }

        public virtual string Overview(ItemHolder<Item> item, Creature? creature = null)
        {
            int amt = (int)item.data.GetValueOrDefault("amt", 0);

            return $"{FormattedName} x{item.data["amt"]}:<br/>" +
                $"Worth {Utils.Coins(amt * SellValue)} total, {Utils.Coins(SellValue)} each ({Utils.Coins(SellValue / Weight)}/lb.)<br>" +
                $"{Utils.Round(amt * Weight, 1)} lbs. total, {Weight} lbs. each<br>" +
                $"{description}";
        }

        public virtual List<Input> GetInputs(Session session, ItemHolder<Item> item, string state)
        {
            //IMPORTANT: All actions
            List<Input> inputs = new();

            if (session.Player?.inventory.Contains(item) ?? false) {
                if (!state.Contains(".drop"))
                    inputs.Add(new(InputMode.Option, "drop", "Drop"));
                else
                    Utils.AddItemAmountOptions(inputs, item);
            }

            return inputs;
        }

        public virtual void HandleInput(Session session, ClientAction action, ItemHolder<Item> item, ref string state, ref bool addStateToPrev)
        {
            Player player = session.Player!;
            if (player.inventory.Contains(item)) {
                if (!state.Contains(".drop") && action.action == "drop")
                    state += ".drop";
                else if(state.Contains(".drop"))
                {
                    try
                    {
                        item = item.Clone();
                        item.amt = int.Parse(action.action);

                        player.Location?.objects.Add(new WorldObjects.DroppedItem(item, player.Location.id));
                        session.Log($"You dropped {item.FormattedName} x{item.amt}.");

                        state = "inventory";
                        addStateToPrev = false;

                        player.inventory.Remove(item); //This edits the original item's amount, so we do it last
                        player.Update();
                    }
                    catch (Exception e)
                    {
                        session.Log($"Invalid amount");
                    }
                }
            }
        }

    }
}