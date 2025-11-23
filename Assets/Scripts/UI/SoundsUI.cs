using UnityEngine;
using UnityEngine.EventSystems;

public class UISound : MonoBehaviour, IPointerClickHandler
{
    public AudioClip clickClip;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickClip != null)
            AudioManager.Instance.PlaySound(clickClip);
    }
}
