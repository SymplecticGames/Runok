using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionTriggerer : MonoBehaviour
{
    [SerializeField]
    private List<string> triggererTags;

    [SerializeField]
    private UnityEvent action;

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
        if (triggererTags.Contains(other.tag))
        {
            action.Invoke();
        }
    }
}
