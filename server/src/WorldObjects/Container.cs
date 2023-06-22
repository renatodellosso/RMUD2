using ItemTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldObjects
{
    public class Container : WorldObject
    {

        Inventory inventory = new();

        public Container(string id, string name, ItemHolder<Item>[] items) : base(id, name)
        {
            inventory.Add(items);
        }

        public Container(string id, string name) : this(id, name, Array.Empty<ItemHolder<Item>>())
        {

        }

        public override List<Input> GetInputs(Player player, string state)
        {
            string[] args = state.Split('.');
            List<Input> inputs = new();

            if (CanAccess(player))
            {
                //If no item specifier is present
                if(args.Length == 2)
                    for(int i = 0; i < inventory.Count; i++)
                        inputs.Add(new(InputMode.Option, i.ToString(), inventory[i].FormattedName));
                else if(args.Length == 3)
                    inputs.Add(new(InputMode.Option, "take", "Take"));
            }

            return inputs;
        }

        public override void HandleInput(Session session, ClientAction action, ref string state)
        {
            string[] args = state.Split('.');

            if (!action.action.Equals("back"))
            {
                if (CanAccess(session.Player))
                {
                    if (args.Length == 2)
                    {
                        //Item not yet specified
                        int index = -1;
                        try
                        {
                            index = int.Parse(action.action);
                        }
                        catch
                        {
                            session.Log("Invalid index");
                            return;
                        }

                        if (index < 0 || index >= inventory.Count)
                        {
                            session.Log("Invalid index");
                            return;
                        }

                        ItemHolder<Item> item = inventory[index];
                        session.Log(item.Item.Overview);
                        state += "." + index;
                    }
                    else if (args.Length == 3)
                    {
                        //Item specified
                        if (action.action.Equals("take"))
                        {
                            ItemHolder<Item>? leftOver = session.Player.inventory.Add(inventory[int.Parse(args[2])]); //Try to add the item to the player's inventory
                            if (leftOver != null)
                            {
                                session.Log($"You cannot carry any more {leftOver.Item.name}");
                            }
                        }
                    }
                }
                else
                {
                    session.Log($"You cannot access {FormattedName}");
                    action.action = "back"; //Automatically go back a state
                }
            }
        }

        protected virtual bool CanAccess(Creature creature)
        {
            return true;
        }
    }
}
