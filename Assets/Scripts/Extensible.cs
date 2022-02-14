using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extensible : MonoBehaviour
{
    [Header("Extensible Properties")]
    [SerializeField] string colliderTag;

    private Vector3 restScale;
    private float newScale;

    // Start is called before the first frame update
    void Start()
    {
        restScale = transform.parent.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(colliderTag))
        {
            newScale = Vector3.Distance(transform.parent.position, other.transform.position);
            transform.parent.localScale = new Vector3(newScale, restScale.y, restScale.z);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(colliderTag))
        {
            transform.parent.localScale = restScale;
        }
    }
}
