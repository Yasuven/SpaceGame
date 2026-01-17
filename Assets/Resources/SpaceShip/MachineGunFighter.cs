using UnityEngine;

[CreateAssetMenu(menuName = "Spaceships/MachineGunFighter")]
public class MachineGunFighter : PlayerSpaceship
{
    public Bullet bulletPrefab;

    public float fireDelay;

    public override bool FireWeapon(Transform firePoint)
    {
        if (!CanShoot()) return false;

        Bullet bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Debug.Log("in FireWeapons");
        bullet.Fire(firePoint.up);

        RegisterShot();
        return true;
    }
}
