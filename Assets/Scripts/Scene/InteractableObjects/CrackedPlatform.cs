using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedPlatform : MonoBehaviour
{
    [SerializeField]
    private Material crackedMaterial;

    [SerializeField]
    private float crackedTime = 0.3f;

    [SerializeField]
    private float brokenTime = 1.0f;

    private float currentCheckPoint = 0.0f;

    private float crackedStep = 0.0f;

    private MeshRenderer currentMaterial;

    private void Start()
    {
        currentMaterial = GetComponent<MeshRenderer>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!enabled)
            return;

        if (other.TryGetComponent(out GolemBehaviour golem))
        {
            if (golem.currentMaterial != GolemMaterial.Plumber)
                return;

            crackedStep += Time.deltaTime;

            // Cracked time
            if (crackedStep > crackedTime && currentCheckPoint < crackedTime)
            {
                currentCheckPoint = crackedTime;
                currentMaterial.material = crackedMaterial;
            }

            // Broken time
            if (crackedStep > brokenTime && currentCheckPoint < brokenTime)
            {
                currentCheckPoint = brokenTime;

                // Break
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!enabled)
            return;

        if (other.TryGetComponent(out GolemBehaviour golem))
        {
            if (golem.currentMaterial != GolemMaterial.Plumber)
                return;

            crackedStep = currentCheckPoint;
        }
    }
}
