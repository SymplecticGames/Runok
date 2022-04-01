using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Altar : MonoBehaviour
{
    // GO reference to the altar's portal
    [SerializeField]
    private GameObject _altarPortal;

    // action to do the Camera Pan
    [SerializeField]
    private UnityEvent action;

    private Color defaultColor;

    private Material mat;

    //////////////////////////////////////////////////  p r o g r a m  /////////////////////////////////////////////////
    /// 
    // Start is called before the first frame update
    void Start()
    {
        mat = _altarPortal.GetComponent<MeshRenderer>().material;

        // get altar portal GOs
        disableAltar();

        defaultColor = mat.color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void enableAltar()
    {
        _altarPortal.GetComponent<Collider>().isTrigger = true;
        defaultColor = new Color(0.0f, 1.0f, 0.0f, 0.5f);
        mat.color = defaultColor;
        action.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Golem") && !other.CompareTag("Beetle"))
            return;

        mat.color = new Color(0.0f, 1.0f, 1.0f, 0.5f);

        // End Level
    }

    private void OnTriggerExit(Collider other)
    {
        mat.color = defaultColor;
    }

    public void disableAltar()
    {
        _altarPortal.GetComponent<Collider>().isTrigger = false;
    }
}
