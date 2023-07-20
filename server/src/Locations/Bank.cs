using ItemTypes;
using Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Locations
{
    public class Bank : Location
    {

        protected override string Description => "The cold stone building has several dozen metal safes built into the wall.";

        public Bank()
        {
            id = "bank";
            name = "Bank";
            status = "In Town";

            safe = true;

            creatures.Add(new Creatures.SimpleNPC("banker", "Benas, Banker", "gold", TalkInputs, TalkHandler, TalkStart));
        }

        Input[] TalkInputs(Session session, DialogueMenu menu)
        {
            List<Input> inputs = new();
            string[] args = menu.state.Split('.');

            Player player = session.Player!;

            if (menu.state == "")
            {
                //The option to leave/go back always goes first
                inputs.Add(new(InputMode.Option, "leave", "Goodbye"));

                int level = player?.vault?.level ?? -1;

                if (level < Vault.SIZES.Length - 1)
                {
                    KeyValuePair<int, int> nextTier = Vault.SIZES[level + 1];

                    string upgradeMsg = level + 1 > 0 ? "Upgrade to a " : "Purchase a ";
                    upgradeMsg += Utils.Weight(nextTier.Value) + " vault for " + Utils.Coins(nextTier.Key);

                    inputs.Add(new(InputMode.Option, "upgrade", upgradeMsg));
                }

                if (player?.vault != null)
                    inputs.Add(new(InputMode.Option, "access", "Access vault"));
            }
            else
            {
                inputs.Add(menu.back);

                if (args[0] == "access")
                {
                    if(args.Length == 1)
                    {
                        if(player.vault?.Weight < player.vault?.MaxWeight)
                            inputs.Add(new(InputMode.Option, "deposit", "Deposit items"));

                        for (int i = 0; i < player.vault?.Count; i++)
                            inputs.Add(new(InputMode.Option, i.ToString(), player.vault[i].FormattedName));
                    }
                    else if (args[1] == "deposit")
                    {
                        if(args.Length == 2)
                        {
                            for (int i = 0; i < player.inventory.Count; i++)
                                inputs.Add(new(InputMode.Option, i.ToString(), player.inventory[i].FormattedName));
                        }
                        else if(args.Length == 3)
                        {
                            try
                            {
                                int index = int.Parse(args[2]);
                                ItemHolder<Item>? item = player?.inventory[index];

                                Utils.AddItemAmountOptions(inputs, item);
                            }
                            catch
                            {

                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            int index = int.Parse(args[1]);
                            ItemHolder<Item>? item = player?.vault[index];

                            Utils.AddItemAmountOptions(inputs, item);
                        }
                        catch
                        {

                        }
                    }
                }
            }

            return inputs.ToArray();
        }

        void TalkHandler(Session session, ClientAction action, DialogueMenu menu)
        {
            string[] args = menu.state.Split('.');
            Player player = session.Player!;

            int level = player?.vault?.level ?? -1;
            KeyValuePair<int, int> nextTier = Vault.SIZES[level + 1];

            if (menu.state == "")
            {
                if (action.action == "upgrade" && level < Vault.SIZES.Length - 1)
                {
                    if (player?.coins >= nextTier.Key)
                    {
                        player.coins -= nextTier.Key;

                        if(player.vault == null)
                            player.vault = new Vault(level + 1);
                        else
                        {
                            player.vault.level++;
                            player.vault.CalculateStats();
                        }

                        player.Update();

                        session.Log($"Acquired a {Utils.Weight(player.vault.maxWeight)} vault!");
                    }
                    else
                        session.Log("You cannot afford that.");
                }
                else if (action.action == "access")
                {
                    menu.state = "access";

                    session.Log(player?.vault?.GetText());
                }
                else if (action.action == "leave")
                    menu.state = "exit";
            }
            else if (args[0] == "access")
            {
                if (args.Length == 1)
                {
                    if (action.action == "back")
                        menu.state = "";
                    else if (action.action == "deposit" && player?.vault?.Weight < player?.vault?.MaxWeight)
                    {
                        menu.state += ".deposit";
                    }
                    else
                    {
                        //Accessing existing item in vault
                        try
                        {
                            int index = int.Parse(action.action);

                            menu.state += "." + index;
                            ItemHolder<Item>? item = player?.vault[index];

                            if (item != null)
                            {
                                session.Log(item.Overview(player));
                            }
                        }
                        catch
                        {
                            session.Log("Invalid index");
                        }
                    }
                }
                else if (args[1] == "deposit")
                {
                    if (args.Length == 2 && action.action == "back")
                        menu.state = "access";
                    else
                    {
                        if (args.Length == 2)
                        {
                            try
                            {
                                int index = int.Parse(action.action);

                                menu.state += "." + index;
                                ItemHolder<Item>? item = player?.inventory[index];

                                if (item != null)
                                {
                                    session.Log(item.Overview(player));
                                }
                            }
                            catch
                            {
                                session.Log("Invalid index");
                            }
                        }
                        else
                        {
                            if (action.action == "back")
                                menu.state = "access.deposit";
                            else
                            {
                                try
                                {
                                    int amt = int.Parse(action.action);

                                    int index = int.Parse(args[2]);
                                    ItemHolder<Item>? item = player?.inventory[index];

                                    if (amt < 0 || amt > item?.amt)
                                        session.Log("Invalid input");
                                    else
                                    {
                                        ItemHolder<Item> clone = item.Clone();
                                        clone.amt = amt;
                                        ItemHolder<Item>? transferred = player?.inventory.Transfer(player.vault, clone);

                                        if (transferred != null)
                                        {
                                            session.Log($"Deposited {transferred.FormattedName} x{transferred.amt}");

                                            menu.state = "access.deposit";
                                        }
                                        else session.Log("Could not deposit item.");
                                    }
                                }
                                catch
                                {
                                    session.Log("Invalid input.");
                                }
                            }
                        }
                    }
                }
                else if(args.Length == 2)
                {
                    if (action.action == "back")
                        menu.state = "access";
                    else
                    {
                        int amt = int.Parse(action.action);

                        int index = int.Parse(args[1]);
                        ItemHolder<Item>? item = player?.vault[index];

                        if (amt < 0 || amt > item?.amt)
                            session.Log("Invalid input");
                        else
                        {
                            ItemHolder<Item> clone = item.Clone();
                            clone.amt = amt;
                            ItemHolder<Item>? transferred = player?.vault.Transfer(player.inventory, clone);

                            if (transferred != null)
                            {
                                session.Log($"Withdrew {transferred.FormattedName} x{transferred.amt}");

                                menu.state = "access";
                            }
                            else session.Log("Could not withdraw item.");
                        }
                    }
                }
            }
        }

        void TalkStart(Session session)
        {
            Player player = session.Player!;

            player.vault?.CalculateStats();

            session.Log(Utils.Dialogue(creatures.First(), "Good day to you."));

            int level = player?.vault?.level ?? -1;

            if (level < Vault.SIZES.Length - 1)
            {
                KeyValuePair<int, int> nextTier = Vault.SIZES[level + 1];

                session.Log(Utils.Dialogue(creatures.First(), $"Can I interest you in a {Utils.Weight(nextTier.Value)} vault for only {Utils.Coins(nextTier.Key)}?"));
            }
        }

    }
}