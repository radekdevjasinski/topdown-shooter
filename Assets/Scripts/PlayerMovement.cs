using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float speed;
    private new Rigidbody2D rigidbody;
    private Animator animator;
    public Vector2 movementInput;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        speed = Player.Instance.Speed;
        //gracz zwrócony w lewo
        animator.SetFloat("WasFacing", -1);
    }

    void FixedUpdate()
    {
        rigidbody.velocity = movementInput * speed * Time.fixedDeltaTime;
        animator.SetFloat("Horizontal", movementInput.x);
        animator.SetFloat("Speed", movementInput.sqrMagnitude);
        if (movementInput.x != 0)
        {
            animator.SetFloat("WasFacing", movementInput.x);
        }
    }
    private void OnMove(InputValue input)
    {
        movementInput = input.Get<Vector2>();
    }
}
