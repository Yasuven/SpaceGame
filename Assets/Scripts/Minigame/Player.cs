using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("References")]
    public Bullet bulletPrefab;
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
    private bool _isPaused = false;
    private bool _isEjecting = false;
    [SerializeField] private bool _inOpenWorld = false;

    // Input System
    public InputActionAsset _inputActions;
    private InputAction _thrustAction;
    private InputAction _turnAction;
    private InputAction _shootAction;
    private InputAction _ejectAction;

    private float _turnDirection;
    private bool _thrusting;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _thrusterParticles = GetComponentInChildren<ParticleSystem>();

        _thrustAction = InputSystem.actions.FindAction("Thrust");
        _turnAction = InputSystem.actions.FindAction("Turn");
        _shootAction = InputSystem.actions.FindAction("Shoot");
        _ejectAction = InputSystem.actions.FindAction("Eject");

        // Events
        if (!_inOpenWorld)
        {
            _shootAction.performed += ctx => Shoot();
            _ejectAction.performed += ctx => HandleEjecting();
        }

        Events.OnWinningCondition += OnWin;
        Events.OnPauseGame += OnPauseGame;
        Events.OnResumeGame += OnResumeGame;
    }

    private void OnEnable()
    {
        _inputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        _inputActions.FindActionMap("Player").Disable();
    }

    private void OnDestroy()
    {
        if (!_inOpenWorld)
        {
            _shootAction.performed -= ctx => Shoot();
            _ejectAction.performed -= ctx => HandleEjecting();
        }
        Events.OnWinningCondition -= OnWin;
        Events.OnPauseGame -= OnPauseGame;
        Events.OnResumeGame -= OnResumeGame;
    }

    private void Update()
    {
        if (!_isAlive || _isPaused) return;

        HandleInput();
        UpdateThrusterEffects();
    }

    private void FixedUpdate()
    {
        if (!_isAlive || _isPaused) return;

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
        {
            AudioManager.Instance.PlayLoop(thrustLoopClip, true, thrustFadeSpeed);
        }
        else
        {
            AudioManager.Instance.PlayLoop(thrustLoopClip, false, thrustFadeSpeed);
        }
    }

    private void HandleEjecting()
    {
        if (_isEjecting) return;
        _isEjecting = true;
        Events.Ejecting();
    }

    private void Shoot()
    {
        if (!_isAlive || _isPaused) return;

        Bullet bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.Fire(transform.up);
        if (shootClip != null)
        {
            AudioManager.Instance.PlaySound(shootClip);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
        if (asteroid != null)
        {
            if (deathClip != null) { AudioManager.Instance.PlaySound(deathClip); }

            _rb.linearVelocity = Vector2.zero;
            _rb.angularVelocity = 0f;

            AudioManager.Instance.StopLoop();

            gameObject.SetActive(false);
            _isAlive = false;

            Events.PlayerDeath(Events.RequestLives() - 1);
            if (Events.RequestLives() > 0)
            {
                Invoke(nameof(Respawn), respawnTime);
            }
        }
    }

    public void Respawn()
    {
        if (respawnClip != null) { AudioManager.Instance.PlaySound(respawnClip); }

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

    private void OnPauseGame()
    {

        _inputActions.FindActionMap("Player").Disable();
    }

    private void OnResumeGame()
    {

        _inputActions.FindActionMap("Player").Enable();
    }
}
