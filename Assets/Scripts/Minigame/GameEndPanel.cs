using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
        //int finalScore = Events.RequestScore();
        int finalScore = DataCarrier.points;
        Title.text = "Game Over";
        ScoreText.text = $"Score: {finalScore}";
        gameObject.SetActive(true);
        StartCoroutine(SelectNextFrame(RestartButton.gameObject));
    }



    private IEnumerator SelectNextFrame(GameObject target)
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(target);
    }


    private void OnWinningCondition()
    {
        Time.timeScale = 0f; // pause the game when won. added for openWorld
        //int finalScore = Events.RequestScore();
        int finalScore = DataCarrier.points;
        Title.text = "You Won";
        ScoreText.text = $"Score: {finalScore}";
        gameObject.SetActive(true);
        StartCoroutine(SelectNextFrame(RestartButton.gameObject));
    }

    public void Restart()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
