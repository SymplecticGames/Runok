using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerCameraControl : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer eyes;

    private float lerpStep;

    private bool isLerping = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isLerping)
        {
            lerpStep += Time.deltaTime;
            eyes.materials[1].SetFloat("_ShadowWidth", Mathf.Lerp(3.0f, 0.5f, lerpStep));

            if (lerpStep > 1.0f)
            {
                eyes.materials[1].SetFloat("_ShadowWidth", 0.5f);
                isLerping = false;
            }
        }
    }

    private void DisableObject()
    {
        eyes.materials[1].SetFloat("_ShadowWidth", 3.0f);
    }

    private void EnableObject()
    {
        eyes.materials[1].SetFloat("_ShadowWidth", 3.0f);
        isLerping = true;
        lerpStep = 0.0f;
    }
}
