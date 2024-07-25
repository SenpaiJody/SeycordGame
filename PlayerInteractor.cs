using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
The PlayerInteractor component is a component that will exist on the InteractionBox GameObject, which is a child of the Player GameObject.

This component is responsible for using an attached collider and communicating with nearby Interactable Components, keeping them in a list and
invoking the Interactable.OnInteract() function,

-Jody 25/07/24
 */

[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerInteractor : MonoBehaviour
{
    private new CapsuleCollider2D collider;
    private List<Interactable> interactables;

    Interactable closestInteractable;
    private void Start()
    {
        collider = GetComponent<CapsuleCollider2D>();
        interactables = new List<Interactable>();
    }

    private void FixedUpdate()
    {
        //Updates the selected Interactable every Physics Frame (Update() works fine too)
        SelectClosestInteractable();

    }

    //Set the Interactable closest to the player as the currently selected interactable
    private void SelectClosestInteractable()
    {
        if (interactables.Count == 0)
        {
            closestInteractable = null;
            return;
        }
        closestInteractable = interactables[0];

        for (int i = 1; i < interactables.Count; i++) { 
            
            if (Vector2.Distance(interactables[i].transform.position, transform.position) <
                    Vector2.Distance(closestInteractable.transform.position, transform.position))
            {
                closestInteractable = interactables[i];
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Interactable interactable = collision.gameObject.GetComponent<Interactable>(); //try to get Interactable Component from collision target.
        if (interactable != null) 
        {
            interactables.Add(interactable);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Interactable interactable = collision.gameObject.GetComponent<Interactable>(); //try to get Interactable Component from collision target.
        if (interactable != null)
        {
            interactables.Remove(interactable);
        }
    }

    public void Interact()
    {
        if (closestInteractable != null)
            closestInteractable.Interact();
    }
}
