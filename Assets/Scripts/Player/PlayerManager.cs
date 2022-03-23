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
    private bool isJumping;
    private bool isFalling;

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
        isFalling = false;
        isJumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCharacter.TryGetComponent(out GolemBehaviour golem))
        {
            float factor = golem.insideLava ? 0.5f : 1.0f;
            currentCharacter.movementFactor = factor / golem.golemStats.weight;

            currentCharacter.maxJumps = golem.golemStats.jumps;

            Debug.Log(currentCharacter.playerVel.y);
            if (!isFalling && isJumping && currentCharacter.playerVel.y < 0.0)
            {
                animator.SetBool("isJumping", false);
                animator.SetBool("isFalling", true);
                isFalling = true;
                isJumping = false;
            }
            else if (currentCharacter.isGrounded && isFalling)
            {
                animator.SetBool("isFalling", false);
                isFalling = false;
            }
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
        animator.SetBool("isWalking", movement.y != 0 || movement.x != 0);
    }

    public void OnActiveBw_Jump(InputAction.CallbackContext context)
    {
        currentCharacter.jumpPressed = context.performed;
        animator.SetBool("isJumping", true);
        isJumping = true;
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
