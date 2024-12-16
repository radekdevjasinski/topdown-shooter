using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Player player;
    public Vector3 shift;  // Przesunięcie względem gracza;
    void Update()
    {
        transform.position = player.transform.position - shift;
    }
}
