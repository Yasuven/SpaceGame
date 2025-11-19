using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RememberCurrentlySelectedGameObject : MonoBehaviour
{
    private EventSystem eventSystem;
    private GameObject lastSelectedElement;

    private bool usingKeyboard = true;

    private void Reset()
    {
        eventSystem = FindFirstObjectByType<EventSystem>();
        if (!eventSystem)
        {
            Debug.LogWarning("Did not find an Event System in this scene.", this);
            return;
        }

        lastSelectedElement = eventSystem.firstSelectedGameObject;
    }

    private void Awake()
    {
        if (!eventSystem) eventSystem = EventSystem.current;
        if (!lastSelectedElement && eventSystem)
            lastSelectedElement = eventSystem.firstSelectedGameObject;
        Events.OnPauseGame += OnPauseGame;
    }

    private void OnDestroy()
    {
        Events.OnPauseGame -= OnPauseGame;
    }

    private void Update()
    {
        if (!eventSystem) return;

        DetectKeyboardInput();
        TrackKeyboardSelection();
    }

    // Detect any keyboard/gamepad key press
    private void DetectKeyboardInput()
    {
        if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
        {
            usingKeyboard = true;

            // If nothing selected, restore last keyboard focus
            if (lastSelectedElement && eventSystem.currentSelectedGameObject == null)
                eventSystem.SetSelectedGameObject(lastSelectedElement);
        }
    }

    // Keep track of selection changes made by keyboard navigation
    private void TrackKeyboardSelection()
    {
        if (usingKeyboard && eventSystem.currentSelectedGameObject &&
            eventSystem.currentSelectedGameObject != lastSelectedElement)
        {
            lastSelectedElement = eventSystem.currentSelectedGameObject;
        }
    }

    public void OnMouseHover(Object hoveredObject)
    {
        usingKeyboard = false;

        // Only clear selection if hovering over a different element
        if (eventSystem.currentSelectedGameObject != hoveredObject)
            eventSystem.SetSelectedGameObject(null);
    }

    private void OnPauseGame()
    {
        // Restore last selected element when pausing
        if (lastSelectedElement)
            eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
    }
}
