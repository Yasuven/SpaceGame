using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 0, -10f);

    void LateUpdate()
    {
        if (player == null) return;

        // Stick camera exactly to player position with offset
        transform.position = player.position + offset;
    }
}