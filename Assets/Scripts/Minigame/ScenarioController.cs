using System.Collections;
using UnityEngine;

public class ScenarioController : MonoBehaviour
{
    public Player player;
    public ParticleSystem explosionPrefab;

    public LevelData DefaultLevel;

    public int lives = 3;

    private void Awake()
    {
        Events.OnSetLives += OnSetLives;
        Events.OnRequestLives += OnRequestLives;
        Events.OnSetScore += OnSetScore;
        Events.OnRequestScore += OnRequestScore;
        Events.OnPlayerDeath += OnPlayerDeath;
        Events.OnAsteroidDestroyed += OnAsteroidDestroyed;
        Events.OnLevelStart += OnLevelStart;
    }

    private void OnDestroy()
    {
        Events.OnSetLives -= OnSetLives;
        Events.OnRequestLives -= OnRequestLives;
        Events.OnSetScore -= OnSetScore;
        Events.OnRequestScore -= OnRequestScore;
        Events.OnPlayerDeath -= OnPlayerDeath;
        Events.OnAsteroidDestroyed -= OnAsteroidDestroyed;
        Events.OnLevelStart -= OnLevelStart;
    }

    private IEnumerator Start()
    {
        Events.SetLives(lives);
        Events.SetScore(DataCarrier.points);

        yield return null; // wait 1 frame so all Awake() methods run

        Events.LevelStart(DefaultLevel);
    }

    private void OnSetLives(int amount)
    {
        lives = amount;
    }

    private int OnRequestLives()
    {
        return lives;
    }

    private void OnSetScore(int amount)
    {
        DataCarrier.points = amount;
    }

    private int OnRequestScore()
    {
        return DataCarrier.points;
    }

    // Handles player death event
    private void OnPlayerDeath(int amount)
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, player.transform.position, Quaternion.identity);
        }

        Events.SetLives(amount);
        if (lives <= 0)
        {
            Events.GameOver();
        }
    }

    // Handles asteroid destroyed event
    private void OnAsteroidDestroyed(Asteroid asteroid)
    {

        int addScore;

        if (asteroid.size < 0.75f)
            addScore = asteroid.ScoreValues[0];
        else if (asteroid.size < 1.10f)
            addScore = asteroid.ScoreValues[1];
        else
            addScore = asteroid.ScoreValues[2];

        DataCarrier.points += addScore;
        Events.SetScore(DataCarrier.points);
    
    // When score reaches 2000 or more trigger gameover
    // This is a temporary win condition
    /*
            if (score >= 2000)
        {
            Events.Victory();
        }
    */
}

    private void OnLevelStart(LevelData level)
    {
    }


}
