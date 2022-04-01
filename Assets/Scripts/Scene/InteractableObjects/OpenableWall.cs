using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class OpenableWall : MonoBehaviour
{
    [SerializeField]
    private Transform wall;

    [SerializeField]
    private Transform targetPos;

    private Vector3 initPos;

    private bool isOpening = false;

    private bool isClosing = false;

    private float lerpStep = 0.0f;

    [SerializeField]
    private float openingVelocity;

    [SerializeField]
    private bool staysOpen = true;

    [SerializeField]
    private float delay = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        initPos = wall.position;
    }

    private void Update()
    {
        if (isOpening)
        {
            if (lerpStep <= 1.0f)
            {
                wall.position = Vector3.Lerp(initPos, targetPos.position, lerpStep);
                lerpStep += Time.deltaTime * openingVelocity;
            }
            else
            {
                lerpStep = 1.0f;
                wall.position = targetPos.position;
                isOpening = false;
            }
        }
        else if (isClosing)
        {
            if (lerpStep >= 0.0f)
            {
                wall.position = Vector3.Lerp(initPos, targetPos.position, lerpStep);
                lerpStep -= Time.deltaTime * openingVelocity;
            }
            else
            {
                lerpStep = 0.0f;
                wall.position = initPos;
                isClosing = false;
            }
        }
    }

    public void OpenWall()
    {
        StartCoroutine(DelayedOpenWall(delay));
    }

    private IEnumerator DelayedOpenWall(float delay)
    {
        yield return new WaitForSeconds(delay);

        isOpening = true;
        isClosing = false;
    }

    public void CloseWall()
    {
        StartCoroutine(DelayedCloseWall(delay));
    }

    private IEnumerator DelayedCloseWall(float delay)
    {
        yield return new WaitForSeconds(delay);

        isClosing = true;
        isOpening = false;
    }
}
