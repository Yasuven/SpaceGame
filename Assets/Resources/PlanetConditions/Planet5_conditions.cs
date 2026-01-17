using UnityEngine;

[CreateAssetMenu(menuName = "PlanetConditions/Planet5")]
public class Planet5Condition : PlanetCondition
{
    public override int VerifyConditions(Planet planet)
    {
        if (DataCarrier.playerSpaceship.name == "ShotgunFighter" || DataCarrier.playerSpaceship.name == "RandomFighter")
        {
            return 1;
        } 

        if (DataCarrier.playerSpaceship.name == "MachineGunFighter")
        {
            return 4;
        }
        return 0;
    }
}
