using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public class Player : Creature
{

    //Static stuff
    static Dictionary<ObjectId, Player> players = new Dictionary<ObjectId, Player>();
    
    public static Player Get(ObjectId id)
    {
        if (players.ContainsKey(id))
            return players[id];
        else return DB.Players.Find(id);
    }

    public static void Add(Player player)
    {
        //Use TryAdd instead of Add to avoid looking up the key twice
        if(!players.TryAdd(player._id, player))
            players[player._id] = player;
    }
    //End of static stuff

    [BsonIgnore]
    public Session session;

    public ObjectId _id = ObjectId.GenerateNewId(), accountId;
    public Account Account => DB.Accounts.Find(accountId);

    public string resetLocation;
    public Location? Location => Location.Get(location);

    public Player(ObjectId accountId)
    {
        this.accountId = accountId;
    }

    public void Update()
    {
        if (_id != null)
            DB.players.ReplaceOneAsync(Builders<Player>.Filter.Eq("_id", _id), this);
        else Utils.Log("_id is null");
    }

}
