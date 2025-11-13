using NUnit.Framework;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
   
    public int PageIndex = 0;

    public ToggleGroup toggleGroup;
    public List<Toggle> tabs = new List<Toggle>();
    public List<CanvasGroup> pages = new List<CanvasGroup>();
    public RectTransform PauseMenu;

    private void Initialize()
    {
        toggleGroup = GetComponentInChildren<ToggleGroup>();

        tabs.Clear();
        pages.Clear();

        tabs.AddRange(GetComponentsInChildren<Toggle>());
        pages.AddRange(GetComponentsInChildren<CanvasGroup>());
    }

    private void Reset()
    {
        Initialize();
    }

    private void OnValidate()
    {
        Initialize();
        OpenPage(PageIndex);
        tabs[PageIndex].isOn = true;
    }

    private void Awake()
    {
        gameObject.SetActive(false);
        foreach (var toggle in tabs)
        {
            toggle.onValueChanged.AddListener(CheckForTab);
            toggle.group = toggleGroup;
        }
    }

    private void OnDestroy()
    {
        foreach (var toggle in tabs)
        {
            toggle.onValueChanged.RemoveListener(CheckForTab);
        }
    }


    private void CheckForTab(bool value)
    {
        for (int i = 0; i < tabs.Count; i++)
        {
           if (!tabs[i].isOn) continue;
            PageIndex = i;
        }
        OpenPage(PageIndex);
    }

    private void OpenPage(int index)
    {
        EnsureIndexIsInRange(index);

        for (int i = 0; i < pages.Count; i++)
        {
           bool isActivePage = (i == index);

            pages[i].alpha = isActivePage ? 1 : 0;
            pages[i].interactable = isActivePage;
            pages[i].blocksRaycasts = isActivePage;
        }
    }

    private void EnsureIndexIsInRange(int index)
    {
        if (tabs.Count == 0 || pages.Count == 0) return;

        PageIndex = Mathf.Clamp(index, 0, pages.Count - 1);
    }

    public void JumpToPage(int page)
    {
        EnsureIndexIsInRange(page);

        tabs[PageIndex].isOn = true;
    }

    public void Back()
    {
        gameObject.SetActive(false);
        PauseMenu.gameObject.SetActive(true);
    }
}
