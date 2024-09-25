using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private Animator animator;
    public Vector2 movementInput;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //gracz zwr�cony w lewo
        animator.SetFloat("WasFacing", -1);
    }

    void FixedUpdate()
    {
        rigidbody.velocity = movementInput * Player.Instance.Speed * Time.fixedDeltaTime;
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