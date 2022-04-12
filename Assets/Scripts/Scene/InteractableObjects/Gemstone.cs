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

    private Material gemMaterial;

    private Color gemEmiColor;

    private AudioSource audio;

    private float _baseVolume;

    // Start is called before the first frame update
    void Start()
    {
        pointLight = GetComponentInChildren<Light>();
        maxLightRange = pointLight.range;

        lightLerpStep = 0.0f;
        lightCoolDown = 0.0f;

        gemMaterial = GetComponent<MeshRenderer>().material;
        gemEmiColor = gemMaterial.GetColor("_EmissionColor");

        audio = GetComponent<AudioSource>();

        _baseVolume = audio.volume;
    }

    // Update is called once per frame
    void Update()
    {
        pointLight.range = Mathf.Lerp(0.0f, maxLightRange, lightLerpStep);
        if (glowingUp)
            gemMaterial.SetColor("_EmissionColor", gemEmiColor * Mathf.Clamp(Mathf.Pow(lightLerpStep, 1.5f), 0.01f, 1.0f));
        else
            gemMaterial.SetColor("_EmissionColor", gemEmiColor * Mathf.Clamp(Mathf.Pow(lightLerpStep, 6), 0.01f, 1.0f));

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

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled)
            return;

        if (!other.CompareTag("RadialLight"))
            return;

        if (lightLerpStep < 0.5f)
        {
            audio.volume = _baseVolume * AudioManager.audioInstance.soundEffectsFactor;
            audio.Play();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!enabled)
            return;

        if (!other.CompareTag("RadialLight"))
            return;

        if (!glowingUp)
        {
            lightCoolDown = stayGlowDuration;
            glowingUp = true;
        }
    }
}
