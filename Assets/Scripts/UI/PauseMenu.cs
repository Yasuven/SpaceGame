using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public Button ResumeButton;
    public Button OptionButton;
    public Button QuitButton;
    public OptionsMenu OptionsMenu;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Resume()
    {
        Events.ResumeGame();
    }

    public void Options()
    {
        gameObject.SetActive(false);
        OptionsMenu.gameObject.SetActive(true);
        OptionsMenu.JumpToPage(0);
    }

    public void Exit()
    {
        //TODO
        Debug.Log("Quit Game - TODO");
    }
}
