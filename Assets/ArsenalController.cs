using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArsenalController : MonoBehaviour
{
    public List<IWeapon> weapons = new();
    public GameObject gunGM;
    public GameObject swordGM;
    public GameObject Rotatable;

    private Animator playerAnimator;
    void Start()
    {
        playerAnimator = Player.Instance.GetComponent<Animator>();

        Gun gun = gunGM.GetComponent<Gun>();
        weapons.Add(gun);

        Sword sword = swordGM.GetComponent<Sword>();
        weapons.Add(sword);
    }
    void Update()
    {
        float wasFacing = playerAnimator.GetFloat("WasFacing");
        if (wasFacing > 0)
            Rotatable.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        else
            Rotatable.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }
}
