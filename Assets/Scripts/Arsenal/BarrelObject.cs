using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarrelObject : MonoBehaviour
{
    public float duration;
    private Barrel barrel;
    private GameObject explosionPrefab;
    private GameObject gunPowderLine;
    private Slider slider;
    private bool alreadyBoomed;
    void Start()
    {

        barrel = GameObject.Find("Barrel").GetComponent<Barrel>();
        explosionPrefab = Resources.Load<GameObject>("Prefabs/Weapons/Barrel Explosion");
        gunPowderLine = Resources.Load<GameObject>("Prefabs/Weapons/Gunpowder Line");
        duration = barrel.barrelDuration;
        slider = GetComponentInChildren<Slider>();
        slider.minValue = 0;
        slider.maxValue = barrel.barrelDuration;
        alreadyBoomed = false;

        LinkGunpowder();

    }
    void LinkGunpowder()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, barrel.explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Barrel"))
            {
                GameObject gunLine = Instantiate(gunPowderLine, gameObject.transform.position, Quaternion.identity, gameObject.transform);
                LineRenderer lineRenderer = gunLine.GetComponent<LineRenderer>();
                lineRenderer.SetPosition(0, this.transform.position);
                lineRenderer.SetPosition(1, collider.gameObject.transform.position);
            }
        }
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
        LineRenderer[] lineRenderers = gameObject.GetComponentsInChildren<LineRenderer>();
        foreach (LineRenderer lineRenderer in lineRenderers)
        {
            lineRenderer.enabled = false;
        }
        explosion.GetComponent<ParticleSystem>().Play();
        StartCoroutine(Destroy(explosion.GetComponent<ParticleSystem>().main.duration));
        
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
