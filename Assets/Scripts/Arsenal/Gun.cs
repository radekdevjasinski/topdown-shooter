using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : WeaponBase
{

    private GameObject bulletPrefab;
    [SerializeField] private float GunCooldown;
    [SerializeField] private float DetectionRadius;
    [SerializeField] private float BulletSpeed;

    private bool canShoot = true;
    private Vector3 targetPosition;

    void Start()
    {
        Level = 1;
        Cooldown = GunCooldown;
        Activate();

        bulletPrefab = Resources.Load<GameObject>("Prefabs/Weapons/Bullet");
    }

    protected override void Update()
    {
        // Wykryj przeciwnik�w w pobli�u
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, DetectionRadius, LayerMask.GetMask("Enemy"));

        if (enemiesInRange.Length > 0 && canShoot)
        {
            // Znajd� najbli�szego przeciwnika
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
            // Uruchom coroutine do strzelania
            StartCoroutine(ShootCoroutine());
        }
    }

    IEnumerator ShootCoroutine()
    {
        // Ustaw flag� na false, aby zapobiec kolejnemu strza�owi
        canShoot = false;

        // Poczekaj 5 sekund
        yield return new WaitForSeconds(5f);

        // Ustaw flag� na true, aby gracz m�g� ponownie strzela�
        canShoot = true;
    }

    protected override void Execute()
    {
        // Oblicz kierunek do przeciwnika
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Stw�rz pocisk i ustaw jego pozycj� oraz rotacj�
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // Ustaw pr�dko�� pocisku
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * BulletSpeed * Time.fixedDeltaTime;
    }
    // Rysowanie zasi�gu wykrywania w edytorze Unity
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DetectionRadius);
    }
}
