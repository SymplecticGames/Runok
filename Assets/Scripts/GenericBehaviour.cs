using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBehaviour : MonoBehaviour
{
    private CharacterController controller;

    [HideInInspector]
    public bool canRotate = true;

    private float jumpFactor;

    [SerializeField]
    private Vector3 gravity = new Vector3(0.0f, -9.8f, 0.0f);

    [SerializeField]
    private Vector3 jumpSpeed;

    [SerializeField]
    private float rotFactor = 0.3f;

    [Header("Movement")]
    [SerializeField]
    private float baseMovementSpeed;

    [SerializeField]
    private float runningSpeedFactor;

    [HideInInspector]
    public Vector2 movementInput;

    [HideInInspector]
    public bool isRunning;

    [HideInInspector]
    public bool jumpPressed;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerVel = Vector3.zero;

        playerVel += gravity * (1.0f - jumpFactor) + jumpSpeed * jumpFactor;

        playerVel += Movement();

        if (jumpFactor > 0.0f)
            jumpFactor -= Time.deltaTime;
        else
            jumpFactor = 0.0f;

        controller.Move(playerVel * Time.deltaTime);

        if (canRotate)
            Rotation();
    }

    public Vector3 Movement()
    {
        Vector3 movementVel = Camera.main.transform.TransformVector(new Vector3(movementInput.x, 0, movementInput.y)) * baseMovementSpeed;
        movementVel.y = 0;

        if (isRunning)
            movementVel *= runningSpeedFactor;

        if (jumpPressed && controller.isGrounded)
            jumpFactor = 1.0f;

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

        this.transform.position = respawnPoint.position;

        controller.enabled = true;
    }
}
