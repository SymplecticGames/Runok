using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gemstone : MonoBehaviour
{
    [SerializeField]
    private float stayGlowDuration = 2.0f;

    [SerializeField]
    private float glowDownDuration = 3.0f;

    [SerializeField]
    private float glowUpDuration = 0.5f;

    [SerializeField]
    private bool neverGlowDown;

    private float lightCoolDown;

    private bool glowingUp;

    private Light pointLight;

    private float maxLightRange;

    private float lightLerpStep;

    // Start is called before the first frame update
    void Start()
    {
        pointLight = GetComponentInChildren<Light>();
        maxLightRange = pointLight.range;

        lightLerpStep = 0.0f;
        lightCoolDown = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        pointLight.range = Mathf.Lerp(0.0f, maxLightRange, lightLerpStep);

        if (glowingUp)
        {
            if (lightLerpStep < 1.0f)
                lightLerpStep += Time.deltaTime / glowUpDuration;
            else
                lightLerpStep = 1.0f;
        }
        else
        {
            if (lightLerpStep > 0.0f)
                lightLerpStep -= Time.deltaTime / glowDownDuration;
            else
                lightLerpStep = 0.0f;
        }

        if (neverGlowDown)
            return;

        // Timer for lightCoolDown
        if (lightCoolDown > 0.0f)
        {
            lightCoolDown -= Time.deltaTime;
        }
        else
        {
            glowingUp = false;

            lightCoolDown = 0.0f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("RadialLight"))
            return;

        lightCoolDown = stayGlowDuration;
        glowingUp = true;
    }
}
