using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarrelObject : MonoBehaviour
{
    public float duration;
    private Barrel barrel;
    private GameObject explosionPrefab;
    private Slider slider;
    private bool alreadyBoomed;
    void Start()
    {

        barrel = GameObject.Find("Barrel").GetComponent<Barrel>();
        explosionPrefab = Resources.Load<GameObject>("Prefabs/Weapons/Barrel Explosion");
        duration = barrel.barrelDuration;
        slider = GetComponentInChildren<Slider>();
        slider.minValue = 0;
        slider.maxValue = barrel.barrelDuration;
        alreadyBoomed = false;

    }
    public void Boom()
    {
        if (alreadyBoomed)
        {
            return;
        }
        alreadyBoomed = true;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, barrel.explosionRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.gameObject.GetComponent<Enemy>().Damage(barrel.barrelDamage);
            }
            if (collider.CompareTag("Barrel"))
            {
                //
                StartCoroutine(DominoBoom(collider.gameObject.GetComponent<BarrelObject>()));
                //
            }
        }

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponentInChildren<Canvas>().enabled = false;
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        explosion.GetComponent<ParticleSystem>().Play();
        StartCoroutine(Destroy(explosion.GetComponent<ParticleSystem>().duration));
        
    }
    IEnumerator Destroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
    IEnumerator DominoBoom(BarrelObject barrelObject)
    {
        yield return new WaitForSeconds(barrel.dominoTime);
        barrelObject.Boom();
    }
    void Update()
    {
        if(duration > 0)
        {
            duration -= Time.deltaTime;
        }
        duration = Mathf.Clamp(duration, 0, barrel.barrelDuration);
        slider.value = duration;
    }
}
