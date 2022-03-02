using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector]
    public GenericBehaviour currentCharacter;

    [SerializeField]
    private Transform respawnPoint;

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

    public void OnActiveBw_Jump(InputAction.CallbackContext context)
    {
        currentCharacter.jumpPressed = context.performed;
    }

    public void OnActiveFw_Hit(InputAction.CallbackContext context)
    {
        if (currentCharacter.TryGetComponent(out GolemBehaviour golem))
            golem.hitPressed = context.performed;
    }

    public void SwapCharacter(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        currentCharacter.movementInput = Vector2.zero;

        if (currentCharacter == golemBehaviour)
        {
            // Activate beetle
            currentCharacter = beetleBehaviour;

            camLookAtTarget.parent = beetleBehaviour.transform;
            camLookAtTarget.localPosition = Vector3.zero;
        }
        else
        {
            // Activate golem
            currentCharacter = golemBehaviour;

            camLookAtTarget.parent = golemBehaviour.transform;
            camLookAtTarget.localPosition = Vector3.zero;
        }
    }

    public void Checkpoint(Transform newRespawnPoint)
    {
        Debug.Log("Checkpoint");
        respawnPoint = newRespawnPoint;
    }

    public void Die()
    {
        golemBehaviour.Die(respawnPoint);
        beetleBehaviour.Die(respawnPoint);
        
        GameManager.instance.newDeath();
    }
}
