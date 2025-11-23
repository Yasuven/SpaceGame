using UnityEngine;

[CreateAssetMenu(menuName = "PlanetConditions/Planet2")]
public class Planet2Condition : PlanetCondition
{
    public override int VerifyConditions(Planet planet)
    {
        if (DataCarrier.playerSpaceship.spaceshipName == "ShotgunFighter")
        {
            return 1;
        }

        return -1;
    }
}
