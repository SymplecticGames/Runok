using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{

    /////////////////////////////////////////  p u b l i c   v a r i a b l e s  ////////////////////////////////////////
    public CanvasGroup title;
    public GameObject selector;
    public Transform playGameButton; 
    public Transform exitGameButton; 
    
    ////////////////////////////////////////  p r i v a t e   v a r i a b l e s  ///////////////////////////////////////
    private float _selectorTargetPosition;
    private float _damp = 5.0f;
    private float _displacement = -15.0f;
    
    // fading parameters
    private bool _doFading;
    private float _fadingDuration = 0.4f;
    private float _pgbTargetPos = 0.0f;
    private float _egbTargetPos = 0.0f;

    private float _maxX = 5024.0f;
    
    
    public void ClickedPlayGame()
    {
        // open hub menu
        SceneManager.LoadScene(2);
    }
    
    public void ClickedExitGame() {
        
        // exit Runok
        Application.Quit();
    }

    public void HoverPlayGameButton()
    {
        // change selector position
        _selectorTargetPosition = playGameButton.localPosition.y;
    }
    
    public void HoverExitGameButton()
    {
        // change selector position
        _selectorTargetPosition = exitGameButton.transform.localPosition.y;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _doFading = true;
        selector.SetActive(false);
        title.alpha = 0.0f;
        
        _pgbTargetPos = playGameButton.localPosition.x;
        playGameButton.localPosition = new Vector3(_maxX, playGameButton.localPosition.y, playGameButton.localPosition.z);
        
        _egbTargetPos = exitGameButton.localPosition.x;
        exitGameButton.localPosition = new Vector3(_maxX, exitGameButton.localPosition.y, exitGameButton.localPosition.z);
        
        _selectorTargetPosition = selector.transform.localPosition.y;

    }

    // Update is called once per frame
    void Update()
    {
        // move selector
        Vector3 pos = selector.transform.localPosition;
        selector.transform.localPosition = Vector2.Lerp(selector.transform.localPosition, 
                                            new Vector3(pos.x,_selectorTargetPosition  + _displacement, pos.z), Time.deltaTime * _damp);

        if (_doFading)
        {
            // fade title
            if (title.alpha < 1)
            {
                title.alpha = Mathf.Lerp(title.alpha, 1.1f, Time.deltaTime);
            }
            
            // move buttons
            pos = playGameButton.localPosition;
            playGameButton.localPosition = Vector2.Lerp(pos, 
                new Vector3(_pgbTargetPos, pos.y, pos.z), Time.deltaTime * _damp);
            
            pos = exitGameButton.localPosition;
            exitGameButton.localPosition = Vector2.Lerp(pos, 
                new Vector3(_egbTargetPos, pos.y, pos.z), Time.deltaTime * _damp / 1.5f);
            
            _doFading = !(title.alpha >= 1 && (Mathf.Abs(_pgbTargetPos - playGameButton.localPosition.x) <= 0.001f));
            
            if (!_doFading)
            {
                selector.SetActive(true);
                HoverPlayGameButton();
            }
            
        }
        
    }
}
