using UnityEngine;

[CreateAssetMenu(menuName = "PlanetConditions/Planet6")]
public class Planet6Condition : PlanetCondition
{
    public override int VerifyConditions(Planet planet)
    {
        return -1;
    }
}
