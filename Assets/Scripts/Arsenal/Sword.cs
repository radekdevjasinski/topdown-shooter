using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : WeaponBase
{
    private GameObject slashPrefab;
    private Animator playerAnimator;
    private Rigidbody2D playerRigidbody2D;

    [SerializeField] private float SwordCooldown;
    [SerializeField] private float SlashSpeed = 100;
    public float SwordDamage;

    private float defaultDamage;
    protected override void Start()
    {
        Cooldown = SwordCooldown;
        defaultDamage = SwordDamage;
        slashPrefab = Resources.Load<GameObject>("Prefabs/Weapons/Slash");

        playerAnimator = playerRef.gameObject.GetComponent<Animator>();
        playerRigidbody2D = playerRef.gameObject.GetComponent<Rigidbody2D>();
        
        base.Start();
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
        rb.velocity = dir * SlashSpeed * Time.fixedDeltaTime + new Vector2(playerRigidbody2D.velocity.x, 0);
    }
    protected override void Update()
    {
        base.Update();
    }
    public override void UpgradeToNextLevel(WeaponLevel weaponLevel)
    {
        base.UpgradeToNextLevel(weaponLevel);
        Cooldown += weaponLevel.cooldownChange;
        SwordDamage += weaponLevel.damageChange;
        Level = weaponLevel.level;
    }
    public override void ResetToDefaultLevel()
    {
        Cooldown = SwordCooldown;
        SwordDamage = defaultDamage;
        base.ResetToDefaultLevel();
    }
}
