using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pushable : MonoBehaviour
{

    [Header("Pushable Properties")]
    [SerializeField] string applierTag;
    [SerializeField] float forceToApply;
    [SerializeField] Transform cubeRespawn;

    private Rigidbody rb;
    private MeshRenderer rend;

    // Angles for constraint direction
    float forwardAngle;
    float leftAngle;
    float backwardAngle;
    float rightAngle;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<MeshRenderer>();

        ResetPushable();
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

    public void ResetPushable()
    {
        rb.useGravity = false;
        rend.enabled = false;
        transform.position = cubeRespawn.position;
    }

    public void SpawnPushable()
    {
        rb.useGravity = true;
        rend.enabled = true;
        transform.position = cubeRespawn.position;
    }
}
