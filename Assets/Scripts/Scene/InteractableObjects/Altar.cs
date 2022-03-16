using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Altar : MonoBehaviour
{
    
    /////////////////////////////////////////  p u b l i c   v a r i a b l e s  ////////////////////////////////////////
    // action to do the Camera Pan
    [SerializeField]
    private UnityEvent action;
    
    ////////////////////////////////////////  p r i v a t e   v a r i a b l e s  ///////////////////////////////////////
    // GO reference to the altar's portal
    private GameObject _altarPortal;
    
    //////////////////////////////////////////////////  p r o g r a m  /////////////////////////////////////////////////
    /// 
    // Start is called before the first frame update
    void Start()
    {
        // get altar portal GOs
        _altarPortal = gameObject.transform.GetChild(0).gameObject;
        disableAltar();
    }

    public void enableAltar()
    {
        _altarPortal.GetComponent<Collider>().isTrigger = true;
        _altarPortal.GetComponent<MeshRenderer>().material.color = new Color(0.0f, 1.0f, 0.0f, 0.5f);
        action.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        _altarPortal.GetComponent<MeshRenderer>().material.color = new Color(0.0f, 1.0f, 1.0f, 0.5f);
    }

    private void OnTriggerExit(Collider other)
    {
        _altarPortal.GetComponent<MeshRenderer>().material.color = new Color(0.0f, 1.0f, 0.0f, 0.5f);
    }

    public void disableAltar()
    {
        _altarPortal.GetComponent<Collider>().isTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
