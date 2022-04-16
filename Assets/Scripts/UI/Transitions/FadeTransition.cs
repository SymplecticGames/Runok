using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTransition : MonoBehaviour
{
    private Animator fadePanelAnim;

    // Start is called before the first frame update
    void Start()
    {
        fadePanelAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ForceTransition()
    {
        fadePanelAnim.SetTrigger("forceTransition");
        fadePanelAnim.ResetTrigger("doFadeIn");
        fadePanelAnim.ResetTrigger("doFadeOut");
    }
}
