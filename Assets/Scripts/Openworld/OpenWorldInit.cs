using System.Collections.Generic;
using UnityEngine;

public class OpenWorldInit : MonoBehaviour
{
    [Header("Setup")]
    public GameObject player;
    public PlayerSpaceship defaultShip;
    public Transform defaultSpawnPoint;

    [Header("Asteroid Areas")]
    public GameObject asteroidAreaPrefab;
    public Transform asteroidAreasParent;

    [Header("Planet Setup")]
    public Transform planetsPositionsParent;

    void Awake()
    {
        if (DataCarrier.playerSpaceship == null) DataCarrier.playerSpaceship = defaultShip;
    }

    void Start()
    {
        //Debug.Log("total points from data carrier are: " + DataCarrier.points);
        HandlePlayerSpawn();
        HandleAsteroidAreas();

        if (DataCarrier.firstLoad)
        {
            InitiatePlanets();
            DataCarrier.firstLoad = false;
        }
        else
        {
            RetrievePlanetsFromList();
        }
    }


    private void InitiatePlanets()
    {
        if (planetsPositionsParent == null)
            return;

        foreach (Transform marker in planetsPositionsParent)
        {
            string planetId = marker.name.Replace("_location", "");

            GameObject planetObj = SpawnPlanet(planetId, marker.position, marker.rotation);
            if (planetObj == null)
                continue;

            Planet comp = planetObj.GetComponent<Planet>();
            if (comp == null)
                continue;

            comp.planetId = planetId;

            SetupPlanet(comp, planetId);

            CreatePlanetState(comp);
            
        }
    }

    private void RetrievePlanetsFromList()
    {
        foreach (PlanetState state in DataCarrier.planetStates)
        {
            GameObject planetObj = SpawnPlanet(state.planetId, state.position, state.rotation);
            if (planetObj == null)
                continue;

            Planet comp = planetObj.GetComponent<Planet>();
            if (comp == null)
                continue;
            
            SetupPlanet(comp, state.planetId, state.dialogueAssetName);
            comp.ApplyState(state);
        }
    }

    private void HandlePlayerSpawn()
    {
        Vector3 spawnPosition;

        if (DataCarrier.playerStartPosition != Vector3.zero)
        {
            spawnPosition = DataCarrier.playerStartPosition;
            DataCarrier.playerStartPosition = Vector3.zero;
        }
        else if (defaultSpawnPoint != null)
        {
            spawnPosition = defaultSpawnPoint.position;
        }
        else
        {
            spawnPosition = player.transform.position;
        }

        player.transform.position = spawnPosition;
    }

    private void HandleAsteroidAreas()
    {
        foreach (Transform child in asteroidAreasParent)
        {
            string areaName = child.name;

            if (!DataCarrier.asteroidAreas.TryGetValue(areaName, out bool isActive) || !isActive)
                continue;

            GameObject asteroidObj = Instantiate(asteroidAreaPrefab, child.position, Quaternion.identity, child);
            asteroidObj.name = areaName;

            var trigger = asteroidObj.GetComponentInChildren<AsteroidsTrigger>();
            if (trigger != null)
                trigger.areaName = areaName;
        }
    }

    private GameObject SpawnPlanet(string planetId, Vector3 position, Quaternion rotation)
    {
        GameObject prefab = Resources.Load<GameObject>($"Prefabs/{planetId}");
        if (prefab == null)
            return null;

        GameObject planetObj = Instantiate(prefab, position, rotation);
        planetObj.name = planetId;
        return planetObj;
    }

    private void SetupPlanet(Planet planetComp, string planetId, string dialogueName = null)
    {
        if (planetComp == null)
            return;
        DataCarrier.planets.Add(planetComp);
        string dialoguePath = $"Dialogues/{dialogueName ?? (planetId + "_dialogue")}";
        DialogueData dialogueAsset = Resources.Load<DialogueData>(dialoguePath);
        if (dialogueAsset != null)
            planetComp.dialogueData = dialogueAsset;

        string conditionPath = $"PlanetConditions/{planetId}_conditions";
        PlanetCondition cond = Resources.Load<PlanetCondition>(conditionPath);
        if (cond != null)
            planetComp.specialConditions = cond;

        string eventsPath = $"PlanetEvents/{planetId}_events";
        PlanetEvents events = Resources.Load<PlanetEvents>(eventsPath);
        if (events != null)
            Debug.Log("we should have loaded events");
            planetComp.events = events;

    }

    private PlanetState CreatePlanetState(Planet planetComp)
    {
        PlanetState state = new PlanetState(planetComp);
        DataCarrier.planetStates.Add(state);
        return state;
    }

    public static void RewritePlanetStates()
    {
        DataCarrier.planetStates.Clear();

        foreach (var planet in DataCarrier.planets)
        {
            if (planet != null)
                DataCarrier.planetStates.Add(planet.GetState());
        }
    }


}
