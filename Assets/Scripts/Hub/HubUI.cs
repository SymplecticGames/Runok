using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HubUI : MonoBehaviour
{
    private CanvasGroup _cg;
    
    // Start is called before the first frame update
    void Start()
    {
        _cg = GetComponent<CanvasGroup>();
        _cg.alpha = 0;
        StartCoroutine(WaitTransitionToEnd());
    }

    public void HideUI(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        _cg.alpha = 0;

    }

    private IEnumerator WaitTransitionToEnd()
    {
        yield return new WaitForSeconds(1.0f);
        _cg.alpha = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
