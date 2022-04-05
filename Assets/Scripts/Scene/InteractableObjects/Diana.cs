using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diana : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!enabled)
            return;
        
        if (GetComponent<ActionTriggerer>().triggererTags.Contains(other.tag) && GetComponent<ActionTriggerer>().isEnabled)
            StartCoroutine(LightDiana());
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<MeshRenderer>().material.DisableKeyword("_EMISSION");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LightDiana()
    {
        GetComponentInChildren<MeshRenderer>().material.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(1.0f);
        GetComponentInChildren<MeshRenderer>().material.DisableKeyword("_EMISSION");
    }
}
