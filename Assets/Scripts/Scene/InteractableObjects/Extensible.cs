using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extensible : MonoBehaviour
{
    private Vector3 restScale;
    private float newScale;

    // Start is called before the first frame update
    void Start()
    {
        restScale = transform.parent.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Golem") || other.CompareTag("Beetle"))
        {
            GameManager.instance.player.Die();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("GenericBlock") && Physics.Raycast(transform.parent.position, transform.parent.right, out RaycastHit hitInfo))
        {
            newScale = hitInfo.distance * 0.5f;
            transform.parent.localScale = new Vector3(newScale, restScale.y, restScale.z);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GenericBlock"))
        {
            transform.parent.localScale = restScale;
        }
    }
}
