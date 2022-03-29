using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBehaviour : MonoBehaviour
{
    [HideInInspector]
    public CharacterController controller;

    [HideInInspector]
    public bool canRotate = true;

    [HideInInspector]
    public float movementFactor;

    [HideInInspector]
    public float maxJumpFactor = 1.0f;

    [HideInInspector]
    public float jumpFactor;

    [HideInInspector]
    public int maxJumps;

    private int jumps;

    [SerializeField]
    private Vector3 gravity = new Vector3(0.0f, -9.8f, 0.0f);

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

    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        playerVel = Vector3.zero;

        playerVel += gravity * (1.0f - jumpFactor) + jumpSpeed * jumpFactor;

        playerVel += additionalVel;

        playerVel += Movement();

        if (jumpFactor > 0.0f)
            jumpFactor -= Time.deltaTime;
        else
            jumpFactor = 0.0f;

        if (controller.enabled)
            controller.Move(playerVel * Time.deltaTime);

        if (canRotate && movementInput.magnitude > 0.0f)
            Rotation();

        if (controller.isGrounded && jumpFactor < maxJumpFactor * 0.5f)
            jumps = 0;     
    }

    public void SetAdditionalVel(Vector3 additionalVelocity)
    {
        additionalVel = additionalVelocity;
    }

    public Vector3 Movement()
    {
        Vector3 movementVel = Camera.main.transform.TransformVector(new Vector3(movementInput.x, 0, movementInput.y)) * baseMovementSpeed * movementFactor;
        movementVel.y = 0;

        if (jumpPressed && jumps < maxJumps)
        {
            jumpPressed = false;
            jumpFactor = maxJumpFactor;
            jumps++;
        }
            
        return movementVel;
    }

    private void Rotation()
    {
        // Hacer esto mejor con un slerp

        // Rotation
        Vector3 targetLookAt = this.transform.position + controller.velocity;
        targetLookAt.y = this.transform.position.y;

        Vector3 forwardVec = targetLookAt - this.transform.position;
        this.transform.forward = Vector3.Slerp(this.transform.forward, forwardVec, rotFactor * Time.deltaTime);
    }

    public void Die(Transform respawnPoint)
    {
        controller.enabled = false;

        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;

        controller.enabled = true;
    }
    
    private void OnControllerColliderHit(ControllerColliderHit other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            GameManager.instance.player.Die();
        }
    }
    
}
