using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeetleSight : MonoBehaviour
{
    public Image sight;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.localPosition = Camera.main.transform.forward;
        transform.localRotation = transform.rotation;
    }
}
