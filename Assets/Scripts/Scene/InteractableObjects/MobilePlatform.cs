using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilePlatform : MonoBehaviour
{
    [SerializeField]
    private Transform platform;

    public Transform targetPos;

    [SerializeField]
    private float speed;

    public bool startMovingWhenOn;

    [HideInInspector]
    public Vector3 startingPos;

    [HideInInspector]
    public float currentSpeed;

    [HideInInspector]
    public bool _movementStarted;

    private bool loopMovement;

    private float lerpTime;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = platform.position;
        _movementStarted = false;

        lerpTime = 0.0f;
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

    public void StartMovingPlatform(bool loop)
    {
        loopMovement = loop;
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
