using Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Dungeon
{

    public static Dictionary<Vector2, Floor> floors;

    public static string startLocation = "";

    public static event Action? OnDungeonGenerated;

    public static void Generate()
    {
        Utils.Log("Generating dungeon...");

        int floorCount = Utils.RandInt(Config.DungeonGeneration.MIN_FLOORS, Config.DungeonGeneration.MAX_FLOORS+1);
        Utils.Log($"Floor Count: {floorCount}");

        floors = new();

        List<Task> tasks = new();

        for(int depth = 0; depth < floorCount; depth++)
        {
            Utils.Log($"Generating floor {depth + 1}/{floorCount}");
            Vector2 pos = new(depth, 0); // Vector2 pos of the floor

            Floor floor = new Floor(pos);
            floors.Add(pos, floor);
            tasks.Add(Task.Run(floor.GenerateFloor));
        }

        WaitForCompletion(tasks);

        Utils.Log("Generating stairs...");
        foreach(Floor floor in floors.Values)
        {
            tasks.Add(Task.Run(floor.GenerateStairs));
        }

        startLocation = floors.First().Value.PosToId(floors.First().Value.startPos);

        Utils.Log("Calling OnDungeonGenerated handler...");
        OnDungeonGenerated?.Invoke();

        Utils.Log("Dungeon generated");
    }

    static void WaitForCompletion(List<Task> tasks)
    {
        while(tasks.Where(t => !t.IsCompleted).Any())
        {
            Thread.Sleep(Config.DungeonGeneration.SLEEP_INTERVAL);
            Utils.Log($"Waiting for {tasks.Where(t => !t.IsCompleted).Count()} tasks to complete...");
        }
    }

    public static async void Reset()
    {
        try
        {
            Utils.Log("Resetting dungeon...");
            Utils.Announce("Dungeon resetting...");

            Utils.Log("Starting task...");

            await Task.Run(RemoveDungeon).ContinueWith(task => Generate());

            Location.Get("dungeonentrance")!.AddExits();

            Utils.Announce("Dungeon reset!");
        } catch(Exception e)
        {
            Utils.Log(e);
        }
    }

    static void RemoveDungeon()
    {
        Utils.Log("Removing entrance to dungeon...");
        DungeonEntrance dungeonEntrance = (DungeonEntrance)Location.Get("dungeonentrance")!;
        Exit dungeonExit = dungeonEntrance.exits.Where(e => e.location == startLocation).First();
        dungeonEntrance.exits.Remove(dungeonExit);

        Utils.Log("Removing players from dungeon...");
        foreach(Player player in Player.players.Values)
        {
            if (player.Location is DungeonLocation)
                player.Move("dungeonentrance", true);
        }

        Utils.Log("Removing dungeon locations...");
        foreach(Floor floor in floors.Values)
        {
            foreach(Location location in floor.locations)
            {
                foreach(Creature creature in location.creatures)
                {
                    if(creature is not Player)
                        creature.Die(null);
                }
                Location.Remove(location);
            }
        }

        Utils.RemoveDungeonCreatures();
    }

}