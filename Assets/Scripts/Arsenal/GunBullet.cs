using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBullet : MonoBehaviour
{
    [SerializeField] private float destroySelfTime = 10f;
    private List<GameObject> hitEnemies = new();

    void Start()
    {
        StartCoroutine(DestroySelf());
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (hitEnemies.Count <= 0)
            {
                collision.gameObject.GetComponent<Enemy>().Damage(GameObject.Find("Gun").GetComponent<Gun>().GunDamage);
                Destroy(gameObject);
                hitEnemies.Add(collision.gameObject);
            }
            
        }
        if (collision.gameObject.CompareTag("Barrel"))
        {
            collision.gameObject.GetComponent<BarrelObject>().Boom();
            Destroy(gameObject);
        }
    }
    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(destroySelfTime);
        Destroy(gameObject);
    }
}
