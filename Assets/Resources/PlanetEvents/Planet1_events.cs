using UnityEngine;

[CreateAssetMenu(menuName="PlanetEvents/Planet1")]
public class Planet1_Events : PlanetEvents
{
    public override void TriggerEvent(string eventId, Planet planet)
    {
        switch (eventId)
        {
            case "CHECKPOINTS":
                if (DataCarrier.points >= 3000)
                {
                    planet.currentNode = 2;
                    break;
                }
                planet.currentNode = 4;
                break;

            case "SHOTGUN":
             
                var newShip = Resources.Load<PlayerSpaceship>("SpaceShip/ShotgunFighter");
                DataCarrier.playerSpaceship = newShip;
                var playerOW = FindFirstObjectByType<Player>();
                playerOW.ApplyNewShip();
                DataCarrier.points -= 3000;
                
                break;

            default:
                Debug.Log("Unknown event: " + eventId);
                break;
        }
    }
}
