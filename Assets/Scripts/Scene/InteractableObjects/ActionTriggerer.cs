using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionTriggerer : MonoBehaviour
{
    [SerializeField]
    private UnityEvent action;

    [SerializeField]
    private string triggererTag = "Beetle";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggererTag))
        {
            action.Invoke();
        }
    }
}
