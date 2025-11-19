using UnityEngine;
using UnityEngine.UIElements;

public class Asteroid : MonoBehaviour
{
    public Sprite[] sprites;
    public float size = 1f;
    public float minSize = 0.5f;
    public float maxSize = 1.5f;
    public float speed = 50f;
    public float maxLifetime = 30f;
    public AudioClip[] explosionClips;

    public int Health = 1;
    public int[] ScoreValues;
    public AsteroidData asteroidType;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;

    public void Init(AsteroidData type)
    {
        asteroidType = type;
        sprites = asteroidType.AsteroidSprites;
        Health = asteroidType.Health;
        ScoreValues = asteroidType.ScoreValues;
    }

    private void Awake()
    {
        
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();

    }

    // Initializes the asteroid with random sprite, rotation, scale, and mass
    private void Start()
    {
        _spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

        transform.eulerAngles = new Vector3(0f, 0f, Random.Range(0f, 360f));
        transform.localScale = Vector3.one * size;

        _rb.mass = size;
    }


    // Sets the trajectory of the asteroid
    public void SetTrajectory(Vector2 direction)
    {
        _rb.AddForce(direction * speed);

       Destroy(gameObject, maxLifetime);
    }

    // Handles collision with bullets
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();

        if (bullet != null)
        {
            Health -= bullet.Damage;
            if (Health <= 0) { 

                if (explosionClips != null && explosionClips.Length > 0)
                {int randomIndex = Random.Range(0, explosionClips.Length);AudioManager.Instance.PlaySound(explosionClips[randomIndex]);}
            
                if (size / 2f >= minSize)
                {
                    CreateSplit();
                    CreateSplit();
                }
                Events.AsteroidDestroyed(this);
                Destroy(gameObject);
            }
        }

    }

    // Creates a smaller asteroid split from the current one
    private void CreateSplit()
    {
        Vector2 position = transform.position;
        position += Random.insideUnitCircle * 0.5f;

        Asteroid half = Instantiate(this, position, transform.rotation);
        half.size = size / 2f;
        half.transform.localScale = Vector3.one * half.size;

        half.asteroidType = asteroidType;
        half.Health = asteroidType.Health;
        half.sprites = asteroidType.AsteroidSprites;
        half.ScoreValues = asteroidType.ScoreValues;

        half.SetTrajectory(Random.insideUnitCircle.normalized * speed);

    }





}
