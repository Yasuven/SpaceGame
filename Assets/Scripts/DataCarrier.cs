using System;
using System.Collections.Generic;
using UnityEngine;

public static class DataCarrier
{
    public static bool firstLoad = true;
    public static int points;
    public static Vector3 playerStartPosition;
    public static bool namesWereAssigned = false;
    public static List<PlanetState> planetStates = new List<PlanetState>();
    public static List<Planet> planets = new List<Planet>();
    public static bool tutorialPassed = false;
    public static string lastEnteredArea;
    public static Dictionary<string, bool> asteroidAreas = new Dictionary<string, bool>
    {
        { "Area1", true },
        { "Area2", true },
        { "Area3", true },
        { "Area4", true },
        { "Area5", true },
        { "Area6", true }
    };

    public static PlayerSpaceship playerSpaceship;

    static DataCarrier()
    {
        playerSpaceship = Resources.Load<PlayerSpaceship>("SpaceShip/BasicFighter");
    }
    
}