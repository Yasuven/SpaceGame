using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 500f;
    public float maxLifetime = 10f;
    public int Damage = 1;
    public ParticleSystem explosionPrefab;
    public ObjectPool explosionPool;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Fire(Vector2 direction)
    {
        _rb.AddForce(direction * Speed);

        Destroy(gameObject, maxLifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    private void Update()
    {
        Bounds bounds = OrthographicBounds(Camera.main);

        if (transform.position.x >= bounds.min.x && 
            transform.position.x <= bounds.max.x &&
            transform.position.y >= bounds.min.y && 
            transform.position.y <= bounds.max.y)
        {
            // Inside bounds
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Bounds OrthographicBounds(Camera camera)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }

}
