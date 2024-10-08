using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ArsenalController : MonoBehaviour
{
    public List<WeaponBase> weapons = new();
    public List<WeaponLevel> choosenUpgrades = new();
    public GameObject gunGM;
    public GameObject swordGM;
    public GameObject Rotatable;

    public static ArsenalController instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private Animator playerAnimator;
    void Start()
    {
        playerAnimator = Player.Instance.GetComponent<Animator>();
        weapons = GetComponentsInChildren<WeaponBase>().ToList();
    }
    void Update()
    {
        float wasFacing = playerAnimator.GetFloat("WasFacing");
        if (wasFacing > 0)
            Rotatable.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        else
            Rotatable.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }
    public List<WeaponLevel> PossibleUpgrades()
    {
        choosenUpgrades = new List<WeaponLevel>();
        foreach (WeaponBase weapon in weapons)
        {
            if (weapon.getNextUpgrade() != null)
            {
                choosenUpgrades.Add(weapon.getNextUpgrade());
            }
        }
        if (choosenUpgrades.Count > 0)
        {
            return choosenUpgrades;
        }
        return null;
    }
    public void UpgradeWeaponWithId(int id, WeaponLevel weaponLevel)
    {
        weapons[id].UpgradeToNextLevel(weaponLevel);
    }
}
