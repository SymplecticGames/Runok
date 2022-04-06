using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MultipleTriggersActivator : MonoBehaviour
{
    [SerializeField]
    private List<ActionTriggerer> triggerers;

    private List<ActionTriggerer> currentActiveTriggerers = new List<ActionTriggerer>();

    [SerializeField]
    private UnityEvent OnActivationAction;

    private bool alreadyInvoked;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddInvokator(ActionTriggerer triggerer)
    {
        if (!triggerers.Contains(triggerer) || currentActiveTriggerers.Contains(triggerer))
            return;

        currentActiveTriggerers.Add(triggerer);

        if (currentActiveTriggerers.Count >= triggerers.Count && !alreadyInvoked)
        {
            OnActivationAction.Invoke();
            alreadyInvoked = true;
        }
    }

    public void SubtractInvokator(ActionTriggerer triggerer)
    {
        if (!triggerers.Contains(triggerer) || !currentActiveTriggerers.Contains(triggerer))
            return;

        currentActiveTriggerers.Remove(triggerer);
    }
}
