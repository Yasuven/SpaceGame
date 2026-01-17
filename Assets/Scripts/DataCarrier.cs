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
        { "Area6", true },
        { "Area7", true },
        { "Area8", true },
        { "Area9", true },
        { "Area10", true },
        { "Area11", true },
        { "Area12", true }
    };

    public static PlayerSpaceship playerSpaceship;

    static DataCarrier()
    {
        playerSpaceship = Resources.Load<PlayerSpaceship>("SpaceShip/BasicFighter");
    }

    public static void ResetOpenWorld()
    {
        firstLoad = true;
        points = 0;
        tutorialPassed = false;
        namesWereAssigned = false;

        playerStartPosition = Vector3.zero;

        planetStates.Clear();
        planets.Clear();

        var keys = new List<string>(asteroidAreas.Keys);
        foreach (var key in keys)
        {
            asteroidAreas[key] = true;
        }

        lastEnteredArea = null;

        playerSpaceship = Resources.Load<PlayerSpaceship>("SpaceShip/BasicFighter");
    }

    public static Vector3 ResolvePlayerSpawn(Vector3 fallbackPosition)
    {
        if (playerStartPosition != Vector3.zero)
        {
            Vector3 pos = playerStartPosition;
            playerStartPosition = Vector3.zero;
            return pos;
        }

        return fallbackPosition;
    }


    
}