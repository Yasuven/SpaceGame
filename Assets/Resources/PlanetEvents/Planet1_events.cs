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

            case "SHOTGUN":
             
                var newShip = Resources.Load<PlayerSpaceship>("SpaceShip/ShotgunFighter");
                DataCarrier.playerSpaceship = newShip;
                var playerOW = FindFirstObjectByType<Player>();
                playerOW.ApplyNewShip();
                DataCarrier.points -= 1000;
                
                break;

            default:
                Debug.Log("Unknown event: " + eventId);
                break;
        }
    }
}
