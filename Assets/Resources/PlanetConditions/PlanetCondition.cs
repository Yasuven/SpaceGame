using UnityEngine;

public abstract class PlanetCondition : ScriptableObject
{   
     public abstract int VerifyConditions(Planet planet);
}