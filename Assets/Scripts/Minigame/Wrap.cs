using UnityEngine;

public class Wrap : MonoBehaviour
{
 
    void Update()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        Vector3 moveAndjustment = Vector3.zero;
        if (viewportPosition.x < 0)
        {
            moveAndjustment.x += 1f;
        }
        else if (viewportPosition.x > 1)
        {
            moveAndjustment.x -= 1f;
        }
        else if (viewportPosition.y < 0)
        {
            moveAndjustment.y += 1f;
        }
        else if (viewportPosition.y > 1)
        {
            moveAndjustment.y -= 1f;
        }

        transform.position = Camera.main.ViewportToWorldPoint(viewportPosition + moveAndjustment);
    }
}
