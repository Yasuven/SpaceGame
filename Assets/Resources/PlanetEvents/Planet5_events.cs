using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName="PlanetEvents/Planet5")]
public class Planet5_Events : PlanetEvents
{
    public override void TriggerEvent(string eventId, Planet planet)
    {
        switch (eventId)
        {
            case "PURCHASE_MACHINEGUN":
                if (DataCarrier.playerSpaceship.name == "RandomFighter")
                {
                    if (DataCarrier.points >= 10000)
                    {
                        var newShip = Resources.Load<PlayerSpaceship>("SpaceShip/MachineGunFighter");
                        DataCarrier.playerSpaceship = newShip;
                        var playerOW = FindFirstObjectByType<Player>();
                        playerOW.ApplyNewShip();
                       
                    } else
                    {
                        planet.currentNode = 2;
                    }
                } else { planet.currentNode = 3;}
                break;

            default:
                Debug.Log("Unknown event: " + eventId);
                break;
        }
    }

}
