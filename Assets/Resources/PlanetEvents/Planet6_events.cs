using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName="PlanetEvents/Planet6")]
public class Planet6_Events : PlanetEvents
{
    public override void TriggerEvent(string eventId, Planet planet)
    {
        switch (eventId)
        {
            case "GIFT10":
                DataCarrier.points += 10;
                break;
            default:
                Debug.Log("Unknown event: " + eventId);
                break;
        }
    }

}
