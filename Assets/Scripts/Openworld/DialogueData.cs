using UnityEngine;

[System.Serializable]
public class DialogueNode {
    [TextArea(3,10)] public string npcText;
    public DialogueOption[] options;
}

[System.Serializable]
public class DialogueOption
{
    public string text;
    public int nextNode; // -1 = end of dialogue

    [Header("Optional Planet Progression")]
    public bool updatesPlanetNode;
    public int newPlanetNode;
}



[CreateAssetMenu(menuName = "Dialogue/TestDialogue")]
public class DialogueData : ScriptableObject {
    public DialogueNode[] nodes;
}
