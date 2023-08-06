using Items;
using ItemTypes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldObjects
{
    public class SoulAnvil : WorldObject
    {

        public SoulAnvil(Location location) : base("soulanvil", Utils.Style("Soul Anvil", "darkred"), location.id)
        {

        }

        public override List<Input> GetInputs(Player player, string state)
        {
            List<Input> inputs = new();

            if (state == "interact." + id)
            {
                inputs = new()
                {
                    new("armor", $"Reforge {player.armor?.FormattedName} with {Utils.Style("Soul Coins", "darkred")}"),
                    new("mainHand", $"Reforge {player.mainHand?.FormattedName} with {Utils.Style("Soul Coins", "darkred")}"),
                    new("offHand", $"Reforge {player.offHand?.FormattedName} with  {Utils.Style("Soul Coins", "darkred")}"),
                };
            }
            else
            {
                inputs = new();

                ItemHolder<Item>? item = null;

                if (state.EndsWith("armor")) item = player.armor;
                else if (state.EndsWith("mainHand")) item = player.mainHand;
                else if (state.EndsWith("offHand")) item = player.offHand;

                if (player.CraftingInventory.Where(i => i.id == "soulcoin1").Any())
                    inputs.Add(new("soulcoin1", $"Reforge {item?.FormattedName} with {ItemList.Get("soulcoin1")?.FormattedName}"));
                if (player.CraftingInventory.Where(i => i.id == "soulcoin2").Any())
                    inputs.Add(new("soulcoin2", $"Reforge {item?.FormattedName} with {ItemList.Get("soulcoin2")?.FormattedName}"));
                if (player.CraftingInventory.Where(i => i.id == "soulcoin3").Any())
                    inputs.Add(new("soulcoin2", $"Reforge {item?.FormattedName} with {ItemList.Get("soulcoin3")?.FormattedName}"));
            }

            return inputs;
        }

        public override void HandleInput(Session session, ClientAction action, ref string state, ref bool addStateToPrev)
        {
            Player player = session.Player!;

            if (state != "interact." + id)
            {
                if (action.action == "back")
                {
                    state = "interact." + id;
                    addStateToPrev = true;
                }
                else
                {
                    string tier = action.action;
                    int tierNum = int.Parse(tier[^1].ToString(), CultureInfo.InvariantCulture);

                    ItemHolder<Item>? item = null;

                    if (state.EndsWith("armor")) item = player.armor;
                    else if (state.EndsWith("mainHand")) item = player.mainHand;
                    else if (state.EndsWith("offHand")) item = player.offHand;

                    if (item != null)
                    {
                        try
                        {
                            List<KeyValuePair<float, string>> reforges = new();

                            for(int i = 0; i < tierNum; i++)
                            {
                                if(state.EndsWith("armor"))
                                    reforges.Add(ReforgeList.SOUL_ARMOR_REFORGES.GetWithWeight());
                                else
                                    reforges.Add(ReforgeList.SOUL_WEAPON_REFORGES.GetWithWeight());
                            }

                            reforges = reforges.OrderByDescending(r => r.Key).ToList();

                            string msg = "You rolled: ";
                            foreach(KeyValuePair<float, string> r in reforges)
                                msg += $"{Reforge.Get(r.Value)?.FormattedName}, ";
                            msg = msg[..^2]; //Remove last comma and space
                            session.Log(msg);

                            string reforge = reforges.Last().Value;

                            //Reforge overviews start with a line break
                            session.Log($"You reforged your {item.FormattedName} to {Reforge.Get(reforge)?.FormattedName}.{Reforge.Get(reforge)?.Overview()}");

                            if (state.EndsWith("armor")) //If item is the armor, we'll have casted it, which removes the reference to the actual armor
                                player.armor.data["reforge"] = reforge;
                            else item.data["reforge"] = reforge;

                            player.CraftingInventory.Remove(new ItemHolder<Item>(tier, 1));

                            player.Update();
                        }
                        catch (Exception e)
                        {
                            Utils.Log(e);
                        }
                    }
                }
            }
            else
            {
                state += "." + action.action;
            }
        }
    }
}
