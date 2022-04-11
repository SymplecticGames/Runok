using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedPlatform : MonoBehaviour
{
    [SerializeField]
    private Material crackedMaterial;

    [SerializeField]
    private float crackedTime = 0.3f;

    [SerializeField]
    private float brokenTime = 1.0f;

    private float currentCheckPoint = 0.0f;

    private float crackedStep = 0.0f;

    private MeshRenderer meshRend;

    private Material originalMaterial;

    private void Start()
    {
        meshRend = GetComponent<MeshRenderer>();

        originalMaterial = meshRend.material;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!enabled)
            return;

        if (other.TryGetComponent(out GolemBehaviour golem))
        {
            if (golem.currentMaterial != GolemMaterial.Plumber)
                return;

            crackedStep += Time.deltaTime;

            // Cracked time
            if (crackedStep > crackedTime && currentCheckPoint < crackedTime)
            {
                currentCheckPoint = crackedTime;
                meshRend.material = crackedMaterial;
                
                // play cracked audio
                GetComponent<AudioSource>().clip = AudioManager.audioInstance.GetObjSound(ObjaudioTag.crackedPlatform);
                GetComponent<AudioSource>().Play();
            }

            // Broken time
            if (crackedStep > brokenTime && currentCheckPoint < brokenTime)
            {
                currentCheckPoint = brokenTime;

                // play broken audio
                Debug.Log("ROTO");
                AudioSource aS = GetComponent<AudioSource>();
                aS.clip = AudioManager.audioInstance.GetObjSound(ObjaudioTag.brokenPlatform);
                aS.Play();
                StartCoroutine(BreakDelay(aS.clip.length));
                
                // hide platform
                foreach (Renderer visualPart in GetComponentsInChildren<Renderer>())
                    visualPart.enabled = false;

                // disable collider
                gameObject.GetComponent<Collider>().enabled = false;
                
                //Destroy(gameObject);
            }
        }
    }

    IEnumerator BreakDelay(float delay)
    {
        // Break
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!enabled)
            return;

        if (other.TryGetComponent(out GolemBehaviour golem))
        {
            if (golem.currentMaterial != GolemMaterial.Plumber)
                return;

            crackedStep = currentCheckPoint;
        }
    }

    public void ResetPlatform()
    {
        crackedStep = 0.0f;
        currentCheckPoint = 0.0f;
        meshRend.material = originalMaterial;

        gameObject.SetActive(true);
        
        // show platform
        foreach (Renderer visualPart in GetComponentsInChildren<Renderer>())
            visualPart.enabled = true;

        // enable collider
        gameObject.GetComponent<Collider>().enabled = true;
        
    }
}
