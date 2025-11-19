using UnityEngine;

[CreateAssetMenu(menuName = "Minigame/Asteroid Type")]
public class AsteroidData : ScriptableObject
{
    public Sprite[] AsteroidSprites;
    public int Health = 1;
    public int[] ScoreValues = {100, 50, 20};
}
