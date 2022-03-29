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

    public GenericBehaviour golemBehaviour;

    public GenericBehaviour beetleBehaviour;

    [SerializeField]
    private LateralMenuUI lateralMenu;

    [SerializeField]
    private Transform camLookAtTarget;

    public CinemachineFreeLook freelookCam;

    [HideInInspector]
    public float camOrbitRadius;

    private Animator animator;
    private int comboCounter;
    private float lastComboHitTime;
    private float maxComboDelay;
    private bool continuousShot;

    private bool restingBeetle;
    private Light beetleLight;

    public bool selectionWheelEnabled;
    [HideInInspector] public PlayerInput input;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();

        if (selectionWheelEnabled)
            input.actions.FindAction("WheelMenu").Enable();
        else
            input.actions.FindAction("WheelMenu").Disable();
    }

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
        comboCounter = 0;
        lastComboHitTime = 0.0f;
        maxComboDelay = 0.7f;
        continuousShot = false;

        // Set beetle on Golem's back
        beetleLight = beetleBehaviour.GetComponentInChildren<Light>();
        AppendBeetle();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCharacter.TryGetComponent(out GolemBehaviour golem))
        {
            currentCharacter.movementFactor = 1.0f / golem.golemStats.weight;

            currentCharacter.maxJumps = golem.golemStats.jumps;

            if (!currentCharacter.controller.isGrounded && currentCharacter.jumpFactor < currentCharacter.maxJumpFactor * 0.5f)
            {
                animator.SetBool("isJumping", false);
            }
            animator.SetBool("isFalling", !currentCharacter.controller.isGrounded);

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f &&
                animator.GetCurrentAnimatorStateInfo(0).IsName("Hit1"))
            {
                animator.SetBool("isHit1", false);
            }
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f &&
                animator.GetCurrentAnimatorStateInfo(0).IsName("Hit2"))
            {
                animator.SetBool("isHit2", false);
            }
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f &&
                animator.GetCurrentAnimatorStateInfo(0).IsName("Hit3"))
            {
                animator.SetBool("isHit3", false);
                comboCounter = 0;
            }

            if (Time.time - lastComboHitTime > maxComboDelay)
            {
                comboCounter = 0;
            }
        }
        else if (currentCharacter.TryGetComponent(out BeetleBehaviour beetle))
        {
            currentCharacter.movementFactor = 1.0f;
            if (continuousShot && beetle.shootElapsedTime > beetle.shootCooldown)
                animator.SetTrigger("Shoot");
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();
        currentCharacter.movementInput = movement;
        animator.SetBool("isWalking", movement.magnitude > 0);
        if (currentCharacter == golemBehaviour)
            animator.SetFloat("WalkSpeed", Mathf.Clamp(movement.magnitude, 0.1f, 1.0f));
    }

    public void OnActiveBw_Jump(InputAction.CallbackContext context)
    {
        currentCharacter.jumpPressed = context.performed;
        if (currentCharacter == golemBehaviour && context.started)
            animator.SetBool("isJumping", true);
        else if (context.started)
        {
            animator.SetTrigger("Ray");
        }
    }

    public void OnActiveFw_Hit(InputAction.CallbackContext context)
    {
        if (currentCharacter.TryGetComponent(out GolemBehaviour golem))
        {
            golem.hitPressed = context.performed;
            if (context.started)
            {
                comboCounter++;
                lastComboHitTime = Time.time;

                if (comboCounter == 1)
                {
                    animator.SetBool("isHit1", true);
                }

                comboCounter = Mathf.Clamp(comboCounter, 0, 3);

                if (comboCounter >= 2 &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f &&
                    animator.GetCurrentAnimatorStateInfo(0).IsName("Hit1"))
                {
                    animator.SetBool("isHit1", false);
                    animator.SetBool("isHit2", true);
                }

                if (comboCounter >= 3 &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f &&
                    animator.GetCurrentAnimatorStateInfo(0).IsName("Hit2"))
                {
                    animator.SetBool("isHit2", false);
                    animator.SetBool("isHit3", true);
                }
            }
        }

        if (currentCharacter.TryGetComponent(out BeetleBehaviour beetle))
        {
            beetle.fwSkillPressed = context.performed;
            continuousShot = context.performed;
            if (context.started && beetle.shootElapsedTime > beetle.shootCooldown)
            {
                continuousShot = false;
                animator.SetTrigger("Shoot");
            }
        }
    }

    public void OnSwapCharacter(InputAction.CallbackContext context)
    {
        if (!context.performed || lateralMenu.menuOpen)
            return;
        SwapCharacter();
    }

    public void SwapCharacter()
    {
        currentCharacter.movementInput = Vector2.zero;

        if (currentCharacter == golemBehaviour)
        {
            // Stop any golem animation and go to idle
            animator.SetBool("isWalking", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
            animator.SetBool("isHit1", false);
            animator.SetBool("isHit2", false);
            animator.SetBool("isHit3", false);

            if (restingBeetle)
            {
                restingBeetle = false;
                beetleBehaviour.controller.enabled = true;
                beetleBehaviour.transform.SetParent(null);
            }

            // Activate beetle
            currentCharacter = beetleBehaviour;
            camLookAtTarget.parent = currentCharacter.transform;
            camLookAtTarget.localPosition = Vector3.zero;

            // Get beetle animator
            animator = currentCharacter.GetComponent<Animator>();
            animator.SetBool("isActive", true);  // Turn on beetle animator

            beetleLight.enabled = true;
        }
        else
        {
            // Activate golem
            currentCharacter = golemBehaviour;

            camLookAtTarget.parent = currentCharacter.transform;
            camLookAtTarget.localPosition = Vector3.zero;

            // Get golem animator
            animator.SetBool("isActive", false);  // Turn off beetle animator
            animator = currentCharacter.GetComponent<Animator>();
        }
    }

    public void ReturnToGolem(InputAction.CallbackContext context)
    {
        if (!context.performed || lateralMenu.menuOpen)
            return;

        if (currentCharacter == beetleBehaviour)
        {
            SwapCharacter();
            lateralMenu.UISwapCharacter();
        }

        AppendBeetle();
    }

    private void AppendBeetle()
    {
        restingBeetle = true;
        beetleBehaviour.controller.enabled = false;

        beetleBehaviour.transform.SetParent(golemBehaviour.GetComponent<GolemBehaviour>().beetleRestPose);
        beetleBehaviour.transform.localPosition = Vector3.zero;
        beetleBehaviour.transform.localRotation = Quaternion.identity;

        beetleLight.enabled = false;
    }

    public void Checkpoint(Transform newRespawnPoint)
    {
        Debug.Log("Checkpoint");
        respawnPoint = newRespawnPoint;
    }

    public void Die()
    {
        if (currentCharacter == golemBehaviour)
        {
            golemBehaviour.Die(respawnPoint);
        }
        else
        {
            SwapCharacter();
            lateralMenu.UISwapCharacter();
        }

        beetleBehaviour.Die(respawnPoint);
        AppendBeetle();

        GameManager.instance.newDeath();
    }

    public void ApplySelection(int selection)
    {
        if (selection <= 0)
            return;

        if (currentCharacter == golemBehaviour)
            golemBehaviour.GetComponent<GolemBehaviour>().ChangeMaterial((GolemMaterial)selection);
        else
            beetleBehaviour.GetComponent<BeetleBehaviour>().ChangeLumMode((LumMode)selection);

    }
}
