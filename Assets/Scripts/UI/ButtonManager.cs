using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{
    private Animator _anim;
    
    public void OnHoverEnter()
    {
        _anim.SetTrigger("Select");
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnHoverExit()
    {
        _anim.SetTrigger("Normal");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
