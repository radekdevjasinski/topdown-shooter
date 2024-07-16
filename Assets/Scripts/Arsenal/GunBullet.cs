using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBullet : MonoBehaviour
{
    [SerializeField] private float destroySelfTime = 10f;
    void Start()
    {
        StartCoroutine(DestroySelf());
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // SprawdŸ, czy pocisk zderzy³ siê z przeciwnikiem
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().Damage(1);

            // Zniszcz pocisk
            Destroy(gameObject);
        }
    }
    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(destroySelfTime);
        Destroy(gameObject);
    }
}
