using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;

public class LateralMenuUI : MonoBehaviour
{
    // This script changes the visible parchment when switching between characters and the buttons input key
    // --> change color to A9A9A9 when pressing
    
    /////////////////////////////////////////  p u b l i c   v a r i a b l e s  ////////////////////////////////////////
    public bool isGolem = true;

    public int testInput = 0; 
    
    public Image beetleParchment;
    public Image golemParchment;

    public Image beetleIconColor;
    public Image golemIconColor;
    
    // 0-> swapTag     1-> parchmentTag     2->selectionWheelTag
    public List<Image> kbTags;
    public List<Image> xboxTags;
    public List<Image> psTags;
    
    ////////////////////////////////////////  p r i v a t e   v a r i a b l e s  ///////////////////////////////////////            

    private string _swapKeyboardTag;
    private string _selectionWheelKeyboardTag;
    private string _parchmentKeyboardTag;
    
    private string _swapJoyStickTag;
    private string _selectionWheelJoyStickTag;
    private string _parchmentJoyStickTag;
    
    public void UISwapCharacter(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        // highlight button:     0-> swapTag     1-> parchmentTag     2->selectionWheelTag
        StartCoroutine(highLightButton(0));
        
        if (isGolem)
        {
            // Activate beetle parchment
            beetleParchment.enabled = true;
            golemParchment.enabled = false;
            
            beetleIconColor.enabled = true;
            golemIconColor.enabled = false;
        }
        else
        {
            // Activate golem parchment
            golemParchment.enabled = true;
            beetleParchment.enabled = false;
            
            golemIconColor.enabled = true;
            beetleIconColor.enabled = false;
        }
        
        isGolem = !isGolem;
    }
    
    public void UIOpenInstructions(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        // highlight button:     0-> swapTag     1-> parchmentTag     2->selectionWheelTag
        StartCoroutine(highLightButton(1));
        
    }
    
    public void UISelectHability(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        // highlight button:     0-> swapTag     1-> parchmentTag     2->selectionWheelTag
        StartCoroutine(highLightButton(2));

    }

    /*public void OnDeviceJoin(InputAction.CallbackContext context)
    {

        if (context.control.device is Keyboard)
        {
            // Keyboard gamepad
            for (int i = 0; i< kbTags.Count; i++)
            {
                kbTags[i].enabled = true;
                xboxTags[i].enabled = false;
                psTags[i].enabled = false;
            }

        }
        else if (context.control.device is XInputController)
        {
            // Xbox gamepad
            for (int i = 0; i< kbTags.Count; i++)
            {
                kbTags[i].enabled = false;
                xboxTags[i].enabled = true;
                psTags[i].enabled = false;
            }
            
        }
        else if (context.control.device is DualShockGamepad)
        {
            // PlayStation gamepad
            for (int i = 0; i< kbTags.Count; i++)
            {
                kbTags[i].enabled = false;
                xboxTags[i].enabled = false;
                psTags[i].enabled = true;
            }
            
        }
    }*/
    
    
    public void OnDeviceChange(PlayerInput context)
    {

        if (context.devices.Count > 0 && kbTags.Count > 0)
        {
            if (context.devices[0].name == "Keyboard" || testInput == 0)
            {
                // Keyboard gamepad
                for (int i = 0; i < kbTags.Count; i++)
                {
                    kbTags[i].enabled = true;
                    xboxTags[i].enabled = false;
                    psTags[i].enabled = false;
                }

            }
            if (context.devices[0].name == "XInputController" || testInput == 1)
            {
                // Xbox gamepad
                for (int i = 0; i < kbTags.Count; i++)
                {
                    kbTags[i].enabled = false;
                    xboxTags[i].enabled = true;
                    psTags[i].enabled = false;
                }

            }
            if (context.devices[0].name == "DualShockGamepad" || testInput == 2)
            {
                // PlayStation gamepad
                for (int i = 0; i < kbTags.Count; i++)
                {
                    kbTags[i].enabled = false;
                    xboxTags[i].enabled = false;
                    psTags[i].enabled = true;
                }

            }
        }
    }

    IEnumerator highLightButton(int buttomIndex)
    {
        if (kbTags.Count > 0)
        {
            if (kbTags[0])
            {
                kbTags[buttomIndex].color = new Color(0.676f, 0.676f, 0.676f);
                yield return new WaitForSeconds(0.1f);
                kbTags[buttomIndex].color = Color.white;
            }
            else if (xboxTags[0])
            {
                xboxTags[buttomIndex].color = new Color(169, 169, 169);
                yield return new WaitForSeconds(0.1f);
                xboxTags[buttomIndex].color = Color.white;;
            }
            else if (psTags[0])
            {
                psTags[buttomIndex].color = new Color(169, 169, 169);
                yield return new WaitForSeconds(0.1f);
                psTags[buttomIndex].color = Color.white;;
            }
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
        // ONLY FOR TESTING !!!!!!!!!!!!
        if (testInput == 0)
        {
            // Keyboard gamepad
            for (int i = 0; i < kbTags.Count; i++)
            {
                kbTags[i].enabled = true;
                xboxTags[i].enabled = false;
                psTags[i].enabled = false;
            }

        }
        if (testInput == 1)
        {
            // Xbox gamepad
            for (int i = 0; i < kbTags.Count; i++)
            {
                kbTags[i].enabled = false;
                xboxTags[i].enabled = true;
                psTags[i].enabled = false;
            }

        }
        if (testInput == 2)
        {
            // PlayStation gamepad
            for (int i = 0; i < kbTags.Count; i++)
            {
                kbTags[i].enabled = false;
                xboxTags[i].enabled = false;
                psTags[i].enabled = true;
            }

        }
    }
}
