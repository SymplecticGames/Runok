using Cinemachine;
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

    public CinemachineFreeLook freelookCam;

    [HideInInspector]
    public float camOrbitRadius;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // Activate golem
        currentCharacter = golemBehaviour;

        camLookAtTarget.parent = currentCharacter.transform;
        camLookAtTarget.localPosition = Vector3.zero;

        camOrbitRadius = freelookCam.m_Orbits[1].m_Radius;
        
        // Get golem animator
        animator = currentCharacter.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCharacter.TryGetComponent(out GolemBehaviour golem))
        {
            float factor = golem.insideLava ? 0.5f : 1.0f;
            currentCharacter.movementFactor = factor / golem.golemStats.weight;

            currentCharacter.maxJumps = golem.golemStats.jumps;

            if (!currentCharacter.controller.isGrounded && currentCharacter.jumpFactor < currentCharacter.maxJumpFactor * 0.5f)
            {
                animator.SetBool("isJumping", false);
                animator.SetBool("isFalling", true);
            }
            animator.SetBool("isFalling", !currentCharacter.controller.isGrounded);
        }
        else
        {
            currentCharacter.movementFactor = 1.0f;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();
        currentCharacter.movementInput = movement;
        animator.SetBool("isWalking", movement.magnitude > 0);
        animator.SetFloat("WalkSpeed", Mathf.Clamp(movement.magnitude, 0.1f, 1.0f));
    }

    public void OnActiveBw_Jump(InputAction.CallbackContext context)
    {
        currentCharacter.jumpPressed = context.performed;
        animator.SetBool("isJumping", true);
    }

    public void OnActiveFw_Hit(InputAction.CallbackContext context)
    {
        if (currentCharacter.TryGetComponent(out GolemBehaviour golem))
        {
            golem.hitPressed = context.performed;
            if (context.performed) animator.SetTrigger("isHitR");
        }

        if (currentCharacter.TryGetComponent(out BeetleBehaviour bettle))
            bettle.fwSkillPressed = context.performed;
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

            camLookAtTarget.parent = currentCharacter.transform;
            camLookAtTarget.localPosition = Vector3.zero;
            
            // Get beetle animator
            animator = currentCharacter.GetComponent<Animator>();
        }
        else
        {
            // Activate golem
            currentCharacter = golemBehaviour;

            camLookAtTarget.parent = currentCharacter.transform;
            camLookAtTarget.localPosition = Vector3.zero;
            
            // Get golem animator
            animator = currentCharacter.GetComponent<Animator>();
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

        golemBehaviour.GetComponent<GolemBehaviour>().insideLava = false;

        GameManager.instance.newDeath();
    }
}
