using UnityEngine;
using UnityEngine.SceneManagement;

public class AsteroidsTrigger : MonoBehaviour
{
    public string areaName; 
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            DataCarrier.playerStartPosition = other.transform.position;

                if (DataCarrier.asteroidAreas.ContainsKey(areaName))
                {
                    Debug.Log("we entered: " + areaName);
                    DataCarrier.asteroidAreas[areaName] = false;
                }
                DataCarrier.asteroidAreas[areaName] = false;
            
            /*
            foreach (var kvp in DataCarrier.asteroidAreas)
            {
                //Debug.Log($"[DataCarrierDebugger] {kvp.Key} = {kvp.Value}");
            }
            */
            OpenWorldInit.RewritePlanetStates();  
            SceneManager.LoadScene("Asteroids");
        }
    }

}
