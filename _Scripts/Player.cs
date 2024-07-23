using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    new private Rigidbody2D rigidbody;

    private float acceleration = 2f;
    private float friction = 0.2f;
    private Vector2 mvmnt;
    private bool isFacingRight = true;

    private SpriteRenderer sprite;
    private Animator animator;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();    
        sprite = GetComponentInChildren<SpriteRenderer>();    
        animator = GetComponentInChildren<Animator>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        mvmnt = context.ReadValue<Vector2>();

        if (mvmnt.x > 0)
            isFacingRight = true;
        else if (mvmnt.x < 0)
            isFacingRight = false;

        animator.SetFloat("Movement", mvmnt.magnitude);
        Debug.Log(animator.GetFloat("Movement"));
    }
    void ApplyFriction()
    {
        {
            rigidbody.velocity -= rigidbody.velocity * friction;
        }
    }
    void ApplyTraction()
    {
        rigidbody.velocity += mvmnt * acceleration;
    }

    void FlipToFacingDirection()
    {
        sprite.flipX = !isFacingRight;   
    }

    void FixedUpdate()
    {
        ApplyTraction();
        ApplyFriction();
        FlipToFacingDirection();
    }
}
