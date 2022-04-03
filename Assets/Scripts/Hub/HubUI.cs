using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubUI : MonoBehaviour
{
    public GameObject enterLevel;
    public GameObject exitLevel;
    
    // Start is called before the first frame update
    void Start()
    {
        enterLevel.SetActive(false);
        exitLevel.SetActive(false);
        StartCoroutine(WaitTransitionToEnd());
    }

    private IEnumerator WaitTransitionToEnd()
    {
        yield return new WaitForSeconds(1.0f);
        enterLevel.SetActive(true);
        exitLevel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
