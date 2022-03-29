using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SelectionWheel : MonoBehaviour
{

    /////////////////////////////////////////  p u b l i c   v a r i a b l e s  ////////////////////////////////////////
    [SerializeField]
    private CanvasGroup golemCanvas;
    [SerializeField]
    private CanvasGroup beetleCanvas;
    public List<Image> wheelButtons;

    public Color neutralColor;
    public Color hoverColor;

    public bool isGolem;

    [HideInInspector]
    public int ability = 1;

    ////////////////////////////////////////  p r i v a t e   v a r i a b l e s  ///////////////////////////////////////
    private Vector2 _selectorPosition;
    private bool _selectorMoving;


    public void OnSelectorMovement(InputAction.CallbackContext context)
    {
        // get selector position
        _selectorMoving = context.performed;
        if (_selectorMoving)
        {
            if (GameManager.instance.usingGamepad)
                _selectorPosition = (context.ReadValue<Vector2>() + Vector2.one) * 0.5f;
            else
                _selectorPosition = Camera.main.ScreenToViewportPoint(context.ReadValue<Vector2>());
        }
    }

    private float NormalizeAngle(float a)
    {
        return (a + 360f) % 360f;
    }


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (isGolem)
        {
            golemCanvas.alpha = 1;
            beetleCanvas.alpha = 0;
        }
        else
        {
            golemCanvas.alpha = 0;
            beetleCanvas.alpha = 1;
        }


        if (_selectorMoving)
        {
            float stepLength = 360f / 3;
            float selectorAngle = NormalizeAngle(Vector3.SignedAngle(Vector3.up, _selectorPosition - new Vector2(0.5f, 0.5f), Vector3.forward) + stepLength / 2f);

            //------------  DEBUG  ------------//
            // Debug.Log("Mouse Angle: " + selectorAngle);
            // Debug.Log("Mouse Position: " + mainCamera.ScreenToViewportPoint(_selectorPosition));
            //---------------------------------//

            if ((_selectorPosition.x >= 0.4 && _selectorPosition.x <= 0.6) && (_selectorPosition.y >= 0.4 && _selectorPosition.y <= 0.6))
            {

                if (!GameManager.instance.usingGamepad)
                {
                    wheelButtons[0].color = hoverColor;
                    wheelButtons[1].color = neutralColor;
                    wheelButtons[2].color = neutralColor;
                    wheelButtons[3].color = neutralColor;

                    ability = 0;
                }
            }
            else
            {

                if (selectorAngle > 223 && selectorAngle <= 346) // B3
                {
                    wheelButtons[0].color = neutralColor;
                    wheelButtons[1].color = hoverColor;
                    wheelButtons[2].color = neutralColor;
                    wheelButtons[3].color = neutralColor;

                    ability = 3;
                }
                else if (selectorAngle > 105 && selectorAngle <= 223) // B2
                {
                    wheelButtons[0].color = neutralColor;
                    wheelButtons[1].color = neutralColor;
                    wheelButtons[2].color = hoverColor;
                    wheelButtons[3].color = neutralColor;

                    ability = 2;
                }
                else // B1
                {
                    wheelButtons[0].color = neutralColor;
                    wheelButtons[1].color = neutralColor;
                    wheelButtons[2].color = neutralColor;
                    wheelButtons[3].color = hoverColor;

                    ability = 1;
                }
            }
        }
        else if (GameManager.instance.usingGamepad)
        {
            wheelButtons[0].color = hoverColor;
            wheelButtons[1].color = neutralColor;
            wheelButtons[2].color = neutralColor;
            wheelButtons[3].color = neutralColor;

            ability = 0;
        }
    }

}

