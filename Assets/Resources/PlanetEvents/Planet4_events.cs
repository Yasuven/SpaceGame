using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName="PlanetEvents/Planet4")]
public class Planet4_Events : PlanetEvents
{
    public override void TriggerEvent(string eventId, Planet planet)
    {
        switch (eventId)
        {

            case "VISITED":
                planet.haveContacted = true;
                break;

            case "RANDOMIZER":
            {
                int[] nodes = { 12, 13, 14, 15 };
                planet.currentNode = nodes[Random.Range(0, nodes.Length)];
                break;
            }

            case "RANDOMIZER_DIVISIBLE5":
                ResolveRandomizer(planet, p => p % 5 == 0);
                break;

            case "RANDOMIZER_DIVISIBLE3":
                ResolveRandomizer(planet, p => p % 3 == 0);
                break;

            case "RANDOMIZER_EVEN":
                ResolveRandomizer(planet, p => p % 2 == 0);
                break;

            case "RANDOMIZER_ODD":
                ResolveRandomizer(planet, p => p % 2 != 0);
                break;

            default:
                Debug.Log("Unknown event: " + eventId);
                break;
        }
    }

    private void ResolveRandomizer(
    Planet planet,
    System.Func<int, bool> winCondition
    )
    {
        int points = DataCarrier.points;

        if (winCondition(points))
        {
            planet.currentNode = 16; // won
            DataCarrier.points = Mathf.RoundToInt(DataCarrier.points * 1.5f);
        }
        else if (points / 2 > 2000)
        {
            DataCarrier.points /= 2;
            planet.currentNode = 18; // can play more
        }
        else
        {
            DowngradeShip();
            planet.currentNode = 17; // lost
        }
    }

    private void DowngradeShip()
    {
        var newShip = Resources.Load<PlayerSpaceship>("SpaceShip/GarbageFighter");
        DataCarrier.playerSpaceship = newShip;

        var player = FindFirstObjectByType<Player>();
        if (player != null)
            player.ApplyNewShip();
    }

}
