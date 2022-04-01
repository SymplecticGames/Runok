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

    public Transform camLookAtTarget;

    public CinemachineFreeLook freelookCam;

    [HideInInspector]
    public float camOrbitRadius;

    private Animator animator;

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

        // Set beetle on Golem's back
        beetleLight = beetleBehaviour.GetComponentInChildren<Light>();
        AppendBeetle();
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

    public void OnGolemJump(InputAction.CallbackContext context)
    {
        if (currentCharacter.TryGetComponent(out GolemBehaviour golem))
        {
            if (golem.currentMaterial == GolemMaterial.Plumber)
                return;

            currentCharacter.jumpPressed = context.performed;
        }
    }

    // Golem Hit
    public void OnGolemHit(InputAction.CallbackContext context)
    {
        if (currentCharacter.TryGetComponent(out GolemBehaviour golem))
        {
            if (context.started)
            {
                golem.GolemHit();
            }
        }
    }

    // Beetle Shoot
    public void OnBeetleShoot(InputAction.CallbackContext context)
    {
        if (currentCharacter.TryGetComponent(out BeetleBehaviour beetle))
        {
            if (beetle.currentLumMode != LumMode.LightShot)
                return;

            beetle.shootPressed = context.performed;
        }
    }

    // Beetle Ray from the front
    public void OnBeetleFrontRay(InputAction.CallbackContext context)
    {
        if (currentCharacter.TryGetComponent(out BeetleBehaviour beetle))
        {
            if (beetle.currentLumMode != LumMode.LightImpulse)
                return;

            beetle.frontRayPressed = context.performed;
        }
    }

    // Beetle Ray from the back
    public void OnBeetleBackRay(InputAction.CallbackContext context)
    {
        if (currentCharacter.TryGetComponent(out BeetleBehaviour beetle))
        {
            if (beetle.currentLumMode != LumMode.LightImpulse)
                return;

            beetle.backRayPressed = context.performed;
        }
    }

    public void OnSwapCharacter(InputAction.CallbackContext context)
    {
        if (!context.performed || lateralMenu.MenuOpened())
            return;

        SwapCharacter();
    }

    public void SwapCharacter()
    {
        currentCharacter.movementInput = Vector2.zero;

        // Reset animator before swapping
        animator.SetBool("isWalking", false);

        if (currentCharacter.CompareTag("Golem"))
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }

        if (currentCharacter.CompareTag("Beetle"))
        {
            animator.SetBool("FrontRay", false);
            animator.SetBool("BackRay", false);
        }

        if (currentCharacter == golemBehaviour)
        {
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
            animator = currentCharacter.GetComponent<Animator>();
        }
    }

    public void ReturnToGolem(InputAction.CallbackContext context)
    {
        if (!context.performed || lateralMenu.MenuOpened())
            return;

        if (currentCharacter == beetleBehaviour)
        {
            animator.SetBool("isActive", false);  // Turn off beetle animator
            SwapCharacter();
            lateralMenu.UISwapCharacter();
        }
        else
            beetleBehaviour.GetComponent<Animator>().SetBool("isActive", false);  // Turn off beetle animator

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

        beetleBehaviour.GetComponent<Animator>().SetBool("isActive", false);

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

    public int GetSelection()
    {
        if (currentCharacter == golemBehaviour)
            return (int) golemBehaviour.GetComponent<GolemBehaviour>().currentMaterial;
        else
            return (int) beetleBehaviour.GetComponent<BeetleBehaviour>().currentLumMode;
    }
}
