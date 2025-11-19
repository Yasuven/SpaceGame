using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button StartButton;
    public Button OptionButton;
    public Button QuitButton;
    public OptionsMenu OptionsMenu;

    private void Awake()
    {
    }

    public void StartGame()
    {
        //TODO
    }

    public void Options()
    {
        OptionsMenu.gameObject.SetActive(true);
        OptionsMenu.JumpToPage(0);
    }

    public void Exit()
    {
        //TODO
        Debug.Log("Quit Game - TODO");
        Application.Quit();
    }
}
