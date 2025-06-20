using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractHandler : MonoBehaviour
{
    [SerializeField]
    Transform cameraTransform; // so raycasts go where player would expect

    [SerializeField]
    float maxInteractDistance = 3f;

    InputAction interactAction;

    private bool wasInteracting = false;

    void Start()
    {
        interactAction = InputSystem.actions.FindAction("Interact");
    }

    void Update()
    {
        // detect when interact key is first pressed down
        if (interactAction.IsPressed())
        {
            if (!wasInteracting)
            {
                wasInteracting = true;
                
                // cast ray from camera in direction player is looking
                RaycastHit hit;
                if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, maxInteractDistance))
                {
                    // if object is interactable, tell it that the player is interacting with it
                    if (hit.collider.gameObject.CompareTag("Interactable"))
                    {
                        hit.collider.gameObject.GetComponent<Interactable>().Interact(gameObject);
                    }
                }
            }
        }
        else
        {
            wasInteracting = false;
        }
    }
}
