using UnityEngine;

[System.Serializable]
public class Weapon
{
    public string weaponName;
    public float damage;
    public GameObject weaponModel;

    public Weapon(string name, float dmg, GameObject model)
    {
        weaponName = name;
        damage = dmg;
        weaponModel = model;
    }

    public void Fire()
    {
        Debug.Log("Firing weapon: " + weaponName);
    }
}
