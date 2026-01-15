using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "PlanetConditions/Planet2")]
public class Planet2Condition : PlanetCondition
{
    public override int VerifyConditions(Planet planet)
    {   
        Debug.Log("Current spaceship is: " + DataCarrier.playerSpaceship.spaceshipName);
        if (DataCarrier.playerSpaceship.spaceshipName == "ShotgunFighter")
        {
            if (planet.haveContacted == false)
            {
                 return 1;
            }
            if (planet.specialValue >= 3) {
                return 8;
            }
            return 4;
        } else if (DataCarrier.playerSpaceship.spaceshipName == "RandomFighter"){
            return 11;
        }

        return 0;
    }
}
