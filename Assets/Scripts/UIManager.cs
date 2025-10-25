using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public Image[] lifeIcons;

    private void Awake()
    {
        Events.OnSetLives += OnSetLives;
        Events.OnSetScore += OnSetScore;
    }

    private void OnDestroy()
    {
        Events.OnSetLives -= OnSetLives;
        Events.OnSetScore -= OnSetScore;
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
}
