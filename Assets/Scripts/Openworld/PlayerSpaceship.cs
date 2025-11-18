using UnityEngine;

[System.Serializable]
public class PlayerSpaceship
{
    public GameObject spaceshipModel;
    public string spaceshipName;
    public Weapon equippedWeapon;

    public PlayerSpaceship(GameObject model, string name, Weapon weapon)
    {
        spaceshipModel = model;
        spaceshipName = name;
        equippedWeapon = weapon;
    }

    public void FireWeapon()
    {
        if (equippedWeapon != null)
            Debug.Log("Firing weapon: " + equippedWeapon.weaponName);
    }

    public void GiveNewWeapon(Weapon newWeapon)
    {
        equippedWeapon = newWeapon;
        Debug.Log("New weapon equipped: " + equippedWeapon.weaponName);
    }
}