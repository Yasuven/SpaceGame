using UnityEngine;

[CreateAssetMenu(menuName = "Spaceships/GarbageFighter")]
public class GarbageFighter : PlayerSpaceship
{
    public Bullet bulletPrefab;

    [Header("Scatter Settings")]
    public int minBullets = 1;
    public int maxBullets = 3;
    public float maxSpreadAngle = 15f;

    [Header("Bullet Size")]
    public float minSizeMultiplier = 1f;
    public float maxSizeMultiplier = 2f;

    public override bool FireWeapon(Transform firePoint)
    {
        if (!CanShoot())
            return false;

        int bulletCount = Random.Range(minBullets, maxBullets + 1);

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = Random.Range(-maxSpreadAngle, maxSpreadAngle);
            Quaternion rot = firePoint.rotation * Quaternion.Euler(0f, 0f, angle);

            Bullet b = Object.Instantiate(
                bulletPrefab,
                firePoint.position,
                rot
            );

            float sizeMultiplier = Random.Range(
                minSizeMultiplier,
                maxSizeMultiplier
            );

            b.transform.localScale *= sizeMultiplier;
            b.Fire(rot * Vector2.up);
        }

        RegisterShot();
        return true;
    }
}
