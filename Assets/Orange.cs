using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Orange : WeaponBase
{
    [SerializeField] private float OrangeCooldown;
    [SerializeField] private float OrangeEatingTime;
    [SerializeField] private float heal;
    [SerializeField] private float speed;

    private GameObject orangePrefab;

    void Start()
    {
        Level = 1;
        Cooldown = OrangeCooldown;
        Activate();

        orangePrefab = Resources.Load<GameObject>("Prefabs/Weapons/Orange");
    }
    protected override void Execute()
    {
        GameObject orange = Instantiate(orangePrefab, transform.position, Quaternion.identity, transform);
        StartCoroutine(EatOrange(orange.GetComponent<Animator>()));

    }
    IEnumerator EatOrange(Animator animator)
    {
        yield return new WaitForSeconds(OrangeEatingTime);
        animator.SetTrigger("End");
        Player.Instance.ChangeHp(heal);
        Player.Instance.Speed += speed;
        
    }
}
