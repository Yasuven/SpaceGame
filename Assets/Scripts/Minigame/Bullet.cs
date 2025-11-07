using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 500f;
    public float maxLifetime = 10f;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
