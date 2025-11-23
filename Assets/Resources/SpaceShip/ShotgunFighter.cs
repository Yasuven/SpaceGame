using UnityEngine;

[CreateAssetMenu(menuName = "Spaceships/ShotgunFighter")]
public class ShotgunFighter : PlayerSpaceship
{
    public Bullet bulletPrefab;
    public int pellets = 5;
    public float spreadAngle = 30f;

    public override void FireWeapon(Transform firePoint)
    {
        if (!CanShoot()) return;

        float step = spreadAngle / (pellets - 1);
        float startAngle = -spreadAngle / 2f;

        for (int i = 0; i < pellets; i++)
        {
            float angle = startAngle + i * step;
            Quaternion rot = firePoint.rotation * Quaternion.Euler(0, 0, angle);

            Bullet b = Object.Instantiate(bulletPrefab, firePoint.position, rot);
            b.Fire(rot * Vector2.up);
        }

        RegisterShot();
    }
}
