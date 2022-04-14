using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBehaviour : MonoBehaviour
{
    [HideInInspector]
    public CharacterController controller;

    [HideInInspector]
    public bool isAttacking = false;

    [HideInInspector]
    public bool canMove= true;

    [HideInInspector]
    public bool canRotate = true;

    [HideInInspector]
    public float movementFactor;

    [HideInInspector]
    public float maxJumpFactor = 1.0f;

    [HideInInspector]
    public float jumpFactor;

    [SerializeField]
    private float gravityFactor;

    [HideInInspector]
    public int maxJumps;

    [HideInInspector]
    public int jumps;

    [SerializeField]
    private Vector3 jumpSpeed;

    [SerializeField]
    private float rotFactor = 0.3f;

    [Header("Movement")]
    [SerializeField]
    private float baseMovementSpeed;

    [HideInInspector]
    public Vector2 movementInput;

    [HideInInspector]
    public bool jumpPressed;

    [HideInInspector]
    public Vector3 playerVel;

    private Vector3 additionalVel;

    private Animator animator;

    [HideInInspector]
    public Vector3 currentForwardTarget;

    [HideInInspector]
    public bool isInBoss;

    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        movementFactor = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        playerVel = Vector3.zero;

        playerVel += gravityFactor * GameManager.gravity * (1.0f - jumpFactor) + jumpSpeed * jumpFactor;

        playerVel += additionalVel;

        playerVel += Movement();

        if (jumpFactor > 0.0f)
            jumpFactor -= Time.deltaTime;
        else
            jumpFactor = 0.0f;

        if (controller.enabled)
            controller.Move(playerVel * Time.deltaTime);


        // Rotation
        if (isAttacking)
        {
            if (CompareTag("Beetle") && !isInBoss)
                InstantRotation(Camera.main.transform.forward);
            // NOTE: Erased by Dani to avoid weird rotations when golem hits
            /*
            else
                InstantRotation(controller.velocity);
                */
        }

        if (canRotate && movementInput.magnitude > 0.0f)
            Rotation();

        if (controller.isGrounded && jumpFactor < maxJumpFactor * 0.5f)
            jumps = 0;

        // Animations
        animator.SetBool("isWalking", canMove && movementInput.magnitude > 0); // Golem/Beetle

        float animationSpeed = Movement().magnitude / 8;

        // Golem walkspeed, jump and falling
        if (CompareTag("Golem"))
        {
            if (!isAttacking && controller.isGrounded)
                animator.SetFloat("WalkSpeed", Mathf.Clamp(animationSpeed, 0.1f, animationSpeed));
            else
                animator.SetFloat("WalkSpeed", 1.0f);

            if (jumpPressed && !isAttacking)
                animator.SetBool("isJumping", true);

            if (!controller.isGrounded && jumpFactor < maxJumpFactor * 0.5f)
                animator.SetBool("isJumping", false);

            animator.SetBool("isFalling", !controller.isGrounded);
        }
    }

    public void SetAdditionalVel(Vector3 additionalVelocity)
    {
        additionalVel = additionalVelocity;
    }

    public Vector3 Movement()
    {
        if (isAttacking && CompareTag("Golem"))
            return Vector3.zero;

        if (!canMove)
            return Vector3.zero;

        Vector3 camSpaceMovement = Camera.main.transform.TransformVector(new Vector3(movementInput.x, 0, movementInput.y));
        camSpaceMovement.y = 0;
        camSpaceMovement = camSpaceMovement.normalized * movementInput.magnitude;
        Vector3 movementVel = camSpaceMovement * baseMovementSpeed * movementFactor;

        if (jumpPressed && jumps < maxJumps)
        {
            jumpPressed = false;
            jumpFactor = maxJumpFactor;
            if (!controller.isGrounded) jumps++;
            AudioManager.audioInstance.PlayCharSound(CharAudioTag.genericJump);
        }

        // Allow some movement (0.4) while shooting a ray
        if (TryGetComponent(out BeetleBehaviour beetle) && (beetle.shootingFrontRay || beetle.shootingBackRay))
            return movementVel * 0.4f;

        return movementVel;
    }


    private void InstantRotation(Vector3 target)
    {
        // Rotation
        Vector3 targetLookAt = transform.position + target;
        targetLookAt.y = transform.position.y;

        currentForwardTarget = (targetLookAt - transform.position).normalized;
        transform.forward = Vector3.Slerp(transform.forward, currentForwardTarget, 3.0f * rotFactor * Time.deltaTime);
    }


    private void Rotation()
    {
        // Rotation
        Vector3 camSpaceMovement = Camera.main.transform.TransformVector(new Vector3(movementInput.x, 0, movementInput.y));
        camSpaceMovement.y = 0;
        camSpaceMovement.Normalize();
        Vector3 targetLookAt = transform.position + camSpaceMovement;
        targetLookAt.y = transform.position.y;

        currentForwardTarget = (targetLookAt - transform.position).normalized;
        transform.forward = Vector3.Slerp(transform.forward, currentForwardTarget, .05f + rotFactor * Time.deltaTime);
    }

    public void Die(Transform respawnPoint)
    {
        controller.enabled = false;

        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;
        transform.parent = null;

        controller.enabled = true;
    }

    private void EnableMovement()
    {
        isAttacking = false;
    }

    private void DisableMovement()
    {
        isAttacking = true;
    }
}
