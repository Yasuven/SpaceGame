using UnityEngine;
using UnityEngine.EventSystems;

public class UISound : MonoBehaviour, IPointerClickHandler, ISubmitHandler
{
    public AudioClip clickClip;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickClip != null)
            AudioManager.Instance.PlaySound(clickClip);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (clickClip != null)
            AudioManager.Instance.PlaySound(clickClip);
    }
}
