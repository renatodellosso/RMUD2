using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Exit
{

    //Where the exit leads
    public string location, direction, state = "";

    public Func<Creature, Exit, bool> canExit = (creature, exit) =>
    {
        return !(Location.Get(exit.location)?.safe ?? false) || (!creature.tags?.Contains("hostile") ?? true);
    };

    public Exit(string location, string direction)
    {
        this.location = location;
        this.direction = direction;
    }

    public Exit(string location, string direction, Func<Creature, Exit, bool> canExit)
    {
        this.location = location;
        this.direction = direction;
        this.canExit = canExit;
    }

    public Exit Reverse(Location location)
    {
        return new Exit(location.id, Utils.ReverseDirection(direction), canExit);
    }

    /// <summary>
    /// Adds an exit the start location, and, if twoWay is true, the reverse exit to the end location
    /// </summary>
    public static void AddExit(Location? start, Location? end, string direction, bool twoWay = true)
    {
        Exit exit = new(end.id, direction);
        start?.exits.Add(exit);
        
        if(twoWay) end?.exits.Add(exit.Reverse(start));
    }

}
