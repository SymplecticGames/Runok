using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
    [SerializeField]
    private float panTime;

    private PlayerManager playerManager;

    [SerializeField]
    private float distanceToTarget = 15.0f;

    private bool firstTime = true;

    [SerializeField]
    private bool onlyOnce = true;

    private bool isPanning;

    private void Start()
    {
        playerManager = GameManager.instance.player;
    }

    public void Pan()
    {
        if (!firstTime && onlyOnce)
            return;

        firstTime = false;

        StartCoroutine(PanCoroutine());
    }

    private IEnumerator PanCoroutine()
    {
        if (!isPanning)
        {
            if (Mathf.Sign(Camera.main.transform.position.y) != Mathf.Sign(transform.position.y))
            {
                playerManager.fadePanelAnim.SetTrigger("doFadeIn");

                yield return new WaitForSeconds(0.1f);

                playerManager.fadePanelAnim.SetTrigger("doFadeOut");
            }

            //playerManager.gameObject.SetActive(false);
            playerManager.input.DeactivateInput();

            playerManager.camLookAtTarget.parent = transform;
            playerManager.camLookAtTarget.localPosition = Vector3.zero;

            // Save the current orbit values
            float radius0 = playerManager.freelookCam.m_Orbits[0].m_Radius;
            float radius1 = playerManager.freelookCam.m_Orbits[1].m_Radius;
            float radius2 = playerManager.freelookCam.m_Orbits[2].m_Radius;
            float height0 = playerManager.freelookCam.m_Orbits[0].m_Height;
            float height1 = playerManager.freelookCam.m_Orbits[1].m_Height;
            float height2 = playerManager.freelookCam.m_Orbits[2].m_Height;

            float convertFactor = distanceToTarget / radius1;

            isPanning = true;

            // Set the orbits to a certain radius and height
            playerManager.freelookCam.m_Orbits[0].m_Radius = radius0 * convertFactor;
            playerManager.freelookCam.m_Orbits[1].m_Radius = radius1 * convertFactor;
            playerManager.freelookCam.m_Orbits[2].m_Radius = radius2 * convertFactor;
            playerManager.freelookCam.m_Orbits[0].m_Height = height0 * convertFactor;
            playerManager.freelookCam.m_Orbits[1].m_Height = height1 * convertFactor;
            playerManager.freelookCam.m_Orbits[2].m_Height = height2 * convertFactor;

            yield return new WaitForSeconds(panTime);

            if (Mathf.Sign(Camera.main.transform.position.y) != Mathf.Sign(playerManager.currentCharacter.transform.position.y))
            {
                playerManager.fadePanelAnim.SetTrigger("doFadeIn");

                yield return new WaitForSeconds(0.1f);

                playerManager.fadePanelAnim.SetTrigger("doFadeOut");
            }

            playerManager.camLookAtTarget.parent = playerManager.currentCharacter.transform;
            playerManager.camLookAtTarget.localPosition = Vector3.zero;

            // Reset the orbits
            playerManager.freelookCam.m_Orbits[0].m_Radius = radius0;
            playerManager.freelookCam.m_Orbits[1].m_Radius = radius1;
            playerManager.freelookCam.m_Orbits[2].m_Radius = radius2;
            playerManager.freelookCam.m_Orbits[0].m_Height = height0;
            playerManager.freelookCam.m_Orbits[1].m_Height = height1;
            playerManager.freelookCam.m_Orbits[2].m_Height = height2;

            //playerManager.gameObject.SetActive(true);
            playerManager.input.ActivateInput();

            if (playerManager.selectionWheelEnabled)
                playerManager.input.actions.FindAction("WheelMenu").Enable();
            else
                playerManager.input.actions.FindAction("WheelMenu").Disable();
        }

        isPanning = false;
    }
}
