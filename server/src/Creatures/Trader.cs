using Events;
using ItemTypes;
using Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creatures
{
    public class Trader : SimpleNPC
    {

        string startMsg;
        bool canSell;

        Recipe[] shop;

        public Trader(string id, string name, string startMsg, Recipe[] shop, string nameColor = "", bool canSell = true)
            : base(id, name, nameColor)
        {
            talkInputs = TalkInputs;
            talkHandler = TalkHandler;
            talkStart = TalkStart;

            this.startMsg = startMsg;
            this.canSell = canSell;
            this.shop = shop;
        }

        Input[] TalkInputs(Session session, DialogueMenu menu)
        {
            List<Input> inputs = new();
            string[] args = menu.state.Split('.');

            if (menu.state == "")
            {
                //The option to leave/go back always goes first
                inputs.Add(new(InputMode.Option, "leave", "Goodbye"));
                inputs.Add(new(InputMode.Option, "buy", $"Buy"));
                if(canSell) inputs.Add(new(InputMode.Option, "sell", "Sell"));
            }
            else
            {
                inputs.Add(menu.back);

                if (canSell)
                {
                    if (menu.state == "sell")
                    {
                        //Item not yet specified

                        //We use session.Player!.inventory because we know the player is not null (at least it shouldn't be)
                        Utils.AddItemOptionsFromInventory(inputs, session.Player!.inventory, new string[] { "coin" }); 
                    }
                    else if (args.Length == 2)
                    {
                        ItemHolder<Item>? item = session.Player!.inventory[int.Parse(args[1])];
                        inputs.Add(new(InputMode.Option, item.amt.ToString(), $"Max - {item.amt}"));
                        inputs.Add(new(InputMode.Text, "sell",
                            $"How many to sell? Sell for {Utils.Coins(Utils.Round(item.Item.SellValue * session.Player.SellCut), false)} each " +
                            $"({Utils.Coins(Utils.Round(item.SellValue * session.Player.SellCut), false)} total)"));
                    }
                }
            }

            return inputs.ToArray();
        }

        void TalkHandler(Session session, ClientAction action, DialogueMenu menu)
        {
            string[] args = menu.state.Split('.');

            if (menu.state == "")
            {
                if (action.action.Equals("leave"))
                {
                    menu.state = "exit"; //Set the state to exit so we can exit the menu.
                }
                else if (action.action == "buy")
                {
                    session?.SetMenu(new CraftingMenu(FormattedName, shop));
                }
                else if (canSell && action.action.Equals("sell"))
                {
                    menu.state = "sell";
                    session.Log(Utils.Dialogue(this, "What would you like to sell?"));
                }
            }
            else if (args.Length == 1)
            {
                if (action.action.Equals("back"))
                    menu.state = "";
                else
                {
                    int index = int.Parse(action.action);

                    if (index < 0 || index >= session.Player!.inventory.Count)
                    {
                        session.Log("Invalid index.");
                        return;
                    }

                    ItemHolder<Item>? item = session.Player!.inventory[index];
                    session.Log(item.Overview());
                    menu.state += "." + index;
                }
            }
            else if (canSell && args.Length == 2)
            {
                if (action.action.Equals("back"))
                    menu.state = "sell";
                else
                {
                    Player player = session.Player!;
                    ItemHolder<Item>? item = player!.inventory[int.Parse(args[1])].Clone();

                    int amt = int.Parse(action.action);
                    if (amt <= 0 || amt > item.amt)
                    {
                        session.Log($"Invalid amount. Must be between 1 and {item.amt}");
                        return;
                    }

                    item.amt = amt;

                    ItemHolder<Item>? sold = player!.inventory.Remove(item);

                    if (sold == null || sold.amt == 0)
                    {
                        session.Log($"You cannot carry any more {item.Item?.name}");
                    }
                    else
                    {

                        ItemHolder<Item> gold = new("coin", Utils.Round(sold.SellValue * player.SellCut));
                        ItemHolder<Item> leftOver = player!.inventory.Add(gold, true)?.Clone() ?? gold.Clone();

                        session.Log($"Sold {sold.amt}x {sold.FormattedName} for {Utils.Coins(sold.SellValue * player.SellCut)}");
                        player.Update();
                    }

                    menu.state = "sell";
                }
            }
        }

        void TalkStart(Session session)
        {
            session.Log(Utils.Dialogue(this, startMsg));
        }

    }
}
