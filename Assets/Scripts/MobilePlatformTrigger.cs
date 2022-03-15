using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilePlatformTrigger : MonoBehaviour
{
    private MobilePlatform mobilePlat;

    private void Start()
    {
        mobilePlat = GetComponentInParent<MobilePlatform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Golem"))
            return;

        if (mobilePlat.startMovingWhenOn && !mobilePlat._movementStarted)
            mobilePlat.StartMovingPlatform(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Golem"))
            return;

        Vector3 velVector = mobilePlat.currentSpeed * (mobilePlat._finalPos.position - mobilePlat._startingPos.position);

        other.GetComponent<GenericBehaviour>().SetAdditionalVel(velVector);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Golem"))
            return;

        other.GetComponent<GenericBehaviour>().SetAdditionalVel(Vector3.zero);
    }
}
