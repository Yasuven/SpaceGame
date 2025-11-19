using UnityEngine;

[System.Serializable]
public class WaveData
{
    public int NumberOfEnemies = 3;
    public float spawnRate = 2f;
    public int spawnAmount = 1;
    public int NumberOfEnemiesGrowth = 2;
    public float SpawnRateGrowth = 0.2f;
    public int SpawnAmountGrowth = 1;
    public int maxEnemies = 100;
    public AsteroidData AsteroidData;
}
