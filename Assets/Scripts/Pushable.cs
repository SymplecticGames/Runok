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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(applierTag))
        {
            rb.AddForce(other.transform.forward * forceToApply, ForceMode.Impulse);
        }
    }
}
