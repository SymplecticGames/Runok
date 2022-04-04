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
    [SerializeField]
    private Transform platform;

    public Transform targetPos;

    [SerializeField]
    private float speed;

    public PlatformPreset platformPreset = PlatformPreset.MoveOnTrigger;

    [SerializeField]
    private bool loopMovement;

    [HideInInspector]
    public Vector3 startingPos;

    [HideInInspector]
    public float currentSpeed;

    [HideInInspector]
    public bool _movementStarted;

    private float lerpTime;

    private bool firstTime = true;

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
    void Update()
    {
        if (_movementStarted)
        {
            platform.position = Vector3.Lerp(startingPos, targetPos.position, lerpTime);

            if (lerpTime > 1.0f)
            {
                if (loopMovement)
                    currentSpeed = -speed;
                else
                    currentSpeed = 0.0f;

                lerpTime = 1.0f;
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
                lerpTime += Time.deltaTime * currentSpeed;
        }
    }

    public void StartMovingPlatform()
    {
        if (!firstTime)
            return;

        firstTime = false;

        StartCoroutine(MovePlatform());
    }

    private IEnumerator MovePlatform()
    {
        yield return new WaitForSeconds(1.0f);
        currentSpeed = speed;
        _movementStarted = true;

        lerpTime = 0.0f;
    }
}
