using System;
using UnityEngine;

public static class Events
{
    
    public static event Action<int> OnSetLives;

    public static void SetLives (int amount) => OnSetLives?.Invoke(amount);

    public static event Func<int> OnRequestLives;

    public static int RequestLives() => OnRequestLives?.Invoke() ?? 0;

    public static event Action<int> OnPlayerDeath;

    public static void PlayerDeath(int amount) => OnPlayerDeath?.Invoke(amount);

    public static event Action<Asteroid> OnAsteroidDestroyed;

    public static void AsteroidDestroyed(Asteroid asteroid) => OnAsteroidDestroyed?.Invoke(asteroid);

    public static event Action<int> OnSetScore;

    public static void SetScore(int amount) => OnSetScore?.Invoke(amount);

    public static event Func<int> OnRequestScore;

    public static int RequestScore() => OnRequestScore?.Invoke() ?? 0;

    public static event Action OnGameOver;
    public static void GameOver() => OnGameOver?.Invoke();

    public static event Action OnWinningCondition;
    public static void Victory() => OnWinningCondition?.Invoke();

    public static event Action OnEjecting;

    public static void Ejecting() => OnEjecting?.Invoke();

    public static event Action OnPauseGame;

    public static void PauseGame() => OnPauseGame?.Invoke();

    public static event Action OnResumeGame;

    public static void ResumeGame() => OnResumeGame?.Invoke();

    public static event Action<WaveData> OnLevelStart;
    public static void LevelStart(WaveData wave) => OnLevelStart?.Invoke(wave);

}

