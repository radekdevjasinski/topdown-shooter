using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private Rigidbody2D rigidbody2D;
    private Animator animator;
    private Vector2 movementInput;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        rigidbody2D.velocity = movementInput * speed * Time.fixedDeltaTime;
        animator.SetFloat("Horizontal", movementInput.x);
        animator.SetFloat("Speed", movementInput.sqrMagnitude);
    }
    private void OnMove(InputValue input)
    {
        movementInput = input.Get<Vector2>();
    }
}
