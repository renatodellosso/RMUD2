using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Floor
{

    public Location[,] locations;

    public Vector2 position; //x is depth, y is 0 if it's the floor, or >0 if it's a side floor

    public Floor(Vector2 position, Vector2 size)
    {
        this.position = position;
        locations = new Location[size.x, size.y];
    }

}
