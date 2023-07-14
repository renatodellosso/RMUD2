// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using System.Diagnostics;

Utils.Log("Starting server...");

JsonSerializer serializer = new JsonSerializer();

if (File.Exists(Config.ENV_PATH))
{
    Utils.Log("Reading .env from " + Config.ENV_PATH);
    string env = File.ReadAllText(Config.ENV_PATH);
    Env.instance = JsonConvert.DeserializeObject<Env>(env);
}
else
{
    Utils.Log("Writing default .env to " + Config.ENV_PATH);
    File.WriteAllText(Config.ENV_PATH, JsonConvert.SerializeObject(Env.instance));
    System.Environment.Exit(0);
}

DB.Init();
AI.Init();

Dungeon.Generate();
Location.GenerateExits();

Network.Init();
Bot.Init();

Task.Run(Utils.RemoveInactiveSessions);

while (true)
{
    Thread.Sleep(Utils.Tick());
}

