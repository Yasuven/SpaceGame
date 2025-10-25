using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Bullet bulletPrefab;

    public float ThrustSpeed = 10f;
    public float TurnSpeed = 100f;
    public float shipMaxVelocity = 10f;
    public float respawnTime = 3f;
    public float invulnerabilityTime = 3f;

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private ParticleSystem _thrusterParticles;
    private bool _thrusting;
    private float _turnDirection;
    private bool _isAlive = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _thrusterParticles = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        if (!_isAlive) return;

        HandleShipThrusting();
        HandleShipRotation();
        HandleShooting();
    }

    private void FixedUpdate()
    {
        if (!_isAlive) return;

        if (_thrusting)
        {
            _rb.AddForce(transform.up * ThrustSpeed);
            _rb.linearVelocity = Vector2.ClampMagnitude(_rb.linearVelocity, shipMaxVelocity);
        }

        if (_turnDirection != 0f)
        {
            _rb.rotation += _turnDirection * TurnSpeed * Time.fixedDeltaTime;
        }
    }

    private void HandleShipThrusting()
    {
        _thrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        
        if (_thrusterParticles != null)
        {
            var emission = _thrusterParticles.emission;
            emission.rateOverTime = _thrusting ? 50f : 0f;
        }
    }

    private void HandleShipRotation()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _turnDirection = 1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _turnDirection = -1f;
        }
        else
        {
            _turnDirection = 0f;
        }
    }

    private void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    // Shoots a bullet from the player's position
    private void Shoot()
    {
        Bullet bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.Fire(transform.up);
    }

    // Handles collision with asteroids
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
        if (asteroid != null)
        {
            _rb.linearVelocity = Vector2.zero;
            _rb.angularVelocity = 0f;

            gameObject.SetActive(false);
            _isAlive = false;

            Events.PlayerDeath(Events.RequestLives() - 1);
            if (Events.RequestLives() > 0)
            {
                Invoke(nameof(Respawn), respawnTime);
            }
        }
    }

    // Respawns the player at the center of the screen with temporary invulnerability
    public void Respawn()
    {
        transform.position = Vector3.zero;
        gameObject.layer = LayerMask.NameToLayer("Ignore Collisions"); 
        gameObject.SetActive(true);
        _isAlive = true;
        
        StartCoroutine(InvulnerabilityFlash());
    }

    // Makes the player flash and be invulnerable for a short time
    private IEnumerator InvulnerabilityFlash()
    {
        float elapsed = 0f;
        bool visible = true;

        while (elapsed < invulnerabilityTime)
        {
            visible = !visible;
            _spriteRenderer.enabled = visible;

            yield return new WaitForSeconds(0.2f);
            elapsed += 0.2f;
        }

        _spriteRenderer.enabled = true;
        TurnOnCollisions();
    }

    // Re-enables collisions for the player
    private void TurnOnCollisions()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
    }


}
