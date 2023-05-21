using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Exit
{

    //Where the exit leads
    public string location, direction, state = "";

    public Func<Creature, bool> canExit = (creature) => true;

    public Exit(string location, string direction)
    {
        this.location = location;
        this.direction = direction;
    }

    public Exit(string location, string direction, Func<Creature, bool> canExit)
    {
        this.location = location;
        this.direction = direction;
        this.canExit = canExit;
    }

    public Exit Reverse(Location location)
    {
        return new Exit(location.id, Utils.ReverseDirection(direction), canExit);
    }

}
