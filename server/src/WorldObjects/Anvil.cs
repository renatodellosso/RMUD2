using ItemTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldObjects
{
    public class Anvil : WorldObject
    {

        const int REFORGE_COST = Config.Gameplay.REFORGE_COST;

        public Anvil(Location location) : base("anvil", "Anvil", location.id)
        {

        }

        public override List<Input> GetInputs(Player player, string state)
        {
            List<Input> inputs = new()
            {
                new("armor", $"Reforge {player.armor?.FormattedName} for {Utils.Coins(REFORGE_COST)}"),
                new("mainHand", $"Reforge {player.mainHand?.FormattedName} for {Utils.Coins(REFORGE_COST)}"),
                new("offHand", $"Reforge {player.offHand?.FormattedName} for {Utils.Coins(REFORGE_COST)}"),
            };

            return inputs;
        }

        public override void HandleInput(Session session, ClientAction action, ref string state, ref bool addStateToPrev)
        {
            Player player = session.Player!;

            if (player.coins > REFORGE_COST)
            {
                ItemHolder<Item>? item = null;

                if (action.action == "armor") item = player.armor;
                else if (action.action == "mainHand") item = player.mainHand;
                else if (action.action == "offHand") item = player.offHand;

                if(item != null)
                {
                    try
                    {
                        string reforge = action.action == "armor" ? ReforgeList.ARMOR_REFORGES.Get() : ReforgeList.WEAPON_REFORGES.Get();
                        //Reforge overviews start with a line break
                        session.Log($"You reforged your {item.FormattedName} to {Reforge.Get(reforge)?.FormattedName}.{Reforge.Get(reforge)?.Overview()}");

                        if (action.action == "armor") //If item is the armor, we'll have casted it, which removes the reference to the actual armor
                            player.armor.data["reforge"] = reforge;
                        else item.data["reforge"] = reforge;

                        player.coins -= REFORGE_COST;
                        player.Update();
                    }
                    catch (Exception e)
                    {
                        Utils.Log(e);
                    }
                }
            }
            else session.Log("You do not have enough coins.");
        }
    }
}
