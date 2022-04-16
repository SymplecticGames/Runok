using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum ClickedButton
{
    Continue = 0,
    MainMenu = 1,
    Hub = 2,
    Settings = 3
}
public class PauseMenuUI : MonoBehaviour
{
    /////////////////////////////////////////  p u b l i c   v a r i a b l e s  ////////////////////////////////////////
    public GameObject pauseMenuPanel;
    public Button continueButton;
    public Button mainMenuButton;
    public Button goToHubButton;
    public Button goToSettingsButton;

    public GameObject confirmationPanel;
    public Button yesButton;
    public Button noButton;

    public GameObject settingsUI;

    public bool showingTutorial;

    public Slider settingsMusicButton;
    
    ////////////////////////////////////////  p r i v a t e   v a r i a b l e s  ///////////////////////////////////////
    private bool _paused;
    private bool _subMenuOpened;
    private ClickedButton _clickedButton;


    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.performed || _subMenuOpened || showingTutorial)
            return;

        if (_paused)
        {
            Continue();
        }
        else
        {
            GameManager.instance.player.PausePlayer();
            GameManager.instance.pause();
            pauseMenuPanel.SetActive(true);
            _paused = !_paused;
            AudioManager.audioInstance.PlayUISound(UIAudioTag.openPauseMenu);
        }
        if(continueButton.gameObject)
            EventSystem.current.SetSelectedGameObject(continueButton.gameObject);

    }

    public void Continue()
    {
        GameManager.instance.player.ResumePlayer();
        GameManager.instance.play();
        pauseMenuPanel.SetActive(false);
        _clickedButton = ClickedButton.Continue;
        _paused = !_paused;
        AudioManager.audioInstance.PlayUISound(UIAudioTag.closePauseMenu);
    }

    public void GoToSettings()
    {
        _clickedButton = ClickedButton.Settings;
        settingsUI.SetActive(true);
        AudioManager.audioInstance.PlayUISound(UIAudioTag.openPauseMenu);
        HidePauseMenuButtons();
        _subMenuOpened = true;
        
        EventSystem.current.SetSelectedGameObject(settingsMusicButton.gameObject);

    }

    public void HidePauseMenuButtons()
    {
        continueButton.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);
        goToHubButton.gameObject.SetActive(false);
        goToSettingsButton.gameObject.SetActive(false);
    }

    public void ShowPauseMenuButtons()
    {
        continueButton.gameObject.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);
        goToHubButton.gameObject.SetActive(true);
        goToSettingsButton.gameObject.SetActive(true);
    }

    public void ClosedSettings()
    {
        ShowPauseMenuButtons();
        settingsUI.SetActive(false);
        _subMenuOpened = false;
        EventSystem.current.SetSelectedGameObject(goToSettingsButton.gameObject);
    }

    public void GoToMainMenu()
    {
        _clickedButton = ClickedButton.MainMenu;
        EventSystem.current.SetSelectedGameObject(yesButton.gameObject);
        confirmationPanel.SetActive(true);
        AudioManager.audioInstance.PlayUISound(UIAudioTag.showConfirmationPanel);
        _subMenuOpened = true;
    }

    public void GoToHub()
    {
        _clickedButton = ClickedButton.Hub;
        EventSystem.current.SetSelectedGameObject(yesButton.gameObject);
        confirmationPanel.SetActive(true);
        AudioManager.audioInstance.PlayUISound(UIAudioTag.showConfirmationPanel);
        _subMenuOpened = true;
    }

    //////////////////////////////////////  CONFIRMATION PANEL//////////////////////////////////////  
    public void ClickedYes()
    {
        string sceneName = "MainMenu";

        switch (_clickedButton)
        {
            case ClickedButton.Hub:
                // open hub menu
                sceneName = "Hub";
                break;

            case ClickedButton.MainMenu:
                // open main menu
                sceneName = "MainMenu";
                break;

            default:
                break;
        }

        // do transition animation
        SceneTransition.instance.LoadScene(sceneName);
        confirmationPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        AudioManager.audioInstance.PlayUISound(UIAudioTag.click);
    }

    public void ClickedNo()
    {
        EventSystem.current.SetSelectedGameObject(continueButton.gameObject);
        confirmationPanel.SetActive(false);
        AudioManager.audioInstance.PlayUISound(UIAudioTag.click);
        _subMenuOpened = false;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////

    // Start is called before the first frame update
    void Start()
    {

        continueButton.onClick.AddListener(Continue);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        goToHubButton.onClick.AddListener(GoToHub);
        goToSettingsButton.onClick.AddListener(GoToSettings);

        yesButton.onClick.AddListener(ClickedYes);
        noButton.onClick.AddListener(ClickedNo);
        
        settingsUI.GetComponent<SettingsUI>().Start();
    }


    // Update is called once per frame
    void Update()
    {

    }
}
