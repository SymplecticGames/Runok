using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBehaviour : MonoBehaviour
{
    private CharacterController controller;

    [HideInInspector]
    public bool isActive = true;

    [HideInInspector]
    public bool canRotate = true;

    private float jumpFactor;

    [SerializeField]
    private Vector3 gravity = new Vector3(0.0f, -9.8f, 0.0f);

    [SerializeField]
    private Vector3 jumpSpeed;

    [Header("Movement")]
    [SerializeField]
    private float baseMovementSpeed;

    [SerializeField]
    private float runningSpeedFactor;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerVel = Vector3.zero;

        if (isActive)
            playerVel += Movement();

        playerVel += gravity * (1.0f - jumpFactor) + jumpSpeed * jumpFactor;

        if (jumpFactor > 0.0f)
            jumpFactor -= Time.deltaTime;
        else
            jumpFactor = 0.0f;

        controller.Move(playerVel * Time.deltaTime);

        if (canRotate)
            Rotation();
    }

    private Vector3 Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movementVel = Camera.main.transform.TransformVector(new Vector3(x, 0, z)) * baseMovementSpeed;
        movementVel.y = 0;

        if (Input.GetKey(KeyCode.LeftShift))
            movementVel *= runningSpeedFactor;

        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
            jumpFactor = 1.0f;

        return movementVel;
    }

    private void Rotation()
    {
        // Hacer esto mejor con un slerp

        // Rotation
        Vector3 targetLookAt = this.transform.position + controller.velocity;
        targetLookAt.y = this.transform.position.y;

        this.transform.LookAt(targetLookAt, Vector3.up);
    }
}
