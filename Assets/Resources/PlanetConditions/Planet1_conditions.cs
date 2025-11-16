using UnityEngine;

[CreateAssetMenu(menuName = "PlanetConditions/Planet1")]
public class Planet1Condition : PlanetCondition
{
    public override int VerifyConditions(Planet planet)
    {
        if (DataCarrier.points >= 1000)
        {
            Debug.Log("we are returning 3 points from conditions");
             return 3;
        }

        return -1;
    }
}
