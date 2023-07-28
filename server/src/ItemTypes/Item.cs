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

            return $"{item.FormattedName} x{item.data["amt"]}:<br/>" +
                $"Worth {Utils.Coins(amt * SellValue)} total, {Utils.Coins(SellValue)} each ({Utils.Coins(SellValue / Weight)}/lb.)<br>" +
                $"{Utils.Round(amt * Weight, 1)} lbs. total, {Weight} lbs. each<br>" +
                $"{description}";
        }

        public virtual List<Input> GetInputs(Session session, ItemHolder<Item> item, string state)
        {
            //IMPORTANT: All actions
            List<Input> inputs = new();

            if (session.Player?.inventory.Contains(item) ?? false) {
                if (!state.Contains(".drop") && !state.Contains(".trade"))
                {
                    inputs.Add(new(InputMode.Option, "drop", "Drop"));
                    inputs.Add(new(InputMode.Option, "trade", "Offer for Trade"));
                }
                else if (state.Contains(".drop") || state.EndsWith(".trade"))
                    Utils.AddItemAmountOptions(inputs, item);
                else if (state.Contains(".trade"))
                    Utils.AddItemAmountOptions(inputs, session.Player.inventory.Where(i => i.Item?.id == "coin").FirstOrDefault(), max: 99999, text: "Enter how many coins to trade for");
            }

            return inputs;
        }

        public virtual void HandleInput(Session session, ClientAction action, ItemHolder<Item> item, ref string state, ref bool addStateToPrev)
        {
            Player player = session.Player!;
            if (player.inventory.Contains(item))
            {
                if (!state.Contains(".drop") && !state.Contains(".trade"))
                {
                    if (action.action == "drop") 
                    {
                        if (item.amt > 1)
                            state += ".drop";
                        else
                            DropItem(session, action, item, ref state, ref addStateToPrev);
                    }
                    else if (action.action == "trade")
                        state += ".trade";
                }
                else if (state.Contains(".drop") || state.EndsWith(".trade"))
                {
                    try
                    {
                        ItemHolder<Item> newItem = item.Clone();
                        try
                        {
                            newItem.amt = int.Parse(action.action);

                            if (state.Contains(".drop"))
                                DropItem(session, action, newItem, ref state, ref addStateToPrev);
                            else if (state.Contains(".trade"))
                            {
                                state += "." + newItem.amt;
                            }
                        }
                        catch
                        {
                            session.Log("Invalid amount");
                        }
                    }
                    catch (Exception e)
                    {
                        session.Log($"Invalid amount");
                    }
                }
                else if (state.Contains(".trade"))
                {
                    string[] args = state.Split('.');
                    int itemAmt = int.Parse(args.Last());

                    int coinAmt = -1;

                    try
                    {
                        coinAmt = int.Parse(action.action);
                    }
                    catch
                    {
                        session.Log("Invalid amount");
                        return;
                    }

                    if (coinAmt < 0)
                    {
                        session.Log("You can't trade negative coins!");
                        return;
                    }

                    ItemHolder<Item> offeredItem = item.Clone();
                    offeredItem.amt = itemAmt;

                    player.tradeOffers.Add(new(player, offeredItem, coinAmt));
                    player.inventory.Remove(offeredItem.Clone());
                    player.Update();

                    state = "inventory";
                    addStateToPrev = false;
                }
            }
            else 
                Utils.Log("Item not in inventory");
        }

        void DropItem(Session session, ClientAction action, ItemHolder<Item> item, ref string state, ref bool addStateToPrev)
        {
            try
            {
                Player player = session.Player!;

                item = item.Clone();
                item.amt = item.amt > 1 ? int.Parse(action.action) : 1;

                player.Location?.objects.Add(new WorldObjects.DroppedItem(item, player.Location.id));
                session.Log($"You dropped {item.FormattedName} x{item.amt}.");

                state = "inventory";
                addStateToPrev = false;

                player.inventory.Remove(item); //This edits the original item's amount, so we do it last
                player.Update();
            }
            catch (Exception e)
            {
                Utils.Log(e);
                session.Log($"Invalid amount");
            }
        }

    }
}