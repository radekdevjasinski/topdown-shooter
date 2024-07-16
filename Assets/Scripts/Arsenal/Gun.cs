using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : WeaponBase
{

    private GameObject bulletPrefab;
    private float DetectionRadius;
    private float BulletSpeed;

    private bool canShoot = true;
    private Vector3 targetPosition;

    public void Initialize(bool isActive, int level, float cooldown, float detectionRadius, float bulletSpeed)
    {
        IsActive = isActive;
        Level = level;
        Cooldown = cooldown;
        cooldownTimer = 0;
        //10
        DetectionRadius = detectionRadius;
        //500
        BulletSpeed = bulletSpeed;
        
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Weapons/Bullet");
    }

    protected override void Update()
    {
        // Wykryj przeciwnik�w w pobli�u
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, DetectionRadius, LayerMask.GetMask("Enemy"));

        // Je�li wykryto co najmniej jednego przeciwnika i gracz mo�e strzeli�
        if (enemiesInRange.Length > 0 && canShoot)
        {
            targetPosition = enemiesInRange[0].transform.position;
            // Uruchom coroutine do strzelania
            StartCoroutine(ShootCoroutine());
        }
    }

    IEnumerator ShootCoroutine()
    {
        // Ustaw flag� na false, aby zapobiec kolejnemu strza�owi
        canShoot = false;

        // Wystrzel pocisk w kierunku przeciwnika
        Execute();

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
