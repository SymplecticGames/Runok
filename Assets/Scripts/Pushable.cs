using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pushable : MonoBehaviour
{

    [Header("Pushable Properties")]
    [SerializeField] string applierTag;
    [SerializeField] float forceToApply;

    private Rigidbody rb;

    // Angles for constraint direction
    float forwardAngle;
    float leftAngle;
    float backwardAngle;
    float rightAngle;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(applierTag))
        {
            forwardAngle = Vector3.Angle(transform.forward, other.transform.forward);
            leftAngle = Vector3.Angle(transform.forward, -other.transform.right);
            backwardAngle = Vector3.Angle(transform.forward, -other.transform.forward);
            rightAngle = Vector3.Angle(transform.forward, other.transform.right);

            float minAngle = Mathf.Min(forwardAngle, Mathf.Min(leftAngle, Mathf.Min(backwardAngle, rightAngle)));

            Vector3 dir = transform.forward;

            if (minAngle == leftAngle)
                dir = transform.right;
            else if (minAngle == backwardAngle)
                dir = -transform.forward;
            else if (minAngle == rightAngle)
                dir = -transform.right;

            rb.AddForce(dir * forceToApply, ForceMode.Impulse);
        }
    }
}
