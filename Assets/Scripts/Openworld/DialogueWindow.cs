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
        if (Input.GetKeyDown(KeyCode.UpArrow))
            MoveSelection(-1);

        else if (Input.GetKeyDown(KeyCode.DownArrow))
            MoveSelection(+1);

        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            ConfirmSelection();
    }
    private void MoveSelection(int direction)
    {
        var node = activeDialogue.nodes[currentNodeIndex];
        int optionCount = node.options.Length;

        selectedOption = (selectedOption + direction + optionCount) % optionCount;

        UpdateOptionColors();
    }
    private void ConfirmSelection()
    {
        var node = activeDialogue.nodes[currentNodeIndex];
        var option = node.options[selectedOption];

        ApplyPlanetProgress(option);
        int currentNode = currentPlanet.currentNode;
        TriggerDialogueEvents(option); 
        int newNode = currentPlanet.currentNode;

       if (currentNode != newNode)
        {
            if (newNode == -1)
                EndDialogue();
            else
                GoToNextNode(newNode);
            return;
        }


        if (option.nextNode == -1)
            EndDialogue();
        else
            GoToNextNode(option.nextNode);
    }
    private void TriggerDialogueEvents(DialogueOption option)
    {
        if (option.triggersEvent && currentPlanet != null && currentPlanet.events != null)
        {
            currentPlanet.events.TriggerEvent(option.eventId, currentPlanet);
        }
    }
    private void ApplyPlanetProgress(DialogueOption option)
    {
        if (option.updatesPlanetNode && currentPlanet != null)
        {
            currentPlanet.currentNode = option.newPlanetNode;
        }
    }

    private void GoToNextNode(int nextNode)
    {
        currentNodeIndex = nextNode;
        selectedOption = 0;
        ShowCurrentNode();
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
