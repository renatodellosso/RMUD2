// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;

Console.WriteLine("Starting server...");

JsonSerializer serializer = new JsonSerializer();

if (File.Exists(Config.ENV_PATH))
{
    Console.WriteLine("Reading .env from " + Config.ENV_PATH);
    string env = File.ReadAllText(Config.ENV_PATH);
    Env.instance = JsonConvert.DeserializeObject<Env>(env);
}
else
{
    Console.WriteLine("Writing default .env to " + Config.ENV_PATH);
    File.WriteAllText(Config.ENV_PATH, JsonConvert.SerializeObject(Env.instance));
    System.Environment.Exit(0);
}

DB.Init();

Network.Init();

while (true)
{

}

