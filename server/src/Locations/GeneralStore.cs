using ItemTypes;
using Items;
using Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations
{
    public class GeneralStore : Location
    {

        protected override string Description => "A small fire faintly illuminates the cramped stone walls around you, its dancing flames reveal all manner of goods on shelves.";

        Recipe[] shop = new Recipe[]
        {
            new("pickaxe"),
        };

        public GeneralStore()
        {
            id = "generalstore";
            name = "General Store";
            status = "In Town";

            creatures.Add(new Creatures.SimpleNPC("tarel", "Tarel, Shopkeeper", 
                talkStart: (session) =>
                {
                    //Talk Start
                    session.Log(Utils.Dialogue(creatures.First(), "What can I do for you today?")); //We use creatures.First() because we can't reference the NPC in its constructor.
                },
                talkInputs: (session, menu) =>
                {
                    List<Input> inputs = new();
                    string[] args = menu.state.Split('.');

                    if (menu.state == "")
                    {
                        //The option to leave/go back always goes first
                        inputs.Add(new(InputMode.Option, "leave", "Goodbye"));
                        inputs.Add(new(InputMode.Option, "buy", "Buy Goods"));
                        inputs.Add(new(InputMode.Option, "sell", "Sell Goods"));
                    }
                    else
                    {
                        inputs.Add(menu.back);

                        if (menu.state == "sell")
                        {
                            //Item not yet specified
                            Utils.AddItemOptionsFromInventory(inputs, session.Player!.inventory, new string[] { "coin" }); //We use session.Player!.inventory because we know the player is not null (at least it shouldn't be)
                        }
                        else if (args.Length == 2)
                        {
                            ItemHolder<Item>? item = session.Player!.inventory[int.Parse(args[1])];
                            inputs.Add(new(InputMode.Option, item.amt.ToString(), $"Max - {item.amt}"));
                            inputs.Add(new(InputMode.Text, "sell", 
                                $"How many to sell? Sell for {Utils.Coins(Utils.Round(item.Item.SellValue * session.Player.SellCut), false)} each" +
                                $"({Utils.Coins(Utils.Round(item.SellValue * session.Player.SellCut), false)} total)"));
                        }
                    }

                    return inputs.ToArray();
                },
                talkHandler: (session, action, menu) =>
                {
                    string[] args = menu.state.Split('.');

                    if (menu.state == "")
                    {
                        if (action.action.Equals("sell"))
                        {
                            menu.state = "sell";
                            session.Log(Utils.Dialogue(creatures.First(), "What would you like to sell?"));
                        }
                        else if (action.action == "buy")
                            session.SetMenu(new CraftingMenu("General Store", shop));
                        else if (action.action.Equals("leave"))
                        {
                            menu.state = "exit"; //Set the state to exit so we can exit the menu.
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
                    } else if(args.Length == 2)
                    {
                        if (action.action.Equals("back"))
                            menu.state = "sell";
                        else
                        {
                            ItemHolder<Item>? item = session.Player!.inventory[int.Parse(args[1])].Clone();

                            int amt = int.Parse(action.action);
                            if (amt <= 0 || amt > item.amt)
                            {
                                session.Log($"Invalid amount. Must be between 1 and {item.amt}");
                                return;
                            }

                            item.amt = amt;

                            ItemHolder<Item>? sold = session.Player!.inventory.Remove(item);

                            if (sold == null || sold.amt == 0)
                            {
                                session.Log($"You cannot carry any more {item.Item?.name}");
                            }
                            else
                            {
                                ItemHolder<Item> gold = new("coin", Utils.Round(sold.SellValue * session.Player.SellCut));
                                ItemHolder<Item> leftOver = session.Player!.inventory.Add(gold)?.Clone() ?? gold.Clone();
                                //If leftOver is null, then we add the full amount of gold to the player's inventory.
                                //Otherwise, we add the amount of gold that was not added to the player's inventory.
                                leftOver.amt = gold.amt - leftOver?.amt ?? 0;

                                if (leftOver!.amt > 0)
                                {
                                    ItemHolder<Item> refunded = sold.Clone();
                                    refunded.amt = (int)Math.Ceiling((double)leftOver.amt / refunded.Item!.SellValue);

                                    leftOver.amt = refunded.amt * refunded.Item.SellValue; //How much gold to take away
                                    session.Player.inventory.Add(refunded);
                                    session.Player.inventory.Remove(leftOver);

                                    session.Log($"You cannot carry any more coins. Refunded {refunded.amt}x {refunded.FormattedName}");
                                }

                                session.Log($"Sold {sold.amt}x {sold.FormattedName} for {Utils.Coins(sold.SellValue)}");
                                session.Player.Update();
                            }

                            menu.state = "sell";
                        }
                    }
                }
            ));
        }

    }
}