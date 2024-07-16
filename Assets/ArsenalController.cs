using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArsenalController : MonoBehaviour
{
    public List<IWeapon> weapons = new();
    public GameObject gunGM;
    public GameObject swordGM;
    void Start()
    {
        Gun gun = gunGM.AddComponent<Gun>();
        gun.Initialize(true, 1, 5f, 10f, 500f);
        weapons.Add(gun);

        Sword sword = swordGM.AddComponent<Sword>();
        sword.Initialize(true, 1, 5f, 100f);
        weapons.Add(sword);
    }
}
