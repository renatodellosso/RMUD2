using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Dungeon
{

    static Dictionary<Vector2, Floor> floors = new();

    public static void Generate()
    {
        Utils.Log("Generating dungeon...");

        int floorCount = Utils.RandInt(Config.DungeonGeneration.MIN_FLOORS, Config.DungeonGeneration.MAX_FLOORS+1);
        Utils.Log($"Floor Count: {floorCount}");

        for(int depth = 0; depth < floorCount; depth++)
        {
            Utils.Log($"Generating floor {depth + 1}/{floorCount}");

            Floor floor = new Floor(new(depth, 0));
        }

        Utils.Log("Dungeon generated");
    }

}