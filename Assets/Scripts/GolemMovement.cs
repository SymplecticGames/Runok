using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemMovement : MonoBehaviour
{
    private CharacterController controller;

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
        Movement();
    }

    private void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 localMovementVector = new Vector3(x, 0, z);
        Vector3 finalMovementVector = Camera.main.transform.localToWorldMatrix * localMovementVector * baseMovementSpeed;
        finalMovementVector.y = 0;

        if (Input.GetKey("left shift"))
            finalMovementVector *= runningSpeedFactor;

        controller.SimpleMove(finalMovementVector);

        // Rotation
        Vector3 targetLookAt = this.transform.position + finalMovementVector;

        Vector3 lookAtPos = new Vector3(targetLookAt.x, this.transform.position.y, targetLookAt.z);
        this.transform.LookAt(lookAtPos, Vector3.up);
    }
}
