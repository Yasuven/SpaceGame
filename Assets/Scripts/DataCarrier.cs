using System.Collections.Generic;
using UnityEngine;

public static class DataCarrier
{
    public static Vector3 playerStartPosition;
    public static bool namesWereAssigned = false;
    public static Dictionary<string, bool> asteroidAreas = new Dictionary<string, bool>
    {
        { "Area1", true },
        { "Area2", true },
        { "Area3", true }
    };
}