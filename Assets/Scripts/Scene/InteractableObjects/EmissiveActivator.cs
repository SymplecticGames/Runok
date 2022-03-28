using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissiveActivator : MonoBehaviour
{
    [SerializeField]
    private float stayGlowDuration = 0.0f;

    [SerializeField]
    private float glowDownDuration = 1.0f;

    [SerializeField]
    private float glowUpDuration = 0.5f;

    [SerializeField]
    private bool neverGlowDown;

    private float emiCoolDown;

    private bool glowingUp;

    private float emiLerpStep;

    private Material emissiveMaterial;

    private Color emissiveColor;

    //////////////////////////////////////////////////  p r o g r a m  /////////////////////////////////////////////////
    /// 
    // Start is called before the first frame update
    void Start()
    {
        emiLerpStep = 0.0f;
        emiCoolDown = 0.0f;

        emissiveMaterial = GetComponentInChildren<MeshRenderer>().material;
        emissiveColor = emissiveMaterial.GetColor("_EmissionColor");
    }

    // Update is called once per frame
    void Update()
    {
        emissiveMaterial.SetColor("_EmissionColor", emissiveColor * Mathf.Pow(emiLerpStep, 2));

        if (glowingUp)
        {
            if (emiLerpStep < 1.0f)
                emiLerpStep += Time.deltaTime / glowUpDuration;
            else
                emiLerpStep = 1.0f;
        }
        else
        {
            if (emiLerpStep > 0.0f)
                emiLerpStep -= Time.deltaTime / glowDownDuration;
            else
                emiLerpStep = 0.0f;
        }

        if (neverGlowDown)
            return;

        // Timer for lightCoolDown
        if (emiCoolDown > 0.0f)
        {
            emiCoolDown -= Time.deltaTime;
        }
        else
        {
            glowingUp = false;

            emiCoolDown = 0.0f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("RadialLight"))
            return;

        emiCoolDown = stayGlowDuration;
        glowingUp = true;
    }
}
