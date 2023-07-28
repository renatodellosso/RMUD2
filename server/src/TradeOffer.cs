using ItemTypes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TradeOffer
{

    ObjectId playerId;
    public Player? Player => Player.Get(playerId);

    public ItemHolder<Item> item;
    public int cost;

    public TradeOffer(Player player, ItemHolder<Item> item, int cost)
    {
        playerId = player._id;
        this.item = item;
        this.cost = cost;
    }

}