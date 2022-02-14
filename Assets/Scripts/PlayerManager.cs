using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private GenericBehaviour currentCharacter;

    [SerializeField]
    private GenericBehaviour golemBehaviour;

    [SerializeField]
    private GenericBehaviour beetleBehaviour;

    [SerializeField]
    private Transform camLookAtTarget;

    // Start is called before the first frame update
    void Start()
    {
        // Activate golem
        currentCharacter = golemBehaviour;

        golemBehaviour.isActive = true;
        beetleBehaviour.isActive = false;

        camLookAtTarget.parent = golemBehaviour.transform;
        camLookAtTarget.localPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();

        currentCharacter.movementInput = movement;
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        currentCharacter.isRunning = context.performed;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        currentCharacter.jumpPressed = context.performed;
    }

    public void SwapCharacter(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (golemBehaviour.isActive)
        {
            // Activate beetle
            currentCharacter = beetleBehaviour;

            //golemBehaviour.isActive = false;
            //beetleBehaviour.isActive = true;

            camLookAtTarget.parent = beetleBehaviour.transform;
            camLookAtTarget.localPosition = Vector3.zero;
        }
        else
        {
            // Activate golem
            currentCharacter = golemBehaviour;

            //golemBehaviour.isActive = true;
            //beetleBehaviour.isActive = false;

            camLookAtTarget.parent = golemBehaviour.transform;
            camLookAtTarget.localPosition = Vector3.zero;
        }
    }
}
