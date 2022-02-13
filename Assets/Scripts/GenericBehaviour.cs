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

    [SerializeField]
    private Vector3 gravity = new Vector3(0.0f, -9.8f, 0.0f);

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
        playerVel += gravity;

        if (isActive)
            playerVel += Movement();

        controller.Move(playerVel * Time.deltaTime);

        if (canRotate)
            Rotation(playerVel);
    }

    private Vector3 Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movementVel = Camera.main.transform.localToWorldMatrix * new Vector3(x, 0, z) * baseMovementSpeed;
        movementVel.y = 0;

        if (Input.GetKey("left shift"))
            movementVel *= runningSpeedFactor;

        return movementVel;
    }

    private void Rotation(Vector3 lookAtDir)
    {
        // Rotation
        Vector3 targetLookAt = this.transform.position + controller.velocity;
        targetLookAt.y = this.transform.position.y;

        this.transform.LookAt(targetLookAt, Vector3.up);
    }
}
