using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GenericBehaviour golemBehaviour;

    [SerializeField]
    private GenericBehaviour beetleBehaviour;

    [SerializeField]
    private Transform camLookAtTarget;

    // Start is called before the first frame update
    void Start()
    {
        // Activate golem
        golemBehaviour.isActive = true;
        beetleBehaviour.isActive = false;

        camLookAtTarget.parent = golemBehaviour.transform;
        camLookAtTarget.localPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            SwapCharacter();
    }

    private void SwapCharacter()
    {
        if (golemBehaviour.isActive)
        {
            // Activate beetle
            golemBehaviour.isActive = false;
            beetleBehaviour.isActive = true;

            camLookAtTarget.parent = beetleBehaviour.transform;
            camLookAtTarget.localPosition = Vector3.zero;
        }
        else
        {
            // Activate golem
            golemBehaviour.isActive = true;
            beetleBehaviour.isActive = false;

            camLookAtTarget.parent = golemBehaviour.transform;
            camLookAtTarget.localPosition = Vector3.zero;
        }
    }
}
