using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    private List<GameObject> hitEnemies = new();
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!hitEnemies.Contains(collision.gameObject))
            {
                collision.gameObject.GetComponent<Enemy>().Damage(GameObject.Find("Sword").GetComponent<Sword>().SwordDamage);
                hitEnemies.Add(collision.gameObject);
            }
        }
           
        if (collision.gameObject.CompareTag("Barrel"))
        {
            collision.gameObject.GetComponent<BarrelObject>().Boom();
        }
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
