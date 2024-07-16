using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : WeaponBase
{
    private GameObject slashPrefab;
    private Animator playerAnimator;
    
    private float SlashSpeed = 100;

    public void Initialize(bool isActive, int level, float cooldown, float slashSpeed)
    {
        IsActive = isActive;
        Level = level;
        Cooldown = cooldown;
        cooldownTimer = 0;
        //default 100
        SlashSpeed = slashSpeed;

        slashPrefab = Resources.Load<GameObject>("Prefabs/Weapons/Slash");

        playerAnimator = Player.Instance.gameObject.GetComponent<Animator>();
    }

    protected override void Execute()
    {
        GameObject slash = Instantiate(slashPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = slash.GetComponent<Rigidbody2D>();
        Vector2 dir;
        if (playerAnimator.GetFloat("WasFacing") > 0)
        {
            dir = Vector2.right;
        }
        else
        {
            slash.transform.rotation = Quaternion.Euler(0, 180, 0);
            dir = Vector2.left;
        }
        rb.velocity = dir * SlashSpeed * Time.fixedDeltaTime;
    }
    protected override void Update()
    {
        base.Update();
    }
}
