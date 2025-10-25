using System.Collections;
using UnityEngine;

public class PlayerOpenWorld : MonoBehaviour
{
    public float ThrustSpeed = 10f;
    public float TurnSpeed = 100f;
    public float shipMaxVelocity = 10f;

    private Rigidbody2D _rb;
    private ParticleSystem _thrusterParticles;
    private bool _thrusting;
    private float _turnDirection;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _thrusterParticles = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        HandleShipThrusting();
        HandleShipRotation();
    }

    private void FixedUpdate()
    {
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
            _turnDirection = 1f;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            _turnDirection = -1f;
        else
            _turnDirection = 0f;
    }
}
