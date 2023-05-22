using Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Floor
{

    public DungeonLocation[,] locations;
    List<DungeonLocation> rooms = new();

    public Vector2 position; //x is depth, y is 0 if it's the floor, or >0 if it's a side floor
    Vector2 size;
    public Vector2 startPos; //The starting position of layout generation

    public float temp = 0;

    public Floor(Vector2 position)
    {
        this.position = position;

        Vector2 size = Utils.RandVector2(Config.DungeonGeneration.MIN_FLOOR_SIZE, Config.DungeonGeneration.MAX_FLOOR_SIZE);
        this.size = size;

        locations = new DungeonLocation[size.x, size.y];
        startPos = new(size.x/2, size.y/2);

        temp = Utils.RandFloat(-1, 1);

        //Apparently we have to cast Vector2s to string to get the formatting, even though I made an implicit converter. TODO: Look into this
        Utils.Log($"Floor {(string)position} Pre-Generation Stats:" +
            $"\n\tSize: {(string)size} ({size.x * size.y} area, {size.x * size.y * Config.DungeonGeneration.MIN_FILL} min rooms)" +
            $"\n\tTemp: {temp}");

        GenerateFloor();

        Utils.Log($"Floor {(string)position} Post-Generation Stats:" +
            $"\n\tRooms: {rooms.Count}");
    }

    public DungeonLocation GetLocation(Vector2 position)
    {
        return locations[position.x, position.y];
    }


    /// <returns>Returns whether the given coordinates represent a valid location or not</returns>
    public bool IsPosValid(Vector2 pos)
    {
        if(pos.x < 0 || pos.y < 0) return false;
        if(pos.x >= locations.GetLength(0) || pos.y >= locations.GetLength(1)) return false;

        return true;
    }

    //Dungeon generation

    //Overall handler for generating the locations, creatures, and contents of the floor
    void GenerateFloor()
    {
        //Generate the layout before we start populating it
        GenerateLocations();
    }

    void GenerateLocations()
    {
        List<DungeonLocation> toGen = new(); //All the locations we need to generate exits for

        locations[startPos.x, startPos.y] = new(position, startPos); //Not sure if there's a better way to access the array
        toGen.Add(GetLocation(startPos));
        rooms.Add(GetLocation(startPos));

        int minRooms = (int)Math.Round(size.x * size.y * Config.DungeonGeneration.MIN_FILL);

        while(toGen.Any() || rooms.Count < minRooms)
        {
            //Make sure we generate enough rooms
            if(!toGen.Any()) 
            {
                toGen.Add(rooms[Utils.RandInt(0, rooms.Count)]);
            }

            DungeonLocation room = toGen.First();

            foreach(Vector2 dir in Vector2.DIRECTIONS)
            {
                Vector2 pos = room.position + dir; //Position of the adjacent room
                if(IsPosValid(pos) && Utils.RandFloat() < Config.DungeonGeneration.EXIT_CHANCE)
                {
                    if (locations[pos.x, pos.y] == null)
                    {
                        //Adding a new room
                        DungeonLocation newRoom = new(position, pos);
                        locations[pos.x, pos.y] = newRoom;
                        toGen.Add(newRoom);
                        rooms.Add(newRoom);

                        //Utils.Log($"Adding room... Floor {(string)position}: {(string)pos}");
                    }

                    DungeonLocation otherRoom = locations[pos.x, pos.y];

                    if (otherRoom != null)
                    {
                        //Add exits
                        Exit.AddExit(room, otherRoom, Utils.Vector2ToDir(pos - room.position));
                    }
                }
            }

            toGen.Remove(room);

        }
    }

    //End dungeon generation

}
