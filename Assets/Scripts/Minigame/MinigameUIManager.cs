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
        Debug.Log("[MinigameUIManager] Awake()");

        // Ensure only one instance exists
        var managers = FindObjectsOfType<MinigameUIManager>();
        if (managers.Length > 1)
        {
            Debug.LogWarning("DESTROYING DUPLICATE MinigameUIManager!");
            Destroy(gameObject);
            return;
        }

        Events.OnSetLives += OnSetLives;
        Events.OnSetScore += OnSetScore;
        Events.OnEjecting += OnEjecting;
    }

    private void OnDestroy()
    {
        Debug.Log("[MinigameUIManager] Awake()");

        Events.OnSetLives -= OnSetLives;
        Events.OnSetScore -= OnSetScore;
        Events.OnEjecting -= OnEjecting;
    }

    private void Start()
    {
        Debug.Log("[MinigameUIManager] Start()");

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
        Debug.Log("[MinigameUIManager] OnSetScore(" + amount + ")");

        scoreText.text = amount.ToString();

        // Don't reset global score on 0
        if (amount > 0)
        {
            DataCarrier.points = amount;
            Debug.Log("[GLOBAL] Updated to: " + DataCarrier.points);
        }
        else
        {
            Debug.Log("[GLOBAL] Ignored amount = 0");
        }
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
        SceneManager.LoadScene("OpenWorld");
    }
}
