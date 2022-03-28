using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SelectionWheel : MonoBehaviour
{

    /////////////////////////////////////////  p u b l i c   v a r i a b l e s  ////////////////////////////////////////
    public Camera mainCamera;

    [SerializeField] 
    private CanvasGroup golemCanvas;
    [SerializeField] 
    private CanvasGroup beetleCanvas;
    public List<Image> wheelButtons;

    public Color neutralColor;
    public Color hoverColor;
    
    public bool isGolem;
    
    [HideInInspector]
    public int _golemHability = 1;
    public int _beetleHability = 1;
    
    ////////////////////////////////////////  p r i v a t e   v a r i a b l e s  ///////////////////////////////////////
    private Vector2 _selectorPosition;
    private bool _selectorMoving;
    
    
    public void OnSelectorMovement(InputAction.CallbackContext context)
    {
        // get selector position
        _selectorMoving = context.performed;
        if (_selectorMoving)
        {
            _selectorPosition = context.ReadValue<Vector2>();
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
            var stepLength = 360f / 3;
            var selectorAngle = NormalizeAngle(Vector3.SignedAngle(Vector3.up,
                                                _selectorPosition - new Vector2(transform.position.x,
                                                    transform.position.y), Vector3.forward) +
                                                    stepLength / 2f);
            
            //------------  DEBUG  ------------//
            // Debug.Log("Mouse Angle: " + selectorAngle);
            // Debug.Log("Mouse Position: " + mainCamera.ScreenToViewportPoint(_selectorPosition));
            //---------------------------------//

            var selectorPositionVp = mainCamera.ScreenToViewportPoint(_selectorPosition);
            if ((selectorPositionVp.x >= 0.4 && selectorPositionVp.x <= 0.6) &&
                (selectorPositionVp.y >= 0.4 && selectorPositionVp.y <= 0.6))
            {
                wheelButtons[0].color = hoverColor;
                wheelButtons[1].color = neutralColor;
                wheelButtons[2].color = neutralColor;
                wheelButtons[3].color = neutralColor;
            }
            else
            {

                if (selectorAngle > 223 && selectorAngle <= 346) // B3
                {
                    wheelButtons[0].color = neutralColor;
                    wheelButtons[1].color = hoverColor;
                    wheelButtons[2].color = neutralColor;
                    wheelButtons[3].color = neutralColor;
                    
                    if (isGolem)
                        _golemHability = 3;
                    else
                        _beetleHability = 3;

                }else if (selectorAngle > 105 && selectorAngle <= 223) // B2
                {
                    wheelButtons[0].color = neutralColor;
                    wheelButtons[1].color = neutralColor;
                    wheelButtons[2].color = hoverColor;
                    wheelButtons[3].color = neutralColor;

                    if (isGolem)
                        _golemHability = 2;
                    else
                        _beetleHability = 2;

                }else // B1
                {
                    wheelButtons[0].color = neutralColor;
                    wheelButtons[1].color = neutralColor;
                    wheelButtons[2].color = neutralColor;
                    wheelButtons[3].color = hoverColor;
                    
                    if(isGolem)
                        _golemHability = 1;
                    else
                        _beetleHability = 1;
                }
            }
        }
        
    }
    
}

