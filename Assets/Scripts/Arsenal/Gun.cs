using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : WeaponBase
{

    private GameObject bulletPrefab;
    [SerializeField] private float GunCooldown;
    [SerializeField] private float DetectionRadius;
    [SerializeField] private float BulletSpeed;
    public float GunDamage;

    private bool canShoot = true;
    private Vector3 targetPosition;
    private void Awake()
    {
        Level = 1;
        Cooldown = GunCooldown;
    }
    void Start()
    {
        Activate();

        bulletPrefab = Resources.Load<GameObject>("Prefabs/Weapons/Bullet");
    }

    protected override void Update()
    {
        LayerMask enemyLayer = LayerMask.GetMask("Enemy");
        LayerMask flyingEnemy = LayerMask.GetMask("FlyingEnemy");
        LayerMask barrel = LayerMask.GetMask("Barrel");

        LayerMask enemies = enemyLayer | flyingEnemy | barrel;
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, DetectionRadius, enemies);

        if (enemiesInRange.Length > 0 && canShoot)
        {
            Collider2D nearestEnemy = enemiesInRange[0];
            float shortestDistance = Vector2.Distance(transform.position, nearestEnemy.transform.position);

            foreach (Collider2D enemy in enemiesInRange)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    nearestEnemy = enemy;
                    shortestDistance = distanceToEnemy;
                }
            }

            targetPosition = nearestEnemy.transform.position;
            Execute();
            StartCoroutine(ShootCoroutine());
            
        }
        if (canShoot == false)
        {
            if (cooldownTimer < Cooldown) cooldownTimer += Time.deltaTime;
            if (cooldownTimer > Cooldown) cooldownTimer = Cooldown;
        }
    }

    IEnumerator ShootCoroutine()
    {
        canShoot = false;
        cooldownTimer = 0;
        yield return new WaitForSeconds(Cooldown);

        canShoot = true;
    }

    protected override void Execute()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * BulletSpeed * Time.fixedDeltaTime;
    }
    // Rysowanie zasiêgu wykrywania w edytorze Unity
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DetectionRadius);
    }
    public override void UpgradeToNextLevel(WeaponLevel weaponLevel)
    {
        base.UpgradeToNextLevel(weaponLevel);
        Cooldown += weaponLevel.cooldownChange;
        GunDamage += weaponLevel.damageChange;
        Level = weaponLevel.level;
    }
}
