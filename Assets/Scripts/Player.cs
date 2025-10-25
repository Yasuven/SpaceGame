using UnityEngine;

public class Player : MonoBehaviour
{
    public Bullet bulletPrefab;

    public float ThrustSpeed = 10f;
    public float TurnSpeed = 100f;
    public float shipMaxVelocity = 10f;

    private Rigidbody2D _rb;
    private bool _thrusting;
    private float _turnDirection;
    private bool _isAlive = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
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

    private void Shoot()
    {
        Bullet bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.Fire(transform.up);
    }
}
