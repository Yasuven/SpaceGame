using UnityEngine;

[CreateAssetMenu(menuName="PlanetEvents/Planet2")]
public class Planet2_Events : PlanetEvents
{
    public override void TriggerEvent(string eventId, Planet planet)
    {
        switch (eventId)
        {
            case "CHECK_6000":
                if (DataCarrier.points >= 6000)
                {
                    planet.currentNode = 3;
                    break;
                }
                planet.currentNode = 6;
                break;
            
            case "HAVECONTACTED":
            {
                planet.haveContacted = true;
                break;
            }

            case "SELL_5000":
            {
                DataCarrier.points += 5000;
                planet.specialValue++;
                if (planet.specialValue > 3) planet.currentNode = 8;
                var newShip = Resources.Load<PlayerSpaceship>("SpaceShip/BasicFighter");
                DataCarrier.playerSpaceship = newShip;
                var playerOW = FindFirstObjectByType<Player>();
                playerOW.ApplyNewShip();
                break;
            }

            case "CHECK_IF_SHOTGUN":
            {

                break;
            }

            case "EXPERIMENTAL_SHIP":
             {
                var newShip = Resources.Load<PlayerSpaceship>("SpaceShip/RandomFighter");
                DataCarrier.playerSpaceship = newShip;
                var playerOW = FindFirstObjectByType<Player>();
                playerOW.ApplyNewShip();
                break;
            }
            default:
                Debug.Log("Unknown event: " + eventId);
                break;
        }
    }
}
