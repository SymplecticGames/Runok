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

    private bool isOpening = false;

    private float lerpStep = 0.0f;

    [SerializeField]
    private float openingVelocity;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if (isOpening)
        {
            if (lerpStep <= 1.0f)
            {
                wall.position = Vector3.Lerp(wall.position, targetPos.position, lerpStep);
                lerpStep += Time.deltaTime * openingVelocity;
            }
            else
            {
                wall.position = targetPos.position;
                isOpening = false;
            }
        }
    }

    public void OpenWall()
    {
        StartCoroutine(DelayedOpenWall());
    }

    private IEnumerator DelayedOpenWall()
    {
        yield return new WaitForSeconds(1.0f);

        isOpening = true;
        lerpStep = 0.0f;
    }
}
