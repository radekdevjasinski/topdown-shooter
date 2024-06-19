using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private GameObject player;
    private Rigidbody2D rigidbody2D;

    void Start()
    {
        player = GameObject.Find("Player");
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rigidbody2D.velocity = direction * speed * Time.fixedDeltaTime;
        }
    }
}
