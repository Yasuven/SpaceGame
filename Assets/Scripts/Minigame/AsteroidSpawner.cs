using System.Collections;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public Asteroid asteroidPrefab;
    public float trajectoryVariance = 15f;
    public float NumberOfEnemiesPerWave = 5;
    public float spawnRate = 2f;
    public float spawnDistance = 12f;
    public int spawnAmount = 1;

    public int NumberOfEnemiesGrowth = 2;
    public float SpawnRateGrowth = 0.2f;
    public int SpawnAmountGrowth = 1;

    private WaveData Wave;

    private void Awake()
    {
        Events.OnLevelStart += OnLevelStart;
    }

    private void OnDestroy()
    {
        Events.OnLevelStart -= OnLevelStart;
    }

    public void OnLevelStart(WaveData wave)
    {
        Debug.Log("Wave started, configuring spawner.");
        Wave = wave;
        spawnRate = Wave.spawnRate;
        spawnAmount = Wave.spawnAmount;
        NumberOfEnemiesPerWave = Wave.NumberOfEnemies;
        NumberOfEnemiesGrowth = Wave.NumberOfEnemiesGrowth;
        SpawnRateGrowth = Wave.SpawnRateGrowth;
        SpawnAmountGrowth = Wave.SpawnAmountGrowth;
    }
    private void Start()
    {
        StartCoroutine(WaveLoop());
    }

    private IEnumerator WaveLoop()
    {
        while (true)
        {
            yield return StartCoroutine(SpawnWave());
            ApplyWaveGrowth();

            // Optional: time between waves
            yield return new WaitForSeconds(2f);
        }
    }

    private IEnumerator SpawnWave()
    {
        int spawnedThisWave = 0;

        while (spawnedThisWave < NumberOfEnemiesPerWave)
        {
            for (int i = 0; i < spawnAmount; i++)
            {
                if (spawnedThisWave >= NumberOfEnemiesPerWave)
                    break;

                SpawnOneAsteroid();
                spawnedThisWave++;
            }

            yield return new WaitForSeconds(spawnRate);
        }
    }

    // Spawns asteroids at random positions around the spawner
    private void SpawnOneAsteroid()
    {
        Vector3 spawnDirection = Random.insideUnitCircle.normalized * spawnDistance;
        Vector3 spawnPoint = transform.position + spawnDirection;

        float variance = Random.Range(-trajectoryVariance, trajectoryVariance);
        Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

        Asteroid asteroid = Instantiate(asteroidPrefab, spawnPoint, rotation);
        asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);

        asteroid.SetTrajectory(rotation * -spawnDirection);
        
    }

    private void ApplyWaveGrowth()
    {
        NumberOfEnemiesPerWave += NumberOfEnemiesGrowth;

        spawnRate = Mathf.Max(0.1f, spawnRate - SpawnRateGrowth);
        // smaller spawnRate = faster waves, so growth *reduces* rate

        spawnAmount = Mathf.Min(10, spawnAmount + SpawnAmountGrowth);

        Debug.Log($"New Wave Settings: Enemies={NumberOfEnemiesPerWave}, SpawnRate={spawnRate}, SpawnAmount={spawnAmount}");
    }

}
