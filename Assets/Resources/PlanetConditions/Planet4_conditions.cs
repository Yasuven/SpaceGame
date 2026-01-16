using UnityEngine;

[CreateAssetMenu(menuName = "PlanetConditions/Planet4")]
public class Planet4Condition : PlanetCondition
{
    public override int VerifyConditions(Planet planet)
    {
        Debug.Log("Current spaceship is: " + DataCarrier.playerSpaceship.spaceshipName);
        if (DataCarrier.playerSpaceship.spaceshipName == "GarbageFighter")
        {
           return 20;
        } 

        if (DataCarrier.points < 2000)
        {
            return 19;
        }

        if (planet.haveContacted)
        {
            return 10;
        }

        return 0;
    }
}
