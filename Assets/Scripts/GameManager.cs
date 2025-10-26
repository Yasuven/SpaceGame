// GameManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshProUGUI timerText;
    public GameObject timerUI;
    public float countDown = 7f;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void StartEjection()
    {
        StartCoroutine(EjectionCountdown());
    }

    private IEnumerator EjectionCountdown()
    {
        float countdown = countDown;
        timerUI.SetActive(true);

        while (countdown > 0)
        {
            timerText.text = Mathf.CeilToInt(countdown).ToString();
            yield return new WaitForSeconds(1f);
            countdown -= 1f;
        }

        timerUI.SetActive(false);
        SceneManager.LoadScene("OpenWorld");
    }
}
