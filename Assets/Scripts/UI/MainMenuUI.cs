using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{

    /////////////////////////////////////////  p u b l i c   v a r i a b l e s  ////////////////////////////////////////
    public CanvasGroup title;
    public GameObject selector;
    public Transform newGameButton;
    public Transform continueGameButton;
    public Transform settingsButton;
    public Transform exitGameButton;
    public Transform creditsButton;

    public GameObject settingsUIgo;
    public Slider settingsMusicButton;

    ////////////////////////////////////////  p r i v a t e   v a r i a b l e s  ///////////////////////////////////////
    private float _selectorTargetPosition;
    private float _damp = 5.0f;
    private float _displacement = -10.0f;

    // fading parameters
    private bool _doFading;
    private float _ngbTargetPos = 0.0f;
    private float _cgbTargetPos = 0.0f;
    private float _egbTargetPos = 0.0f;
    private float _cbTargetPos = 0.0f;
    private float _sbTargetPos = 0.0f;

    private float _maxX = 5024.0f;

    private bool _settingsOpened;

    [SerializeField]
    private Animator fadePanelAnim;

    public void ClickedNewGame()
    {
        if (!selector.activeInHierarchy || _settingsOpened)
            return;

        // resetear la partida para empezar una nueva
        ProgressManager.instance.currentLevel = 0;
        ProgressManager.instance.currentCompletedLevels = 0;
        ProgressManager.instance.SaveGame();  // se guarda la partida en el archivo con nivel actual 0 (se borra el entero que hubiera previamente)

        AudioManager.audioInstance.PlayUISound(UIAudioTag.click);
        Cursor.visible = false;
        SceneManager.LoadScene("Interludio");
    }

    public void ClickedContinueGame()
    {
        if (!selector.activeInHierarchy || _settingsOpened)
            return;

        // cargar desde el nivel en el que se quedó el jugaor
        if (!ProgressManager.instance.LoadGame())
            return;

        AudioManager.audioInstance.PlayUISound(UIAudioTag.click);
        Cursor.visible = true;
        SceneManager.LoadScene("Hub");
    }

    public void ClickedSettings()
    {
        // open settings menu

        if (!selector.activeInHierarchy || _settingsOpened)
            return;

        AudioManager.audioInstance.PlayUISound(UIAudioTag.click);

        settingsUIgo.SetActive(true);
        _settingsOpened = true;

        EventSystem.current.SetSelectedGameObject(settingsMusicButton.gameObject);

    }


    public void ClosedSettings()
    {
        settingsUIgo.SetActive(false);
        _doFading = false;
        _settingsOpened = false;
        EventSystem.current.SetSelectedGameObject(settingsButton.gameObject);
    }

    public void ClickedExitGame()
    {

        // exit Runok
        if (!selector.activeInHierarchy || _settingsOpened)
            return;
        AudioManager.audioInstance.PlayUISound(UIAudioTag.click);
        Application.Quit();
    }

    public void ClickedCredits()
    {

        // credits scene
        if (!selector.activeInHierarchy || _settingsOpened)
            return;
        AudioManager.audioInstance.PlayUISound(UIAudioTag.click);
        Cursor.visible = false;
        SceneManager.LoadScene("Credits");
    }

    public void HoverContinueGameButton()
    {
        // change selector position
        if (!selector.activeInHierarchy || _settingsOpened)
            return;

        _selectorTargetPosition = continueGameButton.localPosition.y;
    }

    public void HoverNewGameButton()
    {
        // change selector position
        if (!selector.activeInHierarchy || _settingsOpened)
            return;

        _selectorTargetPosition = newGameButton.localPosition.y;
    }

    public void HoverSettingsButton()
    {
        // change selector position
        if (!selector.activeInHierarchy || _settingsOpened)
            return;

        _selectorTargetPosition = settingsButton.localPosition.y;
    }

    public void HoverExitGameButton()
    {
        // change selector position
        if (!selector.activeInHierarchy || _settingsOpened)
            return;
        _selectorTargetPosition = exitGameButton.transform.localPosition.y;
    }

    public void HoverCreditsButton()
    {
        // change selector position
        if (!selector.activeInHierarchy || _settingsOpened)
            return;

        _selectorTargetPosition = creditsButton.localPosition.y;
    }

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(newGameButton.gameObject);

        _doFading = true;
        selector.SetActive(false);
        title.alpha = 0.0f;

        _ngbTargetPos = newGameButton.localPosition.x;
        newGameButton.localPosition = new Vector3(_maxX, newGameButton.localPosition.y, newGameButton.localPosition.z);

        _egbTargetPos = exitGameButton.localPosition.x;
        exitGameButton.localPosition = new Vector3(_maxX, exitGameButton.localPosition.y, exitGameButton.localPosition.z);

        _cbTargetPos = creditsButton.localPosition.x;
        creditsButton.localPosition = new Vector3(_maxX, creditsButton.localPosition.y, creditsButton.localPosition.z);

        _sbTargetPos = settingsButton.localPosition.x;
        settingsButton.localPosition = new Vector3(_maxX, settingsButton.localPosition.y, settingsButton.localPosition.z);

        _cgbTargetPos = continueGameButton.localPosition.x;
        continueGameButton.localPosition = new Vector3(_maxX, continueGameButton.localPosition.y, continueGameButton.localPosition.z);

        Color sColor = settingsUIgo.GetComponent<Image>().color;
        settingsUIgo.GetComponent<Image>().color = new Color(sColor.r, sColor.g, sColor.b, 1.0f);

        _selectorTargetPosition = selector.transform.localPosition.y;

        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (!File.Exists(destination))
        {
            Color col = continueGameButton.GetComponentInChildren<Text>().color;
            col.a = 0.2f;
            continueGameButton.GetComponentInChildren<Text>().color = col;
        }

        fadePanelAnim.SetTrigger("doFadeOut");
    }

    // Update is called once per frame
    void Update()
    {
        // move selector
        Vector3 pos = selector.transform.localPosition;
        selector.transform.localPosition = Vector2.Lerp(selector.transform.localPosition,
                                            new Vector3(pos.x, _selectorTargetPosition + _displacement, pos.z), Time.deltaTime * _damp);

        if (_doFading)
        {
            // fade title
            if (title.alpha < 1)
            {
                title.alpha = Mathf.Lerp(title.alpha, 1.1f, Time.deltaTime);
            }

            // move buttons
            pos = newGameButton.localPosition;
            newGameButton.localPosition = Vector2.Lerp(pos,
                new Vector3(_ngbTargetPos, pos.y, pos.z), Time.deltaTime * _damp);

            pos = continueGameButton.localPosition;
            continueGameButton.localPosition = Vector2.Lerp(pos,
                new Vector3(_cgbTargetPos, pos.y, pos.z), Time.deltaTime * _damp / 1.10f);

            pos = settingsButton.localPosition;
            settingsButton.localPosition = Vector2.Lerp(pos,
                new Vector3(_sbTargetPos, pos.y, pos.z), Time.deltaTime * _damp / 1.20f);

            pos = creditsButton.localPosition;
            creditsButton.localPosition = Vector2.Lerp(pos,
                new Vector3(_cbTargetPos, pos.y, pos.z), Time.deltaTime * _damp / 1.30f);

            pos = exitGameButton.localPosition;
            exitGameButton.localPosition = Vector2.Lerp(pos,
                new Vector3(_egbTargetPos, pos.y, pos.z), Time.deltaTime * _damp / 1.40f);

            _doFading = !(title.alpha >= 1 && (Mathf.Abs(_ngbTargetPos - newGameButton.localPosition.x) <= 0.001f));

            if (!_doFading)
            {
                selector.SetActive(true);
                HoverNewGameButton();
            }

        }

    }
}
