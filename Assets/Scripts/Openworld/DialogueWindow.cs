using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DialogueWindow : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI npcText;
    public Transform optionsParent;
    public TextMeshProUGUI optionTemplate;   // <- the "Option" under OptionsParent

    public bool IsFinished { get; private set; }

    [Header("Settings")]
    public Color normalColor = Color.white;
    public Color selectedColor = Color.yellow;

    private DialogueData activeDialogue;
    private int currentNodeIndex = 0;
    private int selectedOption = 0;
    private bool isActive = false;

    private Planet currentPlanet;
    private List<TextMeshProUGUI> optionTexts = new List<TextMeshProUGUI>();

    public DialogueData dialogueData;

    public void StartDialogue(DialogueData dialogue, int startNode = 0, Planet planet = null)
    {
        activeDialogue = dialogue ?? dialogueData;
        currentPlanet = planet;

        if (activeDialogue == null)
            return;

        currentNodeIndex = startNode;
        selectedOption = 0;
        isActive = true;
        IsFinished = false;
        gameObject.SetActive(true);

        ShowCurrentNode();
    }

    private void Update()
    {
        if (!isActive) return;
        HandleInput();
    }

    private void ShowCurrentNode()
    {
        ClearOptions();

        var node = activeDialogue.nodes[currentNodeIndex];
        npcText.text = node.npcText;

        foreach (var option in node.options)
        {
            TextMeshProUGUI newOpt = Instantiate(optionTemplate, optionsParent);
            newOpt.text = option.text;
            newOpt.gameObject.SetActive(true);

            optionTexts.Add(newOpt);
        }

        UpdateOptionColors();
    }

    private void HandleInput()
    {
        var node = activeDialogue.nodes[currentNodeIndex];
        int optionCount = node.options.Length;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedOption = (selectedOption - 1 + optionCount) % optionCount;
            UpdateOptionColors();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedOption = (selectedOption + 1) % optionCount;
            UpdateOptionColors();
        }
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            var option = node.options[selectedOption];

            if (option.updatesPlanetNode && currentPlanet != null)
                currentPlanet.currentNode = option.newPlanetNode;

            if (option.nextNode == -1)
                EndDialogue();
            else
            {
                currentNodeIndex = option.nextNode;
                selectedOption = 0;
                ShowCurrentNode();
            }
        }
    }

    private void UpdateOptionColors()
    {
        for (int i = 0; i < optionTexts.Count; i++)
            optionTexts[i].color = (i == selectedOption) ? selectedColor : normalColor;
    }

    private void ClearOptions()
    {
        foreach (Transform t in optionsParent)
        {
            if (t != optionTemplate.transform) 
                Destroy(t.gameObject);
        }

        optionTexts.Clear();
    }

    private void EndDialogue()
    {
        isActive = false;
        IsFinished = true;
        gameObject.SetActive(false);
        ClearOptions();
    }
}
