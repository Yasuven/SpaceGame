using System.Collections;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public Asteroid asteroidPrefab;
    private AsteroidData asteroidData;

    public float trajectoryVariance = 15f;
    public float NumberOfEnemiesPerWave = 5;
    public float spawnRate = 2f;
    public float spawnDistance = 12f;
    public int spawnAmount = 1;

    public int NumberOfEnemiesGrowth = 2;
    public float SpawnRateGrowth = 0.2f;
    public int SpawnAmountGrowth = 1;
    public int MaxEnemies = 100;

    private int _currentWaveIndex = 0;
    private int _totalSpawnedThisWave = 0;

    private WaveData[] Waves;

    private void Awake()
    {
        Events.OnLevelStart += OnLevelStart;
    }

    private void OnDestroy()
    {
        Events.OnLevelStart -= OnLevelStart;
    }

    public void OnLevelStart(LevelData level)
    {
        Waves = level.waves;

        LoadWaveData(0);

        StartCoroutine(WaveLoop());
    }

    private void LoadWaveData(int waveIndex)
    {
        WaveData wave = Waves[waveIndex];
        asteroidData = wave.AsteroidData;
        spawnRate = wave.spawnRate;
        spawnAmount = wave.spawnAmount;
        NumberOfEnemiesPerWave = wave.NumberOfEnemies;
        NumberOfEnemiesGrowth = wave.NumberOfEnemiesGrowth;
        SpawnRateGrowth = wave.SpawnRateGrowth;
        SpawnAmountGrowth = wave.SpawnAmountGrowth;
        MaxEnemies = wave.maxEnemies;
    }

    private IEnumerator WaveLoop()
    {
        while (true)
        {
            yield return StartCoroutine(SpawnWave());
            HandleWaveProgression();

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
                _totalSpawnedThisWave++;
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

        asteroid.Init(asteroidData);

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

    private void HandleWaveProgression()
    {

        ApplyWaveGrowth();

        if (_totalSpawnedThisWave >= MaxEnemies)
        {
            if (_currentWaveIndex < Waves.Length - 1)
            {
                _currentWaveIndex++;
                LoadWaveData(_currentWaveIndex);
            }
            else
            {
                Debug.Log("Final wave reached. Increasing difficulty indefinitely.");
            }

            _totalSpawnedThisWave = 0;
        }
    }

}
