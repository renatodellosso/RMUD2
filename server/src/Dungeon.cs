using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Dungeon
{

    static Dictionary<Vector2, Floor> floors = new();

    public static string startLocation = "";

    public static event Utils.DefaultDelegate? OnDungeonGenerated;

    public static void Generate()
    {
        Utils.Log("Generating dungeon...");

        int floorCount = Utils.RandInt(Config.DungeonGeneration.MIN_FLOORS, Config.DungeonGeneration.MAX_FLOORS+1);
        Utils.Log($"Floor Count: {floorCount}");

        List<Task> tasks = new();

        for(int depth = 0; depth < floorCount; depth++)
        {
            Utils.Log($"Generating floor {depth + 1}/{floorCount}");
            Vector2 pos = new(depth, 0); // Vector2 pos of the floor

            Floor floor = new Floor(pos);
            floors.Add(pos, floor);
            tasks.Add(Task.Run(floor.GenerateFloor));
        }

        while(tasks.Where(t => !t.IsCompleted).Any())
        {
            Thread.Sleep(Config.DungeonGeneration.SLEEP_INTERVAL);
            Utils.Log($"Waiting for {tasks.Where(t => !t.IsCompleted).Count()} tasks to complete...");
        }

        startLocation = floors.First().Value.PosToId(floors.First().Value.startPos);

        Utils.Log("Calling OnDungeonGenerated handler...");
        OnDungeonGenerated?.Invoke();

        Utils.Log("Dungeon generated");
    }

}