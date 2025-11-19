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


}
