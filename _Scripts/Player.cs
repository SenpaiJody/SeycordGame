using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*
Basic Player Class.
 
Like all other GameObjects, inputs should not be handled directly but rather hooked onto an InputController's events.

Physics of movement have been abstracted out to the CharacterPhysics component. 
As a result, the Player class should never need a reference to its own rigidbody.

If it would later make sense to (and it probably will), animations and effects should be abstracted to some component of its own.
In that case, the Player class would never need a reference to its own SpriteRenderer or Animator.
However, that is a bridge we'll cross if and when we get there

-Jody 24/07/2024
 */
[RequireComponent(typeof(CharacterPhysics))]
public class Player : MonoBehaviour
{
   
    private CharacterPhysics CharacterPhysics;
    private PlayerInteractor interactor;


    private bool isFacingRight = true;

    private SpriteRenderer sprite;
    private Animator animator;
    


    public InputAction OnMove;

    void Start()
    {
        CharacterPhysics = GetComponent<CharacterPhysics>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        interactor = GetComponentInChildren<PlayerInteractor>();

        InputController.instance.OnMove.AddListener(Move);
        InputController.instance.OnInteract.AddListener(Interact);
    }


    public void Interact()
    {
        interactor.Interact();
    }

    public void Move(Vector2 m){

        CharacterPhysics.mvmnt = m;
        if (m.x > 0)
            isFacingRight = true;
        else if (m.x < 0)
            isFacingRight = false;

        animator.SetFloat("Movement", m.magnitude);
    }
    
    void FlipToFacingDirection()
    {
        sprite.flipX = !isFacingRight;   
    }

    void FixedUpdate()
    {
        FlipToFacingDirection();
    }
}
