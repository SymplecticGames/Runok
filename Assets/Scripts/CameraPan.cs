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

        yield return new WaitForSeconds(panTime);

        camLookAtTarget.parent = playerManager.currentCharacter.transform;
        camLookAtTarget.localPosition = Vector3.zero;

        playerManager.gameObject.SetActive(true);
    }
}
