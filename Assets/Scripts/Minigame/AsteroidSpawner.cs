using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public Asteroid asteroidPrefab;
    private AsteroidData asteroidData;

    public float trajectoryVariance = 30f;
    public float NumberOfEnemiesPerWave = 5;
    public float spawnRate = 2f;
    public float spawnDistance = 12f;
    public int spawnAmount = 1;

    public int NumberOfEnemiesGrowth = 2;
    public float SpawnRateGrowth = 0.2f;
    public int SpawnAmountGrowth = 1;
    public int MaxEnemies = 100;

    private List<Asteroid> _trackedAsteroids = new List<Asteroid>();

    private int _currentWaveIndex = 0;
    private int _totalSpawnedThisWave = 0;

    private WaveData[] Waves;

    private void Awake()
    {   
        Events.OnLevelStart += OnLevelStart;
        Events.OnPlayerDeath += OnPlayerDeath;
    }

    private void Events_OnAsteroidDestroyed(Asteroid obj)
    {
        throw new System.NotImplementedException();
    }

    private void OnDestroy()
    {
        Events.OnLevelStart -= OnLevelStart;
        Events.OnPlayerDeath -= OnPlayerDeath;
    }

    public void OnLevelStart(LevelData level)
    {
        Waves = level.waves;

        LoadWaveData(0);

        StartCoroutine(WaveLoop());
    }

    private void OnPlayerDeath(int livesLeft)
    {
        ClearAllAsteroids();
    }

    public void ClearAllAsteroids()
    {
        // We iterate backwards through the list to safely destroy objects
        for (int i = _trackedAsteroids.Count - 1; i >= 0; i--)
        {
            if (_trackedAsteroids[i] != null)
            {
                Destroy(_trackedAsteroids[i].gameObject);
            }
        }
        _trackedAsteroids.Clear();
    }

    public void RegisterAsteroid(Asteroid asteroid)
    {
        if (!_trackedAsteroids.Contains(asteroid))
        {
            _trackedAsteroids.Add(asteroid);
        }
    }

    public void UnregisterAsteroid(Asteroid asteroid)
    {
        _trackedAsteroids.Remove(asteroid);
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

        asteroid.RegisterSpawner(this);
        _trackedAsteroids.Add(asteroid);

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
