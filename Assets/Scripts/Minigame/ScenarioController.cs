using System.Collections;
using UnityEngine;

public class ScenarioController : MonoBehaviour
{
    public Player player;
    public ParticleSystem explosion;

    public WaveData DefaultWave;

    public int lives = 3;
    public int score = 0;

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
        Events.SetScore(score);

        yield return null; // wait 1 frame so all Awake() methods run

        Debug.Log("ScenarioController Start() — FIRING EVENT AFTER 1 FRAME");
        Events.LevelStart(DefaultWave);
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
        score = amount;
    }

    private int OnRequestScore()
    {
        return score;
    }

    // Handles player death event
    private void OnPlayerDeath(int amount)
    {
        explosion.transform.position = player.transform.position;
        explosion.Play();

        Events.SetLives(amount);
        if (lives <= 0)
        {
            Events.GameOver();
        }
    }

    // Handles asteroid destroyed event
    private void OnAsteroidDestroyed(Asteroid asteroid) 
    {
        explosion.transform.position = asteroid.transform.position;
        explosion.Play();

        if (asteroid.size < 0.75f)
        {
            Events.SetScore(score += 100);
        }
        else if (asteroid.size < 1.10f)
        {
            Events.SetScore(score += 50);
        }
        else
        {
            Events.SetScore(score += 20);
        }
    // When score reaches 2000 or more trigger gameover
    // This is a temporary win condition
    /*
            if (score >= 2000)
        {
            Events.Victory();
        }
    */
    }

    private void OnLevelStart(WaveData wave)
    {
    }


}
