using UnityEngine;

[CreateAssetMenu(menuName="PlanetEvents/Planet1")]
public class Planet1_Events : PlanetEvents
{
    public override void TriggerEvent(string eventId, Planet planet)
    {
        switch (eventId)
        {
            case "NEW_SPACESHIP":
                Debug.Log("we got a new spaceship");
                break;

            default:
                Debug.Log("Unknown event: " + eventId);
                break;
        }
    }
}
