using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterSoundManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Golem") || other.CompareTag("Beetle"))
        {
            AudioManager.audioInstance.PlayLaserBeamSound(GetComponent<AudioSource>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Golem") || other.CompareTag("Beetle"))
        {
            AudioManager.audioInstance.StopLaserBeamSound(GetComponent<AudioSource>());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
