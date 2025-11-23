using UnityEngine;

[CreateAssetMenu(menuName = "PlanetConditions/Planet1")]
public class Planet1Condition : PlanetCondition
{
    public override int VerifyConditions(Planet planet)
    {
        return -1;
    }
}
