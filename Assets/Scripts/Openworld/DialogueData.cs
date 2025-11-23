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
    public int nextNode; 

    [Header("Optional Planet Progression")]
    public bool updatesPlanetNode;
    public int newPlanetNode;

    [Header("Events")]
    public bool triggersEvent;
    public string eventId;   

}



[CreateAssetMenu(menuName = "Dialogue/EmptyDialogue")]
public class DialogueData : ScriptableObject {
    public DialogueNode[] nodes;
}
