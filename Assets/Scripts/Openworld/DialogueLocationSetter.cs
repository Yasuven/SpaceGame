using UnityEngine;
using TMPro;

public class DialogueLocationSetter : MonoBehaviour
{
    public Transform target;
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
        rectTransform.position = target.position;
    }
}
