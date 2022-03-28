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

    // 0-> swapTag     1-> parchmentTag     2->selectionWheelTag
    public List<Image> kbTags;
    public List<Image> xboxTags;
    public List<Image> psTags;


    public GameObject selectionWheelGO;
    //public Instructions instructionsScript;

    ////////////////////////////////////////  p r i v a t e   v a r i a b l e s  ///////////////////////////////////////            

    private string _swapKeyboardTag;
    private string _selectionWheelKeyboardTag;
    private string _parchmentKeyboardTag;

    private string _swapJoyStickTag;
    private string _selectionWheelJoyStickTag;
    private string _parchmentJoyStickTag;

    [HideInInspector]
    public bool menuOpen;

    public void OnUISwapCharacter(InputAction.CallbackContext context)
    {
        if (!context.performed || menuOpen)
            return;
        UISwapCharacter();
    }
    
    public void UISwapCharacter()
    {
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
        if (!context.performed || menuOpen)
            return;

        // highlight button:     0-> swapTag     1-> parchmentTag     2->selectionWheelTag
        StartCoroutine(highLightButton(1));
    }

    public void UISelectHability(InputAction.CallbackContext context)
    {
        if (context.canceled && menuOpen)
        {
            selectionWheelGO.SetActive(false);
            GameManager.instance.play();
            menuOpen = false;
            return;
        }

        GameManager.instance.pause();
        // highlight button:     0-> swapTag     1-> parchmentTag     2->selectionWheelTag
        StartCoroutine(highLightButton(2));

        // stop gameMovement

        // open menu
        selectionWheelGO.GetComponentInChildren<SelectionWheel>().isGolem = isGolem;
        selectionWheelGO.SetActive(true);
        menuOpen = true;
    }

    public void OnDeviceChange(PlayerInput context)
    {

        if (context.devices.Count > 0 && kbTags.Count > 0)
        {
            if (context.devices[0].name.StartsWith("Keyboard"))
            {
                // Keyboard gamepad
                for (int i = 0; i < kbTags.Count; i++)
                {
                    kbTags[i].enabled = true;
                    xboxTags[i].enabled = false;
                    psTags[i].enabled = false;
                }

                if (GameManager.instance)
                    GameManager.instance.usingGamepad = false;

            }
            else if (context.devices[0].name.StartsWith("DualShock"))
            {
                // PlayStation gamepad
                for (int i = 0; i < kbTags.Count; i++)
                {
                    kbTags[i].enabled = false;
                    xboxTags[i].enabled = false;
                    psTags[i].enabled = true;
                }

                if (GameManager.instance)
                    GameManager.instance.usingGamepad = true;
            }
            else
            {
                // Xbox gamepad
                for (int i = 0; i < kbTags.Count; i++)
                {
                    kbTags[i].enabled = false;
                    xboxTags[i].enabled = true;
                    psTags[i].enabled = false;
                }

                if (GameManager.instance)
                    GameManager.instance.usingGamepad = true;
            }
        }
    }

    IEnumerator highLightButton(int buttomIndex)
    {
        if (kbTags.Count > 0)
        {
            if (kbTags[0].enabled)
            {
                kbTags[buttomIndex].color = new Color(0.676f, 0.676f, 0.676f);
                yield return new WaitForSeconds(0.1f);
                kbTags[buttomIndex].color = Color.white;
            }
            else if (xboxTags[0].enabled)
            {
                xboxTags[buttomIndex].color = new Color(0.676f, 0.676f, 0.676f);
                yield return new WaitForSeconds(0.1f);
                xboxTags[buttomIndex].color = Color.white;
            }
            else if (psTags[0].enabled)
            {
                psTags[buttomIndex].color = new Color(0.676f, 0.676f, 0.676f);
                yield return new WaitForSeconds(0.1f);
                psTags[buttomIndex].color = Color.white;
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

    }
}
