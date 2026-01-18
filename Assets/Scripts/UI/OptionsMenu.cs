using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Volume Sliders")]
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;

    [Header("Navigation")]
    public int PageIndex = 0;
    public ToggleGroup toggleGroup;
    public List<Toggle> tabs = new List<Toggle>();
    public List<CanvasGroup> pages = new List<CanvasGroup>();
    public RectTransform PauseMenu;

    private void Awake()
    {
        foreach (var toggle in tabs)
        {
            toggle.onValueChanged.AddListener(CheckForTab);
            toggle.group = toggleGroup;
        }
    }

    private void OnEnable()
    {
        if (AudioManager.Instance != null)
        {
            InitializeSlider(masterSlider, "MasterVolumeSave", 0.5f, AudioManager.Instance.SetMasterVolume);
            InitializeSlider(sfxSlider, "SFXVolumeSave", 0.5f, AudioManager.Instance.SetSFXVolume);
            InitializeSlider(musicSlider, "MusicVolumeSave", 0.5f, AudioManager.Instance.SetMusicVolume);
        }

        OpenPage(PageIndex);
    }

    private void InitializeSlider(Slider slider, string saveKey, float defaultVal, UnityEngine.Events.UnityAction<float> onValueChange)
    {
        if (slider == null) return;

        slider.onValueChanged.RemoveAllListeners();
        float savedVal = PlayerPrefs.GetFloat(saveKey, defaultVal);
        slider.value = savedVal;
        
        // Force update the Mixer immediately
        onValueChange.Invoke(savedVal);

        slider.onValueChanged.AddListener(onValueChange);
    }


    private void CheckForTab(bool isOn)
    {
        if (!isOn) return; // Ignore the toggle being turned off

        for (int i = 0; i < tabs.Count; i++)
        {
            if (tabs[i].isOn)
            {
                PageIndex = i;
                break;
            }
        }
        OpenPage(PageIndex);
    }

    private void OpenPage(int index)
    {
        if (!gameObject.activeInHierarchy) return; 

        if (pages.Count == 0) return;
        
        PageIndex = Mathf.Clamp(index, 0, pages.Count - 1);

        for (int i = 0; i < pages.Count; i++)
        {
            bool isActivePage = (i == PageIndex);
            pages[i].alpha = isActivePage ? 1 : 0;
            pages[i].interactable = isActivePage;
            pages[i].blocksRaycasts = isActivePage;
        }
    }

    public void JumpToPage(int page)
    {
        OpenPage(page);
        if (tabs.Count > PageIndex) tabs[PageIndex].isOn = true;
    }

    public void Back()
    {
        gameObject.SetActive(false);
        if (PauseMenu != null) PauseMenu.gameObject.SetActive(true);
    }
}