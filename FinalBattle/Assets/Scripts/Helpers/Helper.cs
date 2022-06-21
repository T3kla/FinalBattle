using UnityEngine;

public static class Vector3Helper
{

    public static Vector3 RotateAround(this Vector3 point, Vector3 axis, float angle)
    {
        return Quaternion.AngleAxis(angle, axis) * point;
    }

}