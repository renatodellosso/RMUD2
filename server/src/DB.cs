using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class DB
{

    static MongoClient client;
    static IMongoDatabase database;

    public static void Init()
    {
        Console.WriteLine("Initializing DB...");

        client = new MongoClient(Env.instance.mongoUri);
        client.GetDatabase("db");

        Console.WriteLine("DB initialized");
    }

}
