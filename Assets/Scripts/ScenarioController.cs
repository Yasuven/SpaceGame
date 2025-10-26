using UnityEngine;

public class ScenarioController : MonoBehaviour
{
    public Player player;
    public ParticleSystem explosion;

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
    }

    private void OnDestroy()
    {
        Events.OnSetLives -= OnSetLives;
        Events.OnRequestLives -= OnRequestLives;
        Events.OnSetScore -= OnSetScore;
        Events.OnRequestScore -= OnRequestScore;
        Events.OnPlayerDeath -= OnPlayerDeath;
        Events.OnAsteroidDestroyed -= OnAsteroidDestroyed;
    }

    private void Start()
    {
        Events.SetLives(lives);
        Events.SetScore(score);
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
    // When score reaches 5000 or more trigger gameover
    // This is a temporary win condition
            if (score >= 5000)
        {
            Events.Victory();
        }
    }


}
