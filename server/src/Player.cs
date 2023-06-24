using Events;
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

    public override int MaxHealth => 35 + Constitution * 3;

    public int xp = 0, level = 0;
    public int XpToNextLevel => (int)Math.Round(30 + level * Math.Pow(1.1, level) * 50);

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
    public Session? session;

    public ObjectId _id = ObjectId.GenerateNewId(), accountId;
    public Account? Account => DB.Accounts.Find(accountId);

    public string? resetLocation;
    public Location? Location => Location.Get(location);

    public Player(ObjectId accountId) : base(accountId.ToString(), "Unnamed Player")
    {
        this.accountId = accountId;
        name = Account?.username ?? "Unnamed Player";
    }

    public void Update()
    {
        CalculateStats();
        if (_id != null)
            DB.players.ReplaceOneAsync(Builders<Player>.Filter.Eq("_id", _id), this);
        else Utils.Log("_id is null");
    }

    protected override void OnDie(CreatureDeathEventData data)
    {
        health = MaxHealth;
        if(!location.Equals("afterlife")) Move("afterlife");
    }

    public void AddXp(int amount, string cause)
    {
        xp += amount;
        session?.Log($"You gained {amount} xp from {cause}.");
        
        if(xp > XpToNextLevel)
        {
            session?.Log(Utils.Style("Level up available! Rest to level up", "yellow"));
        }

        Update();
    }

}
