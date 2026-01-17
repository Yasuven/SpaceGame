using UnityEngine;

[CreateAssetMenu(menuName = "PlanetConditions/Planet3")]
public class Planet3Condition : PlanetCondition
{
    public override int VerifyConditions(Planet planet)
    {
        if (DataCarrier.points >= 100000)
        {
            return 20;
        }
        return -1;
    }
}
