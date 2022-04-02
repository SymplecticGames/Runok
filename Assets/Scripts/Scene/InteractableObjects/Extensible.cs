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

    private void Update()
    {
        if (Physics.Raycast(transform.parent.position, transform.parent.right, out RaycastHit hitInfo) && hitInfo.collider.CompareTag("GenericBlock"))
        {
            newScale = hitInfo.distance * 0.5f;
            transform.parent.localScale = new Vector3(newScale, restScale.y, restScale.z);
        }
        else
        {
            transform.parent.localScale = restScale;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled)
            return;

        if (other.CompareTag("Golem") || other.CompareTag("Beetle"))
            GameManager.instance.player.Die();
    }
}
