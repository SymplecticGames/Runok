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
            PausePlayer();
            GameManager.instance.pause();
            pauseMenuPanel.SetActive(true);
            _paused = !_paused;
            AudioManager.audioInstance.PlayUISound(UIAudioTag.openPauseMenu);
        }
        EventSystem.current.SetSelectedGameObject(continueButton.gameObject);

    }

    public void Continue()
    {
        ResumePlayer();
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
    }

    private void PausePlayer()
    {
        GameManager.instance.player.input.actions.FindAction("Look").Disable();
        GameManager.instance.player.input.actions.FindAction("GolemHit").Disable();
        GameManager.instance.player.input.actions.FindAction("GolemJump").Disable();
        GameManager.instance.player.input.actions.FindAction("ReturnToGolem").Disable();
        GameManager.instance.player.input.actions.FindAction("SwapCharacter").Disable();
        GameManager.instance.player.input.actions.FindAction("BeetleFrontRay").Disable();
        GameManager.instance.player.input.actions.FindAction("BeetleBackRay").Disable();
        GameManager.instance.player.input.actions.FindAction("BeetleShoot").Disable();
        GameManager.instance.player.input.actions.FindAction("OpenInstructions").Disable();
        GameManager.instance.player.input.actions.FindAction("WheelMenu").Disable();
    }

    private void ResumePlayer()
    {
        GameManager.instance.player.input.actions.FindAction("Look").Enable();
        GameManager.instance.player.input.actions.FindAction("GolemHit").Enable();
        GameManager.instance.player.input.actions.FindAction("GolemJump").Enable();
        GameManager.instance.player.input.actions.FindAction("ReturnToGolem").Enable();
        GolemBoss golemBoss = FindObjectOfType<GolemBoss>();
        if (!golemBoss)
            GameManager.instance.player.input.actions.FindAction("SwapCharacter").Enable();
        else if (golemBoss.beaten)
            GameManager.instance.player.input.actions.FindAction("SwapCharacter").Enable();
        GameManager.instance.player.input.actions.FindAction("BeetleFrontRay").Enable();
        GameManager.instance.player.input.actions.FindAction("BeetleBackRay").Enable();
        GameManager.instance.player.input.actions.FindAction("BeetleShoot").Enable();
        GameManager.instance.player.input.actions.FindAction("OpenInstructions").Enable();
        if (GameManager.instance.player.selectionWheelEnabled) GameManager.instance.player.input.actions.FindAction("WheelMenu").Enable();
    }


    // Update is called once per frame
    void Update()
    {
    }
}
