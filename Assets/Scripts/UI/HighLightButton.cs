using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighLightButton : MonoBehaviour
{
    /////////////////////////////////////////  p u b l i c   v a r i a b l e s  ////////////////////////////////////////
    public float scaleFactor = 1.0f;
    public Color highLightColor;
    
    ////////////////////////////////////////  p r i v a t e   v a r i a b l e s  ///////////////////////////////////////   
    private Color _originalButtonColor;
    private Vector3 _originalScaleFactor;

    public void DoChanges()
    {
        gameObject.GetComponent<RectTransform>().localScale = _originalScaleFactor * scaleFactor;
        gameObject.GetComponent<Image>().color = highLightColor;
    }

    public void UndoChanges()
    {
        gameObject.GetComponent<RectTransform>().localScale = _originalScaleFactor;
        gameObject.GetComponent<Image>().color = _originalButtonColor;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _originalButtonColor = gameObject.GetComponent<Image>().color;
        _originalScaleFactor = gameObject.GetComponent<RectTransform>().localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
