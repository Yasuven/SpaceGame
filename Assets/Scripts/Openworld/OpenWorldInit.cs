using UnityEngine;

public class OpenWorldInit : MonoBehaviour
{
    public GameObject player;
    public Transform defaultSpawnPoint;
    public GameObject asteroidAreaPrefab;  
    public Transform asteroidAreasParent; 

    void Start()
    {
        HandlePlayerSpawn();
        HandleAsteroidAreas();
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
            spawnPosition = player.transform.position; // just in case
        }

        player.transform.position = spawnPosition;
    }

        
     // I have spent like 3 hrs on this bullshit dumb sollution... we need to rework it for sure   
    private void HandleAsteroidAreas()
    {
        foreach (Transform child in asteroidAreasParent)
        {
            string areaName = child.name; 

            if (!DataCarrier.asteroidAreas.ContainsKey(areaName)) continue;
            
            bool isActive = DataCarrier.asteroidAreas[areaName];

            if (isActive)
            {
                GameObject asteroidObj = Instantiate(asteroidAreaPrefab, child.position, Quaternion.identity, child);
                asteroidObj.name = areaName;

                var trigger = asteroidObj.GetComponentInChildren<AsteroidsTrigger>();
                if (trigger != null) trigger.areaName = areaName;
            }
        }
    }

}
