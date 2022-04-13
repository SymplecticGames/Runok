using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class LateralMenuUI : MonoBehaviour
{
    // This script changes the visible parchment when switching between characters and the buttons input key
    // --> change color to A9A9A9 when pressing

    /////////////////////////////////////////  p u b l i c   v a r i a b l e s  ////////////////////////////////////////
    public bool isGolem = true;

    public Image beetleParchment;
    public Image golemParchment;

    public Image beetleIconColor;
    public Image golemIconColor;
    
    public GameObject selectionWheelGO;
    public GameObject instructionsMenuGO;

    
    public Image beetleSight;   // Camera.main.WorldToViewportPoint(transform.position) --> 0.4 izq, 0.5 centr, 0.6 der
    
    ////////////////////////////////////////  p r i v a t e   v a r i a b l e s  ///////////////////////////////////////            


    public bool MenuOpened()
    {
        return selectionWheelGO.activeInHierarchy || instructionsMenuGO.activeInHierarchy;
    }
    
    
    public void OnUISwapCharacter(InputAction.CallbackContext context)
    {
        if (!context.performed || MenuOpened())
            return;
        UISwapCharacter();
    }
    
    public void UISwapCharacter()
    {
        
        // highlight button:     0-> swapTag     1-> parchmentTag     2->selectionWheelTag   3-> hitTag    4-> jumpTag
        StartCoroutine(GameManager.instance.highLightTag(deviceTag.swapTag));

        if (isGolem)
        {
            // Activate beetle parchment
            beetleParchment.enabled = true;
            golemParchment.enabled = false;

            beetleIconColor.enabled = true;
            golemIconColor.enabled = false;
            
            beetleSight.gameObject.SetActive(false);
        }
        else
        {
            // Activate golem parchment
            golemParchment.enabled = true;
            beetleParchment.enabled = false;

            golemIconColor.enabled = true;
            beetleIconColor.enabled = false;

            if (GameManager.instance.player.GetSelection() == 2)
            {
                beetleSight.gameObject.SetActive(true);
            }
            else
            {
                beetleSight.gameObject.SetActive(false);
            }
        }

        isGolem = !isGolem;
    }

    public void UIOpenInstructions(InputAction.CallbackContext context)
    {
        if (!selectionWheelGO.activeInHierarchy)
        {
            if (context.canceled && instructionsMenuGO.activeInHierarchy)
            {
                instructionsMenuGO.GetComponentInChildren<InstructionsUI>().DeactivateGifAnimation();
                instructionsMenuGO.SetActive(false);
                GameManager.instance.play();
                AudioManager.audioInstance.PlayUISound(UIAudioTag.instructions);
                return;
            }

            // stop gameMovement
            GameManager.instance.pause();

            if (context.started)
            {
                // highlight button:     0-> swapTag     1-> parchmentTag     2->selectionWheelTag  3-> hitTag    4-> jumpTag
                StartCoroutine(GameManager.instance.highLightTag(deviceTag.parchmentTag));
            }

            // open menu
            instructionsMenuGO.GetComponentInChildren<InstructionsUI>().isGolem = isGolem;
            instructionsMenuGO.GetComponentInChildren<InstructionsUI>().ability = GameManager.instance.player.GetSelection();
            instructionsMenuGO.SetActive(true);
            instructionsMenuGO.GetComponentInChildren<InstructionsUI>().ActivateGifAnimation();
            AudioManager.audioInstance.PlayUISound(UIAudioTag.instructions);
        }
    }

    public void UISelectHability(InputAction.CallbackContext context)
    {
        if (!instructionsMenuGO.activeInHierarchy)
        {
            if (context.canceled && selectionWheelGO.activeInHierarchy)
            {
                selectionWheelGO.SetActive(false);
                GameManager.instance.play();
                GameManager.instance.player.ApplySelection(selectionWheelGO.GetComponent<SelectionWheel>().ability);
                
                // play selected ability sound
                AudioManager.audioInstance.PlayAbilitySound(GameManager.instance.player.GetSelection(), isGolem);
 
                // enable/disable beetleSight
                if (!isGolem && GameManager.instance.player.GetSelection() == 2)
                {
                    beetleSight.gameObject.SetActive(true);
                }
                else
                {
                    beetleSight.gameObject.SetActive(false);
                }
                
                return;
            }

            // stop gameMovement
            GameManager.instance.pause();

            if (context.started)
            {
                // highlight button:     0-> swapTag     1-> parchmentTag     2->selectionWheelTag  3-> hitTag    4-> jumpTag
                StartCoroutine(GameManager.instance.highLightTag(deviceTag.selectionWheelTag));
                AudioManager.audioInstance.PlayUISound(UIAudioTag.openSelectionWheel);
            }

            // open menu
            selectionWheelGO.GetComponentInChildren<SelectionWheel>().isGolem = isGolem;
            selectionWheelGO.SetActive(true);
        }
    }
    
   
    // Start is called before the first frame update
    void Start()
    {
        if (isGolem)
        {
            beetleParchment.enabled = false;
            beetleIconColor.enabled = false;
        }
        else
        {
            golemParchment.enabled = false;
            golemIconColor.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {

    }
}
