using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
Script that handles basic character physics such as movement, friction and acceleration.

Ideally, the rigidbody should not need to be accessed directly by any GameObject, and they should instead modify values of an 
associated CharacterPhysics component

-Jody 24/07/24
 */

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterPhysics : MonoBehaviour
{

    new private Rigidbody2D rigidbody;

    public float acceleration = 2f;
    public float friction = 0.2f;
    public Vector2 mvmnt;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void ApplyFriction()
    {
         rigidbody.velocity -= rigidbody.velocity * friction;
    }
    void ApplyTraction()
    {
        rigidbody.velocity += mvmnt * acceleration;
    }


    void FixedUpdate()
    {
        ApplyTraction();
        ApplyFriction();
    }
}
