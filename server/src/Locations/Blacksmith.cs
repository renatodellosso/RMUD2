using ItemTypes;
using Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations
{
    public class Blacksmith : Location
    {

        protected override string Description => "You feel the furnace's heat warm you against the chill of the wind.";

        Recipe[] shop = new Recipe[]
        {
            new("Bought", new ItemHolder<Item>("coin", Items.ItemList.Get("axe").SellValue), new ItemHolder<Item>("axe", 1)),
        };

        public Blacksmith()
        {
            id = "blacksmith";
            name = "Blacksmith";
            status = "In Town";

            creatures.Add(new Creatures.SimpleNPC("daes", "Daes, Smith", 
                talkStart: (session) =>
                {
                    //Talk Start
                    session.Log(Utils.Dialogue(creatures.First(), "'Ello.")); //We use creatures.First() because we can't reference the NPC in its constructor.
                },
                talkInputs: (session, menu) =>
                {
                    List<Input> inputs = new();
                    string[] args = menu.state.Split('.');

                    if (menu.state == "")
                    {
                        //The option to leave/go back always goes first
                        inputs.Add(new(InputMode.Option, "leave", "Goodbye"));
                        inputs.Add(new(InputMode.Option, "buy", $"Buy"));
                    }
                    else
                    {
                        inputs.Add(menu.back);
                    }

                    return inputs.ToArray();
                },
                talkHandler: (session, action, menu) =>
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
                            session?.SetMenu(new CraftingMenu("Blacksmith", shop));
                        }
                    }
                }
            ));
        }

    }
}