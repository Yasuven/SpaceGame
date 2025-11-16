using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public Asteroid asteroidPrefab;
    public float trajectoryVariance = 15f;
    public float spawnRate = 2f;
    public float spawnDistance = 10f;
    public int spawnAmount = 1;


    void Start()
    {
        InvokeRepeating(nameof(Spawn), 0f, spawnRate);
    }

    // Spawns asteroids at random positions around the spawner
    private void Spawn()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * spawnDistance;
            Vector3 spawnPoint = transform.position + spawnDirection;

            float variance = Random.Range(-trajectoryVariance, trajectoryVariance);
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

            Asteroid asteroid = Instantiate(asteroidPrefab, spawnPoint, rotation);
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);

            asteroid.SetTrajectory(rotation * -spawnDirection);
        }
    }
}
