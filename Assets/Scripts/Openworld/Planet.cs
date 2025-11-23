using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour
{
    [Header("Planet Settings")]
    public string planetId;
    public DialogueData dialogueData;
    public int currentNode;

    [Header("References")]
    public Transform dialogueSpot;     // where player moves to
    public Transform textSpot;         // where dialogue appears
    public DialogueWindow dialogueCanvas;  // world-space prefab reference
    public PlanetCondition specialConditions;
    public PlanetEvents events;

    [Header("Timing")]
    public float moveDuration = 1.5f;
    public float disableControlsTime = 1f;
    private bool isPlayerInteracting = false;
    public PlanetState GetState()
    {
        return new PlanetState(this);
    }

    public void ApplyState(PlanetState state)
    {
        planetId = state.planetId;
        currentNode = state.currentNode;
        transform.position = state.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Current node is: " + currentNode);
        if (other.CompareTag("Player") && !isPlayerInteracting)
        {
            if (specialConditions != null)
            {
                int result = specialConditions.VerifyConditions(this);
                if (result > 0)
                {
                    currentNode = result;
                    Debug.Log($"[Planet] Condition triggered, starting from node {currentNode}");
                }
            }
            isPlayerInteracting = true;
            StartCoroutine(HandlePlayerDialogue(other.transform));
        }
    }

    private IEnumerator HandlePlayerDialogue(Transform player)
    {
        var playerScript = player.GetComponent<PlayerOpenWorld>();
        var rb = player.GetComponent<Rigidbody2D>();

        AudioClip thrustClip = null;
    float fadeDuration = 0f;
    if (playerScript != null)
    {
        thrustClip = playerScript.thrustLoopClip;
        fadeDuration = 1f / playerScript.thrustFadeSpeed; 
        playerScript.enabled = false;
      }

        if (playerScript) playerScript.enabled = false;
        if (rb)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

    if (AudioManager.Instance != null && thrustClip != null)
    {
        float startVolume = AudioManager.Instance._loopSource.volume;
        float elapsedFade = 0f;
        
        while (elapsedFade < fadeDuration)
        {
            float currentVolume = Mathf.Lerp(startVolume, 0f, elapsedFade / fadeDuration);
            AudioManager.Instance._loopSource.volume = currentVolume;
            
            elapsedFade += Time.deltaTime;
            yield return null;
        }

        AudioManager.Instance.StopLoop(); 
    }

        Vector3 startPos = player.position;
        Vector3 targetPos = dialogueSpot != null ? dialogueSpot.position : transform.position;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            player.position = Vector3.Lerp(startPos, targetPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        player.position = targetPos;

        if (dialogueCanvas != null && textSpot != null)
        {
            var window = Instantiate(dialogueCanvas, textSpot.position, Quaternion.identity, textSpot);


            var cam = Camera.main;
            if (cam != null)
            {
                var canvas = window.GetComponent<Canvas>();
                if (canvas) canvas.worldCamera = cam;
                window.transform.forward = cam.transform.forward;
            }
            

            window.StartDialogue(dialogueData, currentNode, this); // WOW THIS SHOULD WORK

            yield return new WaitUntil(() => window.IsFinished);
            Destroy(window.gameObject);
        }

        yield return new WaitForSeconds(disableControlsTime);
        if (playerScript) playerScript.enabled = true;
        if (rb) rb.bodyType = RigidbodyType2D.Dynamic;

        isPlayerInteracting = false;
    }
}
