using UnityEngine;

[CreateAssetMenu(menuName="PlanetEvents/Planet3")]
public class Planet3_Events : PlanetEvents
{
    public override void TriggerEvent(string eventId, Planet planet)
    {
        switch (eventId)
        {
             case "CHECKPOINTS":
                if (DataCarrier.points >= 10000)
                {
                    planet.currentNode = 13; // fix to win condition
                    break;
                }
                planet.currentNode = 11;
                break;

            default:
                Debug.Log("Unknown event: " + eventId);
                break;
        }
    }
}
