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

    void Start()
    {
        Level = 1;
        Cooldown = SwordCooldown;
        Activate();

        slashPrefab = Resources.Load<GameObject>("Prefabs/Weapons/Slash");

        playerAnimator = Player.Instance.gameObject.GetComponent<Animator>();
        playerRigidbody2D = Player.Instance.gameObject.GetComponent<Rigidbody2D>();
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
        rb.velocity = dir * SlashSpeed * Time.fixedDeltaTime + playerRigidbody2D.velocity;
    }
    protected override void Update()
    {
        base.Update();
    }
}
