using UnityEngine;
using UnityEngine.SceneManagement;

public class AsteroidsTrigger : MonoBehaviour
{
    public string areaName; 
    private void OnTriggerEnter2D(Collider2D other)
{
    Debug.Log($"[AsteroidsTrigger] Something entered trigger: {other.name}");

    if (other.CompareTag("Player"))
    {
        Debug.Log("[AsteroidsTrigger] Player entered trigger");

        DataCarrier.playerStartPosition = other.transform.position;

            if (DataCarrier.asteroidAreas.ContainsKey(areaName))
            {
                Debug.Log("we entered: " + areaName);
                DataCarrier.asteroidAreas[areaName] = false;
            }
            DataCarrier.asteroidAreas[areaName] = false;
        
        Debug.Log("[DataCarrierDebugger] ---- Asteroid Area States ----");
        foreach (var kvp in DataCarrier.asteroidAreas)
        {
            Debug.Log($"[DataCarrierDebugger] {kvp.Key} = {kvp.Value}");
        }
            Debug.Log("[DataCarrierDebugger] ---------------------------------");
        
        SceneManager.LoadScene("Asteroids");
    }
}

}
