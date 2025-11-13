using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetsUiElementToSelectOnInteraction : MonoBehaviour
{
    public EventSystem eventSystem;
    public Selectable elementToSelect;

    private bool showVisualization;
    private Color navigationColour = Color.cyan;


    private void OnDrawGizmos()
    {
        if (!showVisualization) return;

        if (elementToSelect == null) return;

        Gizmos.color = navigationColour;
        Gizmos.DrawLine(gameObject.transform.position, elementToSelect.gameObject.transform.position);
    }

    private void Reset()
    {
        eventSystem = Object.FindFirstObjectByType<EventSystem>();
    }

    public void JumpToElement()
    {
        if (eventSystem == null) return;

        if (elementToSelect == null) return;

        eventSystem.SetSelectedGameObject(elementToSelect.gameObject);
    }
}

