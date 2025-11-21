using UnityEngine;

[CreateAssetMenu(menuName="PlanetEvents/Planet2")]
public class Planet2_Events : PlanetEvents
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
              
                if (playerOW != null)
                    playerOW.ApplyNewShip();
                else
                    Debug.LogError("PlayerOpenWorld not found!");
                
                break;

            default:
                Debug.Log("Unknown event: " + eventId);
                break;
        }
    }
}
