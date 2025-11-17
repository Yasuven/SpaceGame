using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{

    public GameObject pauseMenu;
    public GameObject optionsMenu;

    public InputActionAsset PlayerInput;
    public bool PauseMenuOpenInput = false;
    public bool PauseMenuCloseInput = false;
    private InputAction _pauseMenuOpenAction;
    private InputAction _pauseMenuCloseAction;

    private bool isPaused = false;

    private void Awake()
    {
        Events.OnPauseGame += OnPauseGame;
        Events.OnResumeGame += OnResumeGame;
        _pauseMenuOpenAction = PlayerInput.FindAction("PauseMenuOpen");
        _pauseMenuCloseAction = PlayerInput.FindAction("PauseMenuClose");


        PlayerInput.FindActionMap("Player").Enable();
        PlayerInput.FindActionMap("UI").Disable();
        Debug.Log("PlayerMap Enabled: " + PlayerInput.FindActionMap("Player").enabled);
        Debug.Log("UIMap Enabled: " + PlayerInput.FindActionMap("UI").enabled);

    }

    private void OnDestroy()
    {
        Events.OnPauseGame -= OnPauseGame;
        Events.OnResumeGame -= OnResumeGame;
    }

    private void Update()
    {
        PauseMenuOpenInput = _pauseMenuOpenAction.WasPressedThisFrame();
        PauseMenuCloseInput = _pauseMenuCloseAction.WasPressedThisFrame();

        if (PauseMenuOpenInput && !isPaused)
                Events.PauseGame();
        else if (PauseMenuCloseInput && isPaused)
                Events.ResumeGame();
        
    }

    private void OnResumeGame()
    {
        if (!isPaused) return;
        PlayerInput.FindActionMap("UI").Disable();
        PlayerInput.FindActionMap("Player").Enable();
        pauseMenu.gameObject.SetActive(false);
        optionsMenu.gameObject.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
        
    }

    private void OnPauseGame()
    {
        if (isPaused) return;
        PlayerInput.FindActionMap("Player").Disable();
        PlayerInput.FindActionMap("UI").Enable();
        pauseMenu.gameObject.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
        
    }
   

}
