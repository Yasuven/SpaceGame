using UnityEngine;

[CreateAssetMenu(menuName = "Spaceships/Basic Fighter")]
public class BasicFighter : PlayerSpaceship
{
    public Bullet bulletPrefab;

    public override void FireWeapon(Transform firePoint)
    {
        if (!CanShoot()) return;

        Bullet bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Debug.Log("in FireWeapons");
        bullet.Fire(firePoint.up);

        RegisterShot();
    }
}
