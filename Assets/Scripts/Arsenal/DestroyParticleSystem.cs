using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticleSystem : MonoBehaviour
{
    void Start()
    {
        ParticleSystem particle = GetComponent<ParticleSystem>();
        float time = particle.main.duration + 1;
        StartCoroutine(destroyParticle(time));
    }
    IEnumerator destroyParticle(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
