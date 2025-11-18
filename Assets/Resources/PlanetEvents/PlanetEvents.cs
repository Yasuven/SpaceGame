using UnityEngine;

public abstract class PlanetEvents : ScriptableObject
{
    public abstract void TriggerEvent(string eventId, Planet planet);
}
