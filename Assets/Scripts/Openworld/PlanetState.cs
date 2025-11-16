using UnityEngine;

[System.Serializable]
public class PlanetState
{
    public string planetId;
    public Vector3 position;
    public Quaternion rotation; // no clue why, but OpenWorldInit breaks down without rotation here :/
    public int currentNode;
    public string dialogueAssetName;

    public PlanetState() {}

    public PlanetState(Planet planet)
    {
        planetId = planet.planetId;
        position = planet.transform.position;
        currentNode = planet.currentNode;
        dialogueAssetName = planet.dialogueData != null ? planet.dialogueData.name : "";
    }
}
