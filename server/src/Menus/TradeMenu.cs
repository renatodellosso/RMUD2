using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Menus
{
    public class TradeMenu : Menu
    {

        public override bool ShowSidebar => true;

        public override Input[] GetInputs(ServerResponse response)
        {
            string[] args = state.Split('.');

            List<Input> inputs = new()
            {
                back
            };
            
            Player player = session?.Player!;

            if(state == "")
            {
                inputs.Add(new("self", "View your offers"));
                inputs.Add(new("other", "View other player's offers"));
            }
            else if (state == "self")
            {
                for (int i = 0; i < player.tradeOffers.Count; i++)
                {
                    TradeOffer offer = player.tradeOffers[i];
                    inputs.Add(new(InputMode.Option, i.ToString(), $"{offer.item.FormattedName} x{offer.item.amt} - {Utils.Coins(offer.cost)}"));
                }
            }
            else if (state == "other")
            {
                TradeOffer[] offers = GetOtherTradeOffers();
                for(int i = 0; i < offers.Length; i++)
                {
                    TradeOffer offer = offers[i];
                    inputs.Add(new(InputMode.Option, i.ToString(), $"{offer.item.FormattedName} x{offer.item.amt} - {Utils.Coins(offer.cost)}"));
                }
            }
            else if (args[0] == "self")
            {
                int index = int.Parse(args[1]);
                TradeOffer offer = player.tradeOffers[int.Parse(args[1])];

                inputs.Add(new("cancel", "Cancel offer"));
            }

            return inputs.ToArray();
        }

        public override void HandleInput(ClientAction action, ServerResponse response)
        {
            Player player = session?.Player!;
            string[] args = state.Split('.');

            if (state == "")
            {
                if (action.action == "back")
                    Exit();
                else if (action.action == "self")
                    state = "self";
                else if (action.action == "other")
                    state = "other";
            }
            else
            {
                if (args.Length == 1)
                {
                    if(action.action == "back")
                        state = "";
                    else if (args[0] == "self")
                    {
                        Utils.Log("Selecting trade...");
                        try
                        {
                            int index = int.Parse(action.action);
                            Utils.Log("Index: " + index);
                            state += $".{index}";
                            TradeOffer offer = player.tradeOffers[index];
                            Utils.Log("Got offer");
                            session?.Log($"Offering {offer.item.FormattedName} x{offer.item.amt} for {Utils.Coins(offer.cost)}");
                        }
                        catch (Exception e)
                        {
                            Utils.Log(e);
                        }
                        Utils.Log("Trade selected");

                        return;
                    }
                    else if (args[0] == "other")
                    {
                        int index = int.Parse(action.action);
                        TradeOffer[] offers = GetOtherTradeOffers();
                        TradeOffer offer = offers[index];

                        if(player.coins < offer.cost)
                        {
                            session?.Log("You don't have enough coins to buy this item");
                            return;
                        }

                        player.coins -= offer.cost;
                        player.inventory.Add(offer.item);

                        Player otherPlayer = offer.Player!;
                        otherPlayer.tradeOffers.Remove(offer);
                        otherPlayer.coins += offer.cost;

                        player.Update();
                        otherPlayer.Update();

                        session?.Log($"Bought {offer.item.FormattedName} x{offer.item.amt} for {Utils.Coins(offer.cost)} from {otherPlayer.FormattedName}");
                        otherPlayer.session?.Log($"Sold {offer.item.FormattedName} x{offer.item.amt} for {Utils.Coins(offer.cost)} to {player.FormattedName}");
                    }
                }
                else if (args.Length == 2)
                {
                    if (action.action == "back")
                        state = args[0];
                    else if (args[0] == "self" && action.action == "cancel")
                    {
                        TradeOffer offer = player.tradeOffers[int.Parse(args[1])];

                        player.inventory.Add(offer.item);
                        player.tradeOffers.Remove(offer);
                        player.Update();

                        session?.Log("Offer cancelled");
                        state = args[0];
                    }
                }
            }
        }

        void Exit()
        {
            session?.SetMenu(new LocationMenu(session));
        }

        TradeOffer[] GetOtherTradeOffers()
        {
            Player player = session?.Player!;
            List<TradeOffer> offers = new();

            foreach(Player p in player.Location.Players)
            {
                if (p != player)
                {
                    offers.AddRange(p.tradeOffers);
                }
            }

            return offers.ToArray();
        }
    }
}