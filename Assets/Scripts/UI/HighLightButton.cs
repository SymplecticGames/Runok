using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HighLightButton : MonoBehaviour
{

    ////////////////////////////////////////  p r i v a t e   v a r i a b l e s  ///////////////////////////////////////   
    private Animator _anim;
    
    public void DoChanges()
    {
        _anim.SetTrigger("Selected");
        AudioManager.audioInstance.PlayUISound(UIAudioTag.hover);
        if(EventSystem.current.currentSelectedGameObject != gameObject)
            EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void UndoChanges()
    {
        _anim.SetTrigger("Normal");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
