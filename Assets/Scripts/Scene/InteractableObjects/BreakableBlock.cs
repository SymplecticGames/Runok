using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Hitter"))
            return;

        if (other.transform.parent.TryGetComponent(out GolemBehaviour golem) && golem.currentMaterial == GolemMaterial.Plumber)
        {
            // Break
            Destroy(this.gameObject);
        }
        else
        {
            // Play sound of hitting a wall
        }
    }
}
