using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MinigameUIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public Image[] lifeIcons;
    public TextMeshProUGUI timerText;
    public float countDown = 7f;

    private void Awake()
    {
        Events.OnSetLives += OnSetLives;
        Events.OnSetScore += OnSetScore;
        Events.OnEjecting += OnEjecting;
    }

    private void OnDestroy()
    {
        Events.OnSetLives -= OnSetLives;
        Events.OnSetScore -= OnSetScore;
        Events.OnEjecting -= OnEjecting;
    }

    private void Start()
    {
        timerText.gameObject.SetActive(false);
    }

    private void OnSetLives(int amount)
    {
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            lifeIcons[i].enabled = i < amount;
        }
    }

    private void OnSetScore(int amount)
    {
        scoreText.text = amount.ToString();
    }

    private void OnEjecting()
    {
        StartCoroutine(EjectionCountdown());
    }

    private IEnumerator EjectionCountdown()
    {
        float countdown = countDown;
        timerText.gameObject.SetActive(true);

        while (countdown > 0)
        {
            timerText.text = Mathf.CeilToInt(countdown).ToString();
            yield return new WaitForSeconds(1f);
            countdown -= 1f;
        }

        timerText.gameObject.SetActive(false);
        int parsedScore = int.Parse(scoreText.text);
        DataCarrier.points += parsedScore; // assign achiveved points to data carrier
        SceneManager.LoadScene("OpenWorld");
    }
}
