using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Altar : MonoBehaviour
{
    public Color disabledColor;
    public Color enabledColor;
    public Color enteredColor;

    // GO reference to the altar's portal
    [SerializeField]
    private GameObject _altarPortal;

    [SerializeField]
    private GameObject _altarParchment;
    
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
        mat.color = disabledColor;
        _altarParchment.SetActive(false);
        
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
        _altarParchment.SetActive(true);
        defaultColor = enabledColor;
        mat.color = defaultColor;
        action.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled)
            return;

        if (!other.CompareTag("Golem") && !other.CompareTag("Beetle"))
            return;

        mat.color = enteredColor;
        
        // End Level
        StartCoroutine(EndOfLevelTransition());


    }

    private void OnTriggerExit(Collider other)
    {
        if (!enabled)
            return;

        mat.color = defaultColor;
    }

    public void disableAltar()
    {
        _altarPortal.GetComponent<Collider>().isTrigger = false;
    }
    
    
    public IEnumerator EndOfLevelTransition()
    {

        yield return new WaitForSeconds(1.0f);
        
        // viñet animation
        
        // go to hub 1sec later
        SceneManager.LoadScene(2);
    }
    
}
