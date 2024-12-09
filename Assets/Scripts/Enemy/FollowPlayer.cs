using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Vector3 shift;  // Przesunięcie względem gracza;
    void Update()
    {
        transform.position = Player.Instance.transform.position - shift;
    }
}
