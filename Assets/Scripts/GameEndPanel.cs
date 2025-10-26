using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEndPanel : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI ScoreText;
    public Button RestartButton;

    private void Awake()
    {
        gameObject.SetActive(false);
        Events.OnGameOver += OnGameOver;
        Events.OnWinningCondition += OnWinningCondition;
    }

    private void OnDestroy()
    {
        Events.OnGameOver -= OnGameOver;
        Events.OnWinningCondition -= OnWinningCondition;
    }   

    private void OnGameOver()
    {
        int finalScore = Events.RequestScore();
        Title.text = "Game Over";
        ScoreText.text = $"Score: {finalScore}";
        gameObject.SetActive(true);
    }

    private void OnWinningCondition()
    {
        int finalScore = Events.RequestScore();
        Title.text = "You Win";
        ScoreText.text = $"Score: {finalScore}";
        gameObject.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
