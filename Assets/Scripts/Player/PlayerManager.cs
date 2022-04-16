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

    private Animator animator;

    [HideInInspector]
    public bool restingBeetle;

    public bool selectionWheelEnabled;
    [HideInInspector] public PlayerInput input;

    private GameObject selectionWheel;

    [HideInInspector]
    public Animator fadePanelAnim;

    private void Awake()
    {
        selectionWheel = GameObject.FindGameObjectWithTag("SelectionWheel");
        input = GetComponent<PlayerInput>();

        if (selectionWheelEnabled)
        {
            input.actions.FindAction("WheelMenu").Enable();
            selectionWheel.SetActive(true);
        }
        else
        {
            input.actions.FindAction("WheelMenu").Disable();
            selectionWheel.SetActive(false);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        fadePanelAnim = lateralMenu.transform.Find("FadePanel").GetComponent<Animator>();

        // Activate golem
        currentCharacter = golemBehaviour;

        camLookAtTarget.parent = currentCharacter.transform;
        camLookAtTarget.localPosition = Vector3.zero;

        // Get golem animator
        animator = currentCharacter.GetComponent<Animator>();

        // Set beetle on Golem's back
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

            // Get beetle animator
            animator = currentCharacter.GetComponent<Animator>();
            animator.SetBool("isActive", true);  // Turn on beetle animator
        }
        else
        {
            // Activate golem
            currentCharacter = golemBehaviour;

            // Get golem animator
            animator = currentCharacter.GetComponent<Animator>();
        }

        if (Mathf.Sign(Camera.main.transform.position.y) != Mathf.Sign(currentCharacter.transform.position.y))
            StartCoroutine(TransitionWithFade());
        else
        {
            camLookAtTarget.parent = currentCharacter.transform;
            camLookAtTarget.localPosition = Vector3.zero;
        }
    }

    public IEnumerator TransitionWithFade()
    {
        fadePanelAnim.SetTrigger("doFadeIn");

        yield return new WaitForSeconds(0.1f);

        fadePanelAnim.SetTrigger("doFadeOut");

        camLookAtTarget.parent = currentCharacter.transform;
        camLookAtTarget.localPosition = Vector3.zero;
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
        beetleBehaviour.GetComponent<BeetleBehaviour>().DeactivateBackRay();
        beetleBehaviour.GetComponent<BeetleBehaviour>().DeactivateFrontRay();
        beetleBehaviour.isAttacking = false;
        restingBeetle = true;
        beetleBehaviour.controller.enabled = false;

        beetleBehaviour.transform.SetParent(golemBehaviour.GetComponent<GolemBehaviour>().beetleRestPose);
        beetleBehaviour.transform.localPosition = Vector3.zero;
        beetleBehaviour.transform.localRotation = Quaternion.identity;
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
        {
            golemBehaviour.GetComponent<GolemBehaviour>().ChangeMaterial((GolemMaterial)selection);
        }
        else
        {
            beetleBehaviour.GetComponent<BeetleBehaviour>().ChangeLumMode((LumMode)selection);
        }

    }

    public int GetSelection()
    {
        if (currentCharacter == golemBehaviour)
            return (int) golemBehaviour.GetComponent<GolemBehaviour>().currentMaterial;
        else
            return (int) beetleBehaviour.GetComponent<BeetleBehaviour>().currentLumMode;
    }

    public void PausePlayer()
    {
        input.actions.FindAction("Look").Disable();
        input.actions.FindAction("GolemHit").Disable();
        input.actions.FindAction("GolemJump").Disable();
        input.actions.FindAction("ReturnToGolem").Disable();
        input.actions.FindAction("SwapCharacter").Disable();
        input.actions.FindAction("BeetleFrontRay").Disable();
        input.actions.FindAction("BeetleBackRay").Disable();
        input.actions.FindAction("BeetleShoot").Disable();
        input.actions.FindAction("OpenInstructions").Disable();
        input.actions.FindAction("WheelMenu").Disable();
    }

    public void ResumePlayer()
    {
        input.actions.FindAction("Look").Enable();
        input.actions.FindAction("GolemHit").Enable();
        input.actions.FindAction("GolemJump").Enable();
        input.actions.FindAction("ReturnToGolem").Enable();
        GolemBoss golemBoss = FindObjectOfType<GolemBoss>();
        if (!golemBoss)
            input.actions.FindAction("SwapCharacter").Enable();
        else if (golemBoss.beaten)
            input.actions.FindAction("SwapCharacter").Enable();
        input.actions.FindAction("BeetleFrontRay").Enable();
        input.actions.FindAction("BeetleBackRay").Enable();
        input.actions.FindAction("BeetleShoot").Enable();
        input.actions.FindAction("OpenInstructions").Enable();
        if (selectionWheelEnabled) input.actions.FindAction("WheelMenu").Enable();
    }
}
