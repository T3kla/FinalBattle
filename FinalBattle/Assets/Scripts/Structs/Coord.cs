using System;
using UnityEngine;

[Serializable]
public struct Coord
{

    public short x;
    public short z;

    public Coord(short x = 0, short z = 0)
    {
        this.x = x;
        this.z = z;
    }

    public Coord(float x = 0f, float z = 0f)
    {
        this.x = (short)x;
        this.z = (short)z;
    }

    public Coord(Vector2 value)
    {
        this.x = (short)value.x;
        this.z = (short)value.y;
    }

    public Coord(Vector3 value)
    {
        this.x = (short)value.x;
        this.z = (short)value.z;
    }

    public static float operator -(Coord a, Coord b)
    {
        return ((float)a.x - (float)b.x) + ((float)a.z - (float)b.z);
    }

}
