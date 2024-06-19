using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBullet : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        // SprawdŸ, czy pocisk zderzy³ siê z przeciwnikiem
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Zniszcz przeciwnika
            Destroy(collision.gameObject);

            // Zniszcz pocisk
            Destroy(gameObject);
        }
    }
}
