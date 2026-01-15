using UnityEngine;

[CreateAssetMenu(menuName="PlanetEvents/Planet3")]
public class Planet3_Events : PlanetEvents
{
    [SerializeField] private GameObject asteroidAreaPrefab;
    public override void TriggerEvent(string eventId, Planet planet)
    {
        switch (eventId)
        {
            case "TUTORIAL_PASSED":
                DataCarrier.tutorialPassed = true;
                break;

            case "CHECKPOINTS800":
                if (DataCarrier.points >= 800)
                {
                    planet.currentNode = 11; 
                    break;
                }
                planet.currentNode = 9;
                break;

            case "CHECKPOINTS10000":
                if (DataCarrier.points >= 100000)
                {
                    planet.currentNode = 19; 
                    break;
                }
                planet.currentNode = 18;
                break;

            case "SPAWN_TUTORIAL_ASTEROIDS":
            {
                Transform anchor = GameObject.Find("TutorialAsteroidArea")?.transform;
                if (anchor == null) break;

                GameObject asteroidObj = Instantiate(
                    asteroidAreaPrefab,
                    anchor.position,
                    Quaternion.identity,
                    anchor
                );

                asteroidObj.name = "TutorialAsteroids";

                var trigger = asteroidObj.GetComponentInChildren<AsteroidsTrigger>();
                if (trigger != null)
                    trigger.areaName = "Tutorial";

                break;
            }

            case "REMOVE_TUTORIAL_ASTEROIDS":
            {
                Transform anchor = GameObject.Find("TutorialAsteroidArea")?.transform;
                if (anchor == null) break;

                foreach (Transform child in anchor)
                    Destroy(child.gameObject);

                break;
            }

            default:
                Debug.Log("Unknown event: " + eventId);
                break;
        }
    }
}
