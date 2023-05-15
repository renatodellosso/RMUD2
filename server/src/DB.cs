using MongoDB.Bson;
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

    public static void Init()
    {
        Utils.Log("Initializing DB...");

        client = new MongoClient(Env.instance.mongoUri);
        db = client.GetDatabase("db");

        accounts = db.GetCollection<Account>("accounts");

        Utils.Log("DB initialized");
    }

}
