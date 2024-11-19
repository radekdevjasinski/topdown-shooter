using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    // Czas (w sekundach) po jakim obiekt zostanie zniszczony
    public float destroyTime;

    void Start()
    {
        // Zniszczenie obiektu po up�ywie podanego czasu
        Destroy(gameObject, destroyTime);
    }
}

