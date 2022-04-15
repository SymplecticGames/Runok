using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformPreset
{
    MoveOnTrigger = 0,
    MoveOnAwake = 1,
    MoveOnStep = 3,
}

public class MobilePlatform : MonoBehaviour
{
    public Transform platform;

    [SerializeField]
    private Transform targetPos;

    [SerializeField]
    private float speed;

    public PlatformPreset platformPreset = PlatformPreset.MoveOnTrigger;

    [SerializeField]
    private bool loopMovement;

    [HideInInspector]
    public bool _movementStarted;

    private Vector3 startingPos;
    private Vector3 finalPos;
    private float currentSpeed;
    private float lerpTime;
    private bool firstTime = true;
    private bool isTarget;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = platform.position;
        _movementStarted = false;

        lerpTime = 0.0f;

        if (platformPreset == PlatformPreset.MoveOnAwake)
            StartMovingPlatform();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_movementStarted)
        {
            platform.position = Vector3.Lerp(startingPos, finalPos, lerpTime);

            if (lerpTime > 1.0f)
            {
                if (loopMovement)
                    currentSpeed = -speed;
                else
                    currentSpeed = 0.0f;

                lerpTime = 1.0f;

                if (isTarget)
                {
                    _movementStarted = false;
                    platform.position = finalPos;
                }
            }
            else if (lerpTime < 0.0f)
            {
                if (loopMovement)
                    currentSpeed = speed;
                else
                    currentSpeed = 0.0f;

                lerpTime = 0.0f;
            }
            else
                lerpTime += Time.fixedDeltaTime * currentSpeed;
        }
    }

    public void StartMovingPlatform()
    {
        if (!firstTime)
            return;

        firstTime = false;
        finalPos = targetPos.position;

        StartCoroutine(MovePlatform());
    }

    public void StartMovingTarget(Transform target)
    {
        if (target.position != platform.position && !_movementStarted)
        {
            startingPos = platform.position;
            finalPos = target.position;
            isTarget = true;
            StartCoroutine(MovePlatform());
        } 
    }

    private IEnumerator MovePlatform()
    {
        yield return new WaitForSeconds(1.0f);
        currentSpeed = speed;
        _movementStarted = true;

        lerpTime = 0.0f;
    }

}
