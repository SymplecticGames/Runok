using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GenericBehaviour golem;

    [SerializeField]
    private GenericBehaviour beetle;

    [SerializeField]
    private Cinemachine.CinemachineFreeLook cam;

    [SerializeField]
    private Transform camLookAt;

    // Start is called before the first frame update
    void Start()
    {
        // Activate golem
        golem.isActive = true;
        beetle.isActive = false;

        camLookAt.parent = golem.transform;
        camLookAt.localPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            SwapCharacter();
    }

    private void SwapCharacter()
    {
        if (golem.isActive)
        {
            // Activate beetle
            golem.isActive = false;
            beetle.isActive = true;

            camLookAt.parent = beetle.transform;
            camLookAt.localPosition = Vector3.zero;
        }
        else
        {
            // Activate golem
            golem.isActive = true;
            beetle.isActive = false;

            camLookAt.parent = golem.transform;
            camLookAt.localPosition = Vector3.zero;
        }
    }
}
