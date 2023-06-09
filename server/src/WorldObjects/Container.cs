﻿using ItemTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldObjects
{
    public class Container : WorldObject
    {

        public Inventory inventory = new();

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
                    Utils.AddItemOptionsFromInventory(inputs, inventory);
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
                    else if (args.Length == 3)
                    {
                        //Item specified
                        if (action.action.Equals("take"))
                        {
                            addStateToPrev = false;
                            state += ".take";
                        }
                    }
                    else if (args.Length == 4)
                    {
                        try
                        {
                            ItemHolder<Item>? item = inventory[int.Parse(args[2])];

                            int amt = int.Parse(action.action);
                            if(amt <= 0 || amt > item.amt)
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
                                addStateToPrev = false;
                                state = "back";
                                session.Player.Update();
                            }
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
    }
}
