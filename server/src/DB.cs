using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class DB
{

    static MongoClient client;
    static IMongoDatabase db;

    public static IMongoCollection<Account> accounts;
    public static IMongoCollection<Player> players;

    public static void Init()
    {
        Utils.Log("Initializing DB...");

        //Allows us to store enums as strings
        BsonSerializer.RegisterSerializer(new EnumSerializer<AbilityScore>(BsonType.String));

        client = new MongoClient(Env.instance.mongoUri);
        db = client.GetDatabase("db");

        accounts = db.GetCollection<Account>("accounts");
        players = db.GetCollection<Player>("players");

        Utils.Log("DB initialized");
    }

    public static class Accounts
    {
        public static Account? Find(ObjectId? id)
        {
            try
            {
                return accounts.FindSync(Builders<Account>.Filter.Eq("_id", id)).First();
            } catch
            {
                return null;
            }
        }

        public static Account? FindByUsername(string username)
        {
            try
            {
                return accounts.FindSync(Builders<Account>.Filter.Eq("username", username)).First();
            } catch
            {
                return null;
            }
        }
    }

    public static class Players
    {
        public static Player? Find(ObjectId? id)
        {
            try
            {
                return players.FindSync(Builders<Player>.Filter.Eq("_id", id)).First();
            } catch (Exception e)
            { 
                Utils.Log(e.Message);
                return null;
            }
        }
    }

}
