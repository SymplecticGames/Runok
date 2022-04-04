using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilePlatformTrigger : MonoBehaviour
{
    [SerializeField]
    private MobilePlatform mobilePlat;

    private void Awake()
    {
        if (!mobilePlat)
            enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled)
            return;

        if (!other.CompareTag("Golem"))
            return;

        if (mobilePlat.platformPreset == PlatformPreset.MoveOnStep && !mobilePlat._movementStarted)
            mobilePlat.StartMovingPlatform();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!enabled)
            return;

        if (!other.CompareTag("Golem"))
            return;

        Vector3 velVector = mobilePlat.currentSpeed * (mobilePlat.targetPos.position - mobilePlat.startingPos);

        other.GetComponent<GenericBehaviour>().SetAdditionalVel(velVector);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!enabled)
            return;

        if (!other.CompareTag("Golem"))
            return;

        other.GetComponent<GenericBehaviour>().SetAdditionalVel(Vector3.zero);
    }
}
