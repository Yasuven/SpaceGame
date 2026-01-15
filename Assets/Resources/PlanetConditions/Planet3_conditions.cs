using UnityEngine;

[CreateAssetMenu(menuName = "PlanetConditions/Planet3")]
public class Planet3Condition : PlanetCondition
{
    public override int VerifyConditions(Planet planet)
    {
        return -1;
    }
}
