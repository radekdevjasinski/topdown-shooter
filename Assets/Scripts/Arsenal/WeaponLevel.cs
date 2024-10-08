using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponLevel", menuName = "Weapon/WeaponLevel")]
public class WeaponLevel : ScriptableObject
{
    public int weaponId;
    public int level;
    public string title;
    public string description;
    public Sprite image;
    public float damageChange;
    public float cooldownChange;
}