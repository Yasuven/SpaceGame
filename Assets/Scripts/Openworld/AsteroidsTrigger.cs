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
            DataCarrier.asteroidAreas[areaName] = false;
            DataCarrier.lastEnteredArea = areaName;
            OpenWorldInit.RewritePlanetStates();  
            SceneManager.LoadScene("Asteroids");
        }
    }

}
