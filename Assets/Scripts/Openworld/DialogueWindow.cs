using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DialogueWindow : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI npcText;
    public Transform optionsParent;
    public TextMeshProUGUI optionPrefab;
    public bool IsFinished { get; private set; }

    [Header("Settings")]

    //TODO: add spaceship as object, and inherit current node from it, also inherit dialogue data
    public Color normalColor = Color.white;
    public Color selectedColor = Color.yellow;
    private DialogueData activeDialogue;
    private int currentNodeIndex = 0;
    private int selectedOption = 0;
    private List<TextMeshProUGUI> optionTexts = new List<TextMeshProUGUI>();
    private bool isActive = false;
    private Planet currentPlanet;
    public DialogueData dialogueData;
    public void StartDialogue(DialogueData dialogue, int startNode = 0, Planet planet = null)
    {
        activeDialogue = dialogue ?? dialogueData;
        currentPlanet = planet;
        if (activeDialogue == null)
        {
            //Debug.LogError("[DialogueWindow] No dialogue data assigned!");
            return;
        }

        currentNodeIndex = startNode;
        selectedOption = 0;
        isActive = true;
        IsFinished = false;
        gameObject.SetActive(true);

        //Debug.Log($"[DialogueWindow] Starting dialogue with {activeDialogue.nodes.Length} nodes");
        ShowCurrentNode();
    }

    private void Update()
    {
        if (!isActive) return;
        HandleInput();
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

            // --- Update planet currentNode if requested ---
            if (option.updatesPlanetNode && currentPlanet != null)
            {
                currentPlanet.currentNode = option.newPlanetNode;
                //Debug.Log($"[Dialogue] Planet {currentPlanet.planetId} node set to {option.newPlanetNode}");
            }

            // --- Continue or end dialogue ---
            if (option.nextNode == -1)
            {
                EndDialogue();
            }
            else
            {
                currentNodeIndex = option.nextNode;
                selectedOption = 0;
                ShowCurrentNode();
            }
        }

    }

    private void ShowCurrentNode()
    {
        ClearOptions();

        if (activeDialogue == null)
        {
            //Debug.LogError("[DialogueWindow] No active dialogue!");
            return;
        }

        if (currentNodeIndex >= activeDialogue.nodes.Length)
        {
            //Debug.LogError($"[DialogueWindow] Node index {currentNodeIndex} out of range!");
            return;
        }

        var node = activeDialogue.nodes[currentNodeIndex];
        npcText.text = node.npcText;
        //Debug.Log($"[DialogueWindow] Showing node {currentNodeIndex}: \"{node.npcText}\" with {node.options.Length} options");

        if (optionPrefab == null)
        {
            //Debug.LogError("[DialogueWindow] Option prefab is null!");
            return;
        }

        if (optionsParent == null)
        {
            //Debug.LogError("[DialogueWindow] Options parent is null!");
            return;
        }

        foreach (var option in node.options)
        {
            var newText = Instantiate(optionPrefab, optionsParent);
            newText.text = option.text;
            newText.gameObject.SetActive(true);
            optionTexts.Add(newText);

            //Debug.Log($"[DialogueWindow] Created option '{option.text}'");
        }

        UpdateOptionColors();
    }

    private void UpdateOptionColors()
    {
        for (int i = 0; i < optionTexts.Count; i++)
        {
            optionTexts[i].color = (i == selectedOption) ? selectedColor : normalColor;
        }
    }

    private void ClearOptions()
    {
        foreach (Transform child in optionsParent)
            Destroy(child.gameObject);
        optionTexts.Clear();
    }

    private void EndDialogue()
    {
        //Debug.Log("[DialogueWindow] Dialogue ended");
        isActive = false;
        IsFinished = true;
        gameObject.SetActive(false);
        ClearOptions();
    }
}
