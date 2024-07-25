using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/*
Singleton that handles user input and user input events. Is based on the New Input System and relies on an InputActionAsset.
Purpose is to add a layer of abstraction between the input system and GameObjects that may read inputs.

No GameObject should have to access input directly from the InputSystem; rather, they should use callbacks defined here instead.

Polling (i.e., checking the value of an input every frame, rather than a callback once the value changes) *should* be possible, 
though I have yet to try.

-Jody 24/07/24
 */
public class InputController : MonoBehaviour
{
    public static InputController instance;

    [SerializeField] protected InputActionAsset ActionsAsset;

    private void Awake()
    {
        if (instance != null) {
            throw new System.Exception("Error! Second Instance of Singleton InputController");
        }
        instance = this;
    }

    InputAction Move;
    InputAction Interact;
    private void Start()
    {
        ActionsAsset.FindActionMap("Default").Enable(); //Enabling the action, this is to be moved to a more appropriate spot once we have a system for this

        Move = ActionsAsset.FindActionMap("Default").FindAction("Movement");
        Move.performed += (InputAction.CallbackContext ctx) => OnMove.Invoke(ctx.ReadValue<Vector2>()); //.performed is called when the button is pressed
        Move.canceled += (InputAction.CallbackContext ctx) => OnMove.Invoke(ctx.ReadValue<Vector2>()); //.canceled is called when the button is released

        Interact = ActionsAsset.FindActionMap("Default").FindAction("Interact");
        Interact.performed += (InputAction.CallbackContext ctx) => OnInteract.Invoke(); //.performed is called when the button is pressed
    }

    //events for GameObjects to hook onto 
    public UnityEvent<Vector2> OnMove = new();
    public UnityEvent OnInteract = new();




}
