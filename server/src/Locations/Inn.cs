using ItemTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations
{
    public class Inn : Location
    {

        protected override string Description => "A warm hearth sheds light on the humble inn and tavern around you.";

        const int REST_COST = 5;

        public Inn()
        {
            id = "inn";
            name = "Inn & Tavern";
            status = "At the Tavern";

            creatures.Add(new Creatures.SimpleNPC("hesin", "Hesin, Innkeeper", 
                talkStart: (session) =>
                {
                    //Talk Start
                    session.Log(Utils.Dialogue(creatures.First(), "Welcome back.")); //We use creatures.First() because we can't reference the NPC in its constructor.
                },
                talkInputs: (session, menu) =>
                {
                    List<Input> inputs = new();
                    string[] args = menu.state.Split('.');

                    if (menu.state == "")
                    {
                        //The option to leave/go back always goes first
                        inputs.Add(new(InputMode.Option, "leave", "Goodbye"));
                        inputs.Add(new(InputMode.Option, "rest", $"Rent a room & rest - {Utils.Coins(REST_COST)}"));
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
                        if(action.action.Equals("leave"))
                        {
                            menu.state = "exit"; //Set the state to exit so we can exit the menu.
                        }
                        else if (action.action == "rest")
                        {
                            int coins = session.Player!.coins;

                            if (coins < REST_COST)
                            {
                                session.Log(Utils.Dialogue(creatures.First(), "Come back when you can afford it."));
                                return;
                            }

                            session.Player.coins -= REST_COST;
                            session.Log(Utils.Dialogue(creatures.First(), "Here's your key. Your room is upstairs."));
                            session.Player?.Rest();
                        }
                    }
                }
            ));
        }

    }
}