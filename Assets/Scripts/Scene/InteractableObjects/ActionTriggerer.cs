using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionTriggerer : MonoBehaviour
{
    [SerializeField]
    private List<string> triggererTags;

    [SerializeField]
    private UnityEvent onHitAction;

    [SerializeField]
    private UnityEvent onReleaseAction;

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
        if (!enabled)
            return;

        if (triggererTags.Contains(other.tag))
            onHitAction.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!enabled)
            return;

        if (triggererTags.Contains(other.tag))
            onReleaseAction.Invoke();
    }
}
