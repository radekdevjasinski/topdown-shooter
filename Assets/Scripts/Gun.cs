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
            // Wykryj przeciwnik�w w pobli�u
            Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, detectionRadius, LayerMask.GetMask("Enemy"));

            // Je�li wykryto co najmniej jednego przeciwnika i gracz mo�e strzeli�
            if (enemiesInRange.Length > 0 && canShoot)
            {
                // Uruchom coroutine do strzelania
                StartCoroutine(ShootCoroutine(enemiesInRange[0].transform.position));
            }
        }
    }

    IEnumerator ShootCoroutine(Vector3 targetPosition)
    {
        // Ustaw flag� na false, aby zapobiec kolejnemu strza�owi
        canShoot = false;

        // Wystrzel pocisk w kierunku przeciwnika
        Shoot(targetPosition);

        // Poczekaj 5 sekund
        yield return new WaitForSeconds(5f);

        // Ustaw flag� na true, aby gracz m�g� ponownie strzela�
        canShoot = true;
    }

    void Shoot(Vector3 targetPosition)
    {
        // Oblicz kierunek do przeciwnika
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Stw�rz pocisk i ustaw jego pozycj� oraz rotacj�
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // Ustaw pr�dko�� pocisku
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed * Time.fixedDeltaTime;
    }
    // Rysowanie zasi�gu wykrywania w edytorze Unity
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
