using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI timerText;
    public GameObject timer;
    [Header("Movement Settings")]
    public float ThrustSpeed = 10f;
    public float TurnSpeed = 100f;
    public float shipMaxVelocity = 10f;
    [Header("Respawn Settings")]
    public float respawnTime = 3f;
    public float invulnerabilityTime = 3f;
    [Header("Audio Settings")]
    public float thrustFadeSpeed = 2f;
    public AudioClip deathClip;
    public AudioClip thrustLoopClip;
    public AudioClip shootClip;
    public AudioClip respawnClip;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private ParticleSystem _thrusterParticles;
    private bool _isAlive = true;
    private bool _isEjecting = false;
    [SerializeField] private bool _inOpenWorld = false;
    // Inputs
    public InputActionAsset PlayerInput;
    private InputAction _thrustAction;
    private InputAction _turnAction;
    private InputAction _shootAction;
    private InputAction _ejectAction;
    private float _turnDirection;
    private bool _thrusting;
    // Spaceship
    private PlayerSpaceship _spaceship;
    private void Awake()
    {   
        // spaceship from DataCarrier
        _spaceship = DataCarrier.playerSpaceship;
        _spaceship.ResetShootTimer();

        // visuals
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _spaceship.shipSprite;

        // spaceship stats
        ThrustSpeed = _spaceship.thrustSpeed;
        TurnSpeed = _spaceship.turnSpeed;
        shipMaxVelocity = _spaceship.maxVelocity;

        // audio
        thrustLoopClip = _spaceship.thrustLoopClip;
        shootClip = _spaceship.shootClip;
        deathClip = _spaceship.deathClip;
        respawnClip = _spaceship.respawnClip;

        // components
        _rb = GetComponent<Rigidbody2D>();
        _thrusterParticles = GetComponentInChildren<ParticleSystem>();

        // inputs
        _thrustAction = PlayerInput.FindAction("Thrust");
        _turnAction = PlayerInput.FindAction("Turn");
        _shootAction = PlayerInput.FindAction("Shoot");
        _ejectAction = PlayerInput.FindAction("Eject");

    }

        private void OnEnable()
        {
        _thrustAction = PlayerInput.FindAction("Thrust");
        _turnAction = PlayerInput.FindAction("Turn");
        _shootAction = PlayerInput.FindAction("Shoot");
        _ejectAction = PlayerInput.FindAction("Eject");

        _thrustAction?.Enable();
        _turnAction?.Enable();
        _shootAction?.Enable();
        _ejectAction?.Enable();

        _shootAction.performed += OnShoot;
        _ejectAction.performed += HandleEjecting;

    }

    private void OnDisable()
    {

        _shootAction.performed -= OnShoot;
        _ejectAction.performed -= HandleEjecting;

        _thrustAction?.Disable();
        _turnAction?.Disable();
        _shootAction?.Disable();
        _ejectAction?.Disable();
    }

    private void OnDestroy()
    {
        if (!_inOpenWorld)
        {
            _shootAction.performed -= OnShoot;
            _ejectAction.performed -= HandleEjecting;
        }

        Events.OnWinningCondition -= OnWin;
    }

    private void Update()
    {
        if (!_isAlive) return;

        HandleInput();
        UpdateThrusterEffects();
    }

    private void FixedUpdate()
    {
        if (!_isAlive) return;

        if (_thrusting)
        {
            _rb.AddForce(transform.up * ThrustSpeed);
            _rb.linearVelocity = Vector2.ClampMagnitude(_rb.linearVelocity, shipMaxVelocity);
        }

        if (Mathf.Abs(_turnDirection) > 0.01f)
        {
            _rb.rotation += _turnDirection * TurnSpeed * Time.fixedDeltaTime;
        }
    }

    private void HandleInput()
    {
        _thrusting = _thrustAction.ReadValue<float>() > 0.5f;
        _turnDirection = _turnAction.ReadValue<float>();    
    }

    private void UpdateThrusterEffects()
    {
        if (_thrusterParticles != null)
        {
            var emission = _thrusterParticles.emission;
            emission.rateOverTime = _thrusting ? 50f : 0f;
        }

        if (_thrusting)
            AudioManager.Instance.PlayLoop(thrustLoopClip, true, thrustFadeSpeed);
        else
            AudioManager.Instance.PlayLoop(thrustLoopClip, false, thrustFadeSpeed);
    }

    private void OnShoot(InputAction.CallbackContext ctx)
    {
        if (!_isAlive) return;
        if (_inOpenWorld) return;

        bool fired = _spaceship.FireWeapon(transform);

        if (fired && _spaceship.shootClip != null)
            AudioManager.Instance.PlaySound(_spaceship.shootClip);
    }

    private void HandleEjecting(InputAction.CallbackContext ctx)
    {
        if (_isEjecting) return;
        _isEjecting = true;
        Events.Ejecting();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
        if (asteroid != null)
        {
            if (deathClip != null)
                AudioManager.Instance.PlaySound(deathClip);

            _rb.linearVelocity = Vector2.zero;
            _rb.angularVelocity = 0f;

            AudioManager.Instance.StopLoop();
            gameObject.SetActive(false);

            _isAlive = false;
            Events.PlayerDeath(Events.RequestLives() - 1);

            if (Events.RequestLives() > 0)
                Invoke(nameof(Respawn), respawnTime);
        }
    }

    public void Respawn()
    {
        if (respawnClip != null)
            AudioManager.Instance.PlaySound(respawnClip);

        transform.position = Vector3.zero;
        gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
        gameObject.SetActive(true);

        _isAlive = true;
        StartCoroutine(InvulnerabilityFlash());
    }

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

    private void TurnOnCollisions()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void OnWin()
    {
        gameObject.SetActive(false);
    }

    public void ApplyNewShip()
    {
        var ship = DataCarrier.playerSpaceship;

        _spriteRenderer.sprite = ship.shipSprite;
        ThrustSpeed = ship.thrustSpeed;
        TurnSpeed = ship.turnSpeed;
        shipMaxVelocity = ship.maxVelocity;
        thrustLoopClip = ship.thrustLoopClip;
    }
}
