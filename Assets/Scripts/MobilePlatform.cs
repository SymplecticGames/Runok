using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilePlatform : MonoBehaviour
{
    private Transform _startingPos;

    [SerializeField]
    private Transform _finalPos;

    [SerializeField]
    private float speed;

    private float currentSpeed;

    private bool _movementStarted;

    private bool loopMovement;

    private float lerpTime;

    // Start is called before the first frame update
    void Start()
    {
        _startingPos = this.transform.parent;
        _movementStarted = false;

        lerpTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_movementStarted)
        {
            transform.position = Vector3.Lerp(_startingPos.position, _finalPos.position, lerpTime);

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

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Golem"))
            return;

        Vector3 velVector = currentSpeed * (_finalPos.position - _startingPos.position);

        other.GetComponent<GenericBehaviour>().SetAdditionalVel(velVector);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Golem"))
            return;

        other.GetComponent<GenericBehaviour>().SetAdditionalVel(Vector3.zero);
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
