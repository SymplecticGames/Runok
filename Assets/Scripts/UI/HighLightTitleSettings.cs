using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HighLightTitleSettings : MonoBehaviour
{
    
    private Animator _anim;
    public Image imageToChange;
    
    public void DoChanges()
    {
        _anim.SetTrigger("Select");
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
        _anim = imageToChange.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
