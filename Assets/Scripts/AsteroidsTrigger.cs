using UnityEngine;
using UnityEngine.SceneManagement;

public class AsteroidsTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            SceneManager.LoadScene("Asteroids");
        }
    }
}
