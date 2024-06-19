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
    public Vector2 movementInput;

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
