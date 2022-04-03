using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diana : MonoBehaviour
{
    [SerializeField]
    private ActionTriggerer trigger;
    private void OnTriggerEnter(Collider other)
    {
        if (!enabled)
            return;
        
        if (trigger.triggererTags.Contains(other.tag))
            StartCoroutine(LightDiana());
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LightDiana()
    {
        GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(1.0f);
        GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
    }
}
