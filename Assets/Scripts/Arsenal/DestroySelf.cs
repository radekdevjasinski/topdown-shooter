using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeObject : MonoBehaviour
{
    public void DestroySelfFunction()
    {
        Destroy(gameObject);
    }
    public void StopParticleSystem()
    {
        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Stop();
    }
}
