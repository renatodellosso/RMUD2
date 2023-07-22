using ItemTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace WorldObjects
{
    public class Container : WorldObject
    {

        public Inventory inventory = new();

        protected virtual bool RemoveIfEmpty => false;

        public Container(string id, string name, string location, ItemHolder<Item>[] items) : base(id, name, location)
        {
            inventory.Add(items);
        }

        public Container(string id, string name, string location) : this(id, name, location, Array.Empty<ItemHolder<Item>>())
        {

        }

        public override List<Input> GetInputs(Player player, string state)
        {
            string[] args = state.Split('.');
            List<Input> inputs = new();

            if (CanAccess(player))
            {
                //If no item specifier is present
                if (args.Length == 2)
                    Utils.AddItemOptionsFromInventory(inputs, inventory, takeAll: true);
                else if (args.Length == 3)
                    inputs.Add(new(InputMode.Option, "take", "Take"));
                else if (args.Length == 4)
                {
                    ItemHolder<Item>? item = inventory[int.Parse(args[2])];
                    inputs.Add(new(InputMode.Option, item.amt.ToString(), $"Max - {item.amt}"));
                    inputs.Add(new(InputMode.Text, "amt", "Enter an amount to take"));
                }
            }

            return inputs;
        }

        public override void HandleInput(Session session, ClientAction action, ref string state, ref bool addStateToPrev)
        {
            string[] args = state.Split('.');

            if (!action.action.Equals("back"))
            {
                if (CanAccess(session.Player))
                {
                    if (args.Length == 2)
                    {
                        if (action.action == "takeall")
                        {
                            Player player = session.Player!;

                            for(int i = 0; i < inventory.Count; i++)
                            {
                                try
                                {
                                    ItemHolder<Item> item = inventory[i];
                                    ItemHolder<Item>? transferred = inventory.Transfer(player.inventory, item);
                                    if (transferred != null)
                                    {
                                        session.Log($"Took {transferred.FormattedName} x{transferred.amt}");
                                        i--; //One less item in the inventory, so we need to go back one
                                    }
                                    else
                                        session.Log($"No room left in inventory");
                                }
                                catch(Exception e)
                                {
                                    Utils.Log(e);
                                }
                            }

                            OnModified(session, ref state, ref addStateToPrev);
                        }
                        else
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
                            session.Log(item.Overview());
                            addStateToPrev = false;
                            state += "." + index;
                        }
                    }
                    else if (args.Length == 3)
                    {
                        ItemHolder<Item> item = inventory[int.Parse(args[2])];

                        //Item specified
                        if (action.action.Equals("take") && item.amt > 1)
                        {
                            addStateToPrev = false;
                            state += ".take";
                        }
                        else TakeItem(session, item, 1, ref state, ref addStateToPrev);
                    }
                    else if (args.Length == 4)
                    {
                        try
                        {
                            int amt = int.Parse(action.action);
                            ItemHolder<Item> item = inventory[int.Parse(args[2])];
                            TakeItem(session, item, amt, ref state, ref addStateToPrev);
                        }
                        catch
                        {
                            session.Log("Invalid amount");
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

        void OnModified(Session session, ref string state, ref bool addStateToPrev)
        {
            addStateToPrev = false;
            session.Player?.Update();

            if (!inventory.Any())
            {
                state = "interact";
                if (RemoveIfEmpty
                    )
                    Location.objects.Remove(this);
            }
        }

        void TakeItem(Session session, ItemHolder<Item> item, int amt, ref string state, ref bool addStateToPrev)
        {
            if (amt <= 0 || amt > item.amt)
            {
                session.Log($"Invalid amount. Must be between 1 and {item.amt}");
                return;
            }

            item = item.Clone();
            item.amt = amt;

            ItemHolder<Item>? transferred = inventory.Transfer(session.Player.inventory, item);

            if (transferred == null || transferred.amt == 0)
            {
                session.Log($"You cannot carry any more {item.Item?.name}");
            }
            else
            {
                session.Log($"Took {transferred.amt}x {transferred.FormattedName}");

                OnModified(session, ref state, ref addStateToPrev);
            }
        }
    }
}
