using UnityEngine;
using System.Collections;

public class PlayerEntersPlanet : MonoBehaviour
{
    public Transform dialoguePoint; 
    public float moveDuration = 1.5f;
    private bool isMovingToDialogue = false;

    public GameObject initialMessage;

    public float messageDuration = 6f;

    public void Start()
    {
        if (initialMessage != null) initialMessage.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isMovingToDialogue && collision.name == "Player")
        {
            isMovingToDialogue = true;
            StartCoroutine(MovePlayerToDialogue(collision.transform));
        }
    }


    private IEnumerator MovePlayerToDialogue(Transform player)
    {
        // disable movement
        var playerScript = player.GetComponent<PlayerOpenWorld>();
        if (playerScript != null) playerScript.enabled = false;

        var rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        Vector3 startPos = player.position;
        Vector3 targetPos = dialoguePoint.position;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            player.position = Vector3.Lerp(startPos, targetPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        player.position = targetPos;

        // make a proper dialogue window
        if (initialMessage != null)
        {
            StartCoroutine(ShowInitialMessage());
        }

        // enable movement
        if (playerScript != null) playerScript.enabled = true;
        if (rb != null) rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private IEnumerator ShowInitialMessage()
    {
        initialMessage.SetActive(true);
        yield return new WaitForSeconds(messageDuration);
        initialMessage.SetActive(false);
    }

}
