using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class OpenableDoor : MonoBehaviour
{
    private MeshRenderer doorRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        doorRenderer = GetComponent<MeshRenderer>();
    }

    public void OpenDoor()
    {
        StartCoroutine(DelayedOpenDoor());
    }

    private IEnumerator DelayedOpenDoor()
    {
        yield return new WaitForSeconds(1.0f);
        gameObject.SetActive(false);
        GetComponent<Renderer>().enabled = false;
    } 
}
