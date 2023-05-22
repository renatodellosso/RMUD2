using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct Vector2
{

    public static readonly Vector2[] DIRECTIONS = new Vector2[]
    {
        new(0, 1), new(1, 0), new(0, -1), new(-1, 0),
    };

    public int x, y;

    public Vector2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public bool Equals(Vector2 other)
    {
        return x == other.x && y == other.y;
    }

    //Custom addition
    public static Vector2 operator +(Vector2 a, Vector2 b)
    {
        return new Vector2(a.x + b.x, a.y + b.y);
    }

    public static Vector2 operator -(Vector2 a, Vector2 b)
    {
        return new Vector2(a.x - b.x, a.y - b.y);
    }

    //Casts
    public static implicit operator string(Vector2 vector2)
    {
        return $"({vector2.x}, {vector2.y})";
    }

    public static implicit operator int[](Vector2 vector2)
    {
        return new int[] { vector2.x, vector2.y };
    }

}
