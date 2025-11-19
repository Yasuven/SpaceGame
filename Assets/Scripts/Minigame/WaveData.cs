using UnityEngine;

[CreateAssetMenu(menuName = "SpaceGame/Wave")]
public class WaveData : ScriptableObject
{
    public int NumberOfEnemies = 3;
    public float spawnRate = 2f;
    public int spawnAmount = 1;
    public int NumberOfEnemiesGrowth = 2;
    public float SpawnRateGrowth = 0.2f;
    public int SpawnAmountGrowth = 1;
}
