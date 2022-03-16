using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
    [SerializeField]
    private float panTime;

    [SerializeField]
    private PlayerManager playerManager;

    [SerializeField]
    private Transform camLookAtTarget;

    [SerializeField]
    private float distanceToTarget = 15.0f;
    
    private bool firstTime = true;

    public void Pan()
    {
        if (firstTime)
        {
            firstTime = false;
            StartCoroutine(PanCoroutine());
        }
    }

    private IEnumerator PanCoroutine()
    {
        playerManager.gameObject.SetActive(false);

        camLookAtTarget.parent = transform;
        camLookAtTarget.localPosition = Vector3.zero;

        // Set the orbits to a certain radius
        playerManager.freelookCam.m_Orbits[0].m_Radius = distanceToTarget - 5.0f;
        playerManager.freelookCam.m_Orbits[1].m_Radius = distanceToTarget;
        playerManager.freelookCam.m_Orbits[2].m_Radius = distanceToTarget + 5.0f;

        yield return new WaitForSeconds(panTime);

        camLookAtTarget.parent = playerManager.currentCharacter.transform;
        camLookAtTarget.localPosition = Vector3.zero;

        // Reset the orbits
        playerManager.freelookCam.m_Orbits[0].m_Radius = playerManager.camOrbitRadius - 5.0f;
        playerManager.freelookCam.m_Orbits[1].m_Radius = playerManager.camOrbitRadius;
        playerManager.freelookCam.m_Orbits[2].m_Radius = playerManager.camOrbitRadius + 5.0f;

        playerManager.gameObject.SetActive(true);
    }
}
