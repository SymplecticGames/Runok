using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBehaviour : MonoBehaviour
{
    [HideInInspector]
    public CharacterController controller;

    [HideInInspector]
    private bool isAttacking = false;

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
            if (CompareTag("Beetle") || movementInput.magnitude <= 0.0f)
                InstantRotation(Camera.main.transform.forward);
            else
                InstantRotation(controller.velocity);
        }
        else if (canRotate && movementInput.magnitude > 0.0f)
            Rotation();

        if (controller.isGrounded && jumpFactor < maxJumpFactor * 0.5f)
            jumps = 0;

        // Animations
        animator.SetBool("isWalking", movementInput.magnitude > 0); // Golem/Beetle

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

        Vector3 movementVel = Camera.main.transform.TransformVector(new Vector3(movementInput.x, 0, movementInput.y)) * baseMovementSpeed * movementFactor;
        movementVel.y = 0;

        if (jumpPressed && jumps < maxJumps)
        {
            jumpPressed = false;
            jumpFactor = maxJumpFactor;
            if (!controller.isGrounded) jumps++;
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
        Vector3 targetLookAt = transform.position + controller.velocity;
        targetLookAt.y = transform.position.y;

        currentForwardTarget = (targetLookAt - transform.position).normalized;
        transform.forward = Vector3.Slerp(transform.forward, currentForwardTarget, rotFactor * Time.deltaTime);
    }

    public void Die(Transform respawnPoint)
    {
        controller.enabled = false;

        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;

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
