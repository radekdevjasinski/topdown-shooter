using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public float hp;
    [SerializeField] public float attack;


    protected GameObject playerRef;
    protected new Rigidbody2D rigidbody;

    protected virtual void Start()
    {
        playerRef = Player.Instance.gameObject;
        rigidbody = GetComponent<Rigidbody2D>();
    }

    protected abstract void Move();
    public abstract void Damage(float amount);
    public abstract void Attack();

    protected virtual void FixedUpdate()
    {
        Move();
    }
}
