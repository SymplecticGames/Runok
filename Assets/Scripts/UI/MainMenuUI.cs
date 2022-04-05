using System;
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
    public Transform creditsButton; 
    
    ////////////////////////////////////////  p r i v a t e   v a r i a b l e s  ///////////////////////////////////////
    private float _selectorTargetPosition;
    private float _damp = 5.0f;
    private float _displacement = -10.0f;
    
    // fading parameters
    private bool _doFading;
    private float _pgbTargetPos = 0.0f;
    private float _egbTargetPos = 0.0f;
    private float _cbTargetPos = 0.0f;

    private float _maxX = 5024.0f;
    
    // audio
    private string audioFolder;
    private AudioSource _audioSource;
    private AudioClip _audioClip;

    public void ClickedPlayGame()
    {
        // open hub menu

        if(!selector.activeInHierarchy)
            return;
        
        AudioManager.audioInstance.PlayUISound(UIaudioTag.click);
        SceneManager.LoadScene("Hub");
    }
    
    public void ClickedExitGame() {
        
        // exit Runok
        if(!selector.activeInHierarchy)
            return;
        AudioManager.audioInstance.PlayUISound(UIaudioTag.click);
        Application.Quit();
    }
    
    public void ClickedCredits() {
        
        // credits scene
        if(!selector.activeInHierarchy)
            return;
        AudioManager.audioInstance.PlayUISound(UIaudioTag.click);
        SceneManager.LoadScene("Credits");
    }

    public void HoverPlayGameButton()
    {
        // change selector position
        if(!selector.activeInHierarchy)
            return;

        _selectorTargetPosition = playGameButton.localPosition.y;
    }
    
    public void HoverExitGameButton()
    {
        // change selector position
        if(!selector.activeInHierarchy)
            return;
        _selectorTargetPosition = exitGameButton.transform.localPosition.y;
    }
    
    public void HoverCreditsButton()
    {
        // change selector position
        if(!selector.activeInHierarchy)
            return;
        
        _selectorTargetPosition = creditsButton.localPosition.y;
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
        
        _cbTargetPos = creditsButton.localPosition.x;
        creditsButton.localPosition = new Vector3(_maxX, creditsButton.localPosition.y, creditsButton.localPosition.z);
        
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
            
            pos = creditsButton.localPosition;
            creditsButton.localPosition = Vector2.Lerp(pos, 
                new Vector3(_cbTargetPos, pos.y, pos.z), Time.deltaTime * _damp / 1.25f);
            
            _doFading = !(title.alpha >= 1 && (Mathf.Abs(_pgbTargetPos - playGameButton.localPosition.x) <= 0.001f));
            
            if (!_doFading)
            {
                selector.SetActive(true);
                HoverPlayGameButton();
            }
            
        }
        
    }
}
