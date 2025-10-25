using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Sprite[] sprites;
    public float size = 1f;
    public float minSize = 0.5f;
    public float maxSize = 1.5f;
    public float speed = 50f;
    public float maxLifetime = 30f;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

        transform.eulerAngles = new Vector3(0f, 0f, Random.Range(0f, 360f));
        transform.localScale = Vector3.one * size;

        _rb.mass = size;
    }

    public void SetTrajectory(Vector2 direction)
    {
        _rb.AddForce(direction * speed);

        Destroy(gameObject, maxLifetime);
    }
}
