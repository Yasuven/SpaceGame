using UnityEngine;

public abstract class PlayerSpaceship : ScriptableObject
{
    public string spaceshipName;
    public Sprite shipSprite;

    // movement
    public float thrustSpeed;
    public float turnSpeed;
    public float maxVelocity;

    // shooting
    public float shootInterval = 0.25f;
    protected float _lastShotTime = -999f;

    // audio
    public AudioClip thrustLoopClip;
    public AudioClip shootClip;
    public AudioClip deathClip;
    public AudioClip respawnClip;

    public abstract bool FireWeapon(Transform firePoint);

    protected bool CanShoot()
    {
        return Time.time - _lastShotTime >= shootInterval;
    }

    protected void RegisterShot()
    {
        _lastShotTime = Time.time;
    }

    public void ResetShootTimer()
    {
        _lastShotTime = -999f;
    }
}
