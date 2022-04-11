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

        other.transform.SetParent(mobilePlat.platform.transform); // Each time the golem enters the trigger of the platform it will set it as its parent

        if (mobilePlat.platformPreset == PlatformPreset.MoveOnStep && !mobilePlat._movementStarted)
            mobilePlat.StartMovingPlatform();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!enabled)
            return;

        if (!other.CompareTag("Golem"))
            return;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!enabled)
            return;

        if (!other.CompareTag("Golem"))
            return;

        other.transform.parent = null; // Each time the golem exits the trigger of the platform it will set its parent as null
    }
}
