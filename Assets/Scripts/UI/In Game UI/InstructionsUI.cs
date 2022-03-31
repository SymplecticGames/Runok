using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsUI : MonoBehaviour
{
    
    /////////////////////////////////////////  p u b l i c   v a r i a b l e s  ////////////////////////////////////////
    
    
    [SerializeField]
    private CanvasGroup golemCanvas;
    [SerializeField]
    private CanvasGroup beetleCanvas;

    /*
        Terracotta = 1,
        Plumber = 2,
        Wooden = 3
        
        RadialLight = 1,
        LightShot = 2,
        LightImpulse = 3
    */
    [HideInInspector]
    public bool isGolem = true;
    [HideInInspector]
    public int ability = 1;
    
    ////////////////////////////////////////  p r i v a t e   v a r i a b l e s  ///////////////////////////////////////     


    public void DeactivateGifAnimation()
    {
        if (isGolem)
        {
            golemCanvas.gameObject.transform.GetChild(ability - 1).gameObject.GetComponent<PlayGifs>().enabled = false;
            golemCanvas.gameObject.transform.GetChild(ability - 1).gameObject.SetActive(false);
        }
        else
        {
            beetleCanvas.gameObject.transform.GetChild(ability - 1).gameObject.GetComponent<PlayGifs>().enabled = false;
            beetleCanvas.gameObject.transform.GetChild(ability - 1).gameObject.SetActive(false);
        }
    }
    
    public void ActivateGifAnimation()
    {
        if (isGolem)
        {
            golemCanvas.alpha = 1;
            beetleCanvas.alpha = 0;

            golemCanvas.gameObject.transform.GetChild(ability - 1).gameObject.GetComponent<PlayGifs>().enabled = true;
            golemCanvas.gameObject.transform.GetChild(ability - 1).gameObject.SetActive(true);
        }
        else
        {
            golemCanvas.alpha = 0;
            beetleCanvas.alpha = 1;

            beetleCanvas.gameObject.transform.GetChild(ability - 1).gameObject.GetComponent<PlayGifs>().enabled = true;
            beetleCanvas.gameObject.transform.GetChild(ability - 1).gameObject.SetActive(true);
        }
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
