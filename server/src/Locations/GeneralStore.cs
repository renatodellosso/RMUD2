using ItemTypes;
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

                    if (args.Length == 0)
                    {
                        inputs.Add(new(InputMode.Option, "sell", "Sell Goods"));
                        inputs.Add(new(InputMode.Option, "leave", "Goodbye"));
                    }
                    else
                    {
                        inputs.Add(menu.back);

                        if (menu.state == "sell")
                        {
                            //Item not yet specified
                            Utils.AddItemOptionsFromInventory(inputs, session.Player!.inventory); //We use session.Player!.inventory because we know the player is not null (at least it shouldn't be)
                        }
                        else if (args.Length == 2)
                        {
                            ItemHolder<Item>? item = session.Player!.inventory[int.Parse(args[1])];
                            inputs.Add(new(InputMode.Option, "sell", $"Sell for {Utils.Gold(item.Item.SellValue)} each ({Utils.Gold(item.SellValue)} total)"));
                        }
                    }

                    return inputs.ToArray();
                },
                talkHandler: (session, action, menu) =>
                {
                    string[] args = menu.state.Split('.');

                    if (args.Length == 0)
                    {
                        if (action.action.Equals("sell"))
                        {
                            menu.state = "sell";
                            session.Log(Utils.Dialogue(creatures.First(), "What would you like to sell?"));
                        }
                        else if(action.action.Equals("leave"))
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

                        }
                    }
                }
            ));
        }

    }
}