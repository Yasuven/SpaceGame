using UnityEngine;

public class UIManager : MonoBehaviour
{
    
    public GameObject pauseMenu;
    public GameObject optionsMenu;

    private bool isPaused = false;

    private void Awake()
    {
        Events.OnPauseGame += OnPauseGame;
        Events.OnResumeGame += OnResumeGame;
    }

    private void OnDestroy()
    {
        Events.OnPauseGame -= OnPauseGame;
        Events.OnResumeGame -= OnResumeGame;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Events.ResumeGame();
            }
            else
            {
                Events.PauseGame();
            }
        }
    }

    private void OnResumeGame()
    {
        if (!isPaused) return;
        pauseMenu.gameObject.SetActive(false);
        optionsMenu.gameObject.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }

    private void OnPauseGame()
    {
        if (isPaused) return;
        pauseMenu.gameObject.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
    }


}
