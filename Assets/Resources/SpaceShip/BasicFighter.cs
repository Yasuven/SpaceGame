using UnityEngine;

[CreateAssetMenu(menuName = "Spaceships/Basic Fighter")]
public class BasicFighter : PlayerSpaceship
{
    public Bullet bulletPrefab;

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
