using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour, IAbility
{

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float bulletSpeed = 500f;

    private bool isActive = true;
    private bool canShoot = true;

    public bool IsActive => isActive;

    void Update()
    {
        if (isActive)
        {
            // Wykryj przeciwników w pobli¿u
            Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, detectionRadius, LayerMask.GetMask("Enemy"));

            // Jeœli wykryto co najmniej jednego przeciwnika i gracz mo¿e strzeliæ
            if (enemiesInRange.Length > 0 && canShoot)
            {
                // Uruchom coroutine do strzelania
                StartCoroutine(ShootCoroutine(enemiesInRange[0].transform.position));
            }
        }
    }

    IEnumerator ShootCoroutine(Vector3 targetPosition)
    {
        // Ustaw flagê na false, aby zapobiec kolejnemu strza³owi
        canShoot = false;

        // Wystrzel pocisk w kierunku przeciwnika
        Shoot(targetPosition);

        // Poczekaj 5 sekund
        yield return new WaitForSeconds(5f);

        // Ustaw flagê na true, aby gracz móg³ ponownie strzelaæ
        canShoot = true;
    }

    void Shoot(Vector3 targetPosition)
    {
        // Oblicz kierunek do przeciwnika
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Stwórz pocisk i ustaw jego pozycjê oraz rotacjê
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // Ustaw prêdkoœæ pocisku
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed * Time.fixedDeltaTime;
    }
    // Rysowanie zasiêgu wykrywania w edytorze Unity
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public void Activate(bool state)
    {
        isActive = true;
    }

    public void Upgrade()
    {
        throw new System.NotImplementedException();
    }
}
