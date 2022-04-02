using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    [SerializeField]
    private SphereCollider rangeOfAction;

    [SerializeField]
    private List<ActionTriggerer> dianas;

    private Quaternion initialRotation;

    private Quaternion targetRotation;

    private bool isRotating = false;

    private float slerpStep;

    GenericBehaviour golem;

    // Start is called before the first frame update
    void Start()
    {
        golem = GameManager.instance.player.golemBehaviour;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotating)
        {
            if (slerpStep < 1.0f)
            {
                slerpStep += Time.deltaTime;

                transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, slerpStep);
            }
            else
            {
                transform.rotation = targetRotation;
                isRotating = false;

                foreach (ActionTriggerer diana in dianas)
                    diana.enabled = true;

                // If Player is on platform set him free
                if (rangeOfAction.bounds.Contains(golem.transform.position) && golem.controller.isGrounded)
                {
                    golem.transform.SetParent(null);
                    golem.controller.enabled = true;
                }
            }
        }
    }

    public void StartCounterClockwiseRotation()
    {
        initialRotation = transform.rotation;
        targetRotation = Quaternion.Euler(transform.rotation.eulerAngles - new Vector3(0.0f, 90.0f, 0.0f));

        isRotating = true;
        slerpStep = 0.0f;

        foreach (ActionTriggerer diana in dianas)
            diana.enabled = false;

        // If Player is on platform keep him on
        if (rangeOfAction.bounds.Contains(golem.transform.position) && golem.controller.isGrounded)
        {
            golem.transform.SetParent(transform);
            golem.controller.enabled = false;
        }
    }

    public void StartClockwiseRotation()
    {
        initialRotation = transform.rotation;
        targetRotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0.0f, 90.0f, 0.0f));

        isRotating = true;
        slerpStep = 0.0f;

        foreach (ActionTriggerer diana in dianas)
            diana.enabled = false;

        // If Player is on platform keep him on
        if (rangeOfAction.bounds.Contains(golem.transform.position) && golem.controller.isGrounded)
        {
            golem.transform.SetParent(transform);
            golem.controller.enabled = false;
        }
    }
}
