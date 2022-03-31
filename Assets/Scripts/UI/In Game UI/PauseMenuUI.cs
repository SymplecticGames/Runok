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
    Hub = 2
}
public class PauseMenuUI : MonoBehaviour
{
    /////////////////////////////////////////  p u b l i c   v a r i a b l e s  ////////////////////////////////////////
    public GameObject pauseMenuPanel;
    public Button continueButton;
    public Button mainMenuButton;
    public Button goToHubButton;
    
    public GameObject confirmationPanel;
    public Button yesButton;
    public Button noButton;
    
    ////////////////////////////////////////  p r i v a t e   v a r i a b l e s  ///////////////////////////////////////
    private bool _paused;
    private ClickedButton _clickedButton;


    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        
        if (_paused)
        {
            Continue();
        }
        else
        {
            Debug.Log("PAUSAR");
            GameManager.instance.player.input.actions.FindAction("ActiveFw_Hit").Disable();
            GameManager.instance.pause();
            pauseMenuPanel.SetActive(true);
            _paused = !_paused;    
        }
        
    }

    public void Continue()
    {
        GameManager.instance.player.input.actions.FindAction("ActiveFw_Hit").Enable();
        GameManager.instance.play();
        pauseMenuPanel.SetActive(false);
        _clickedButton = ClickedButton.Continue;
        _paused = !_paused;
    }

    public void GoToMainMenu()
    {
        _clickedButton = ClickedButton.MainMenu;
        confirmationPanel.SetActive(true);
    }

    public void GoToHub()
    {
        _clickedButton = ClickedButton.Hub;
        confirmationPanel.SetActive(true);
    }

    //////////////////////////////////////  CONFIRMATION PANEL//////////////////////////////////////  
    public void ClickedYes()
    {
        switch (_clickedButton)
        {
            case ClickedButton.Hub:
                // open hub menu
                SceneManager.LoadScene(2);
                break;
            
            case ClickedButton.MainMenu:
                // open main menu
                SceneManager.LoadScene(1);
                break;
            
            default:
                break;
        }
        confirmationPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);

    }

    public void ClickedNo()
    {
        confirmationPanel.SetActive(false);        
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////
    
    // Start is called before the first frame update
    void Start()
    {
        
        continueButton.onClick.AddListener(Continue);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        goToHubButton.onClick.AddListener(GoToHub);
        
        yesButton.onClick.AddListener(ClickedYes);
        noButton.onClick.AddListener(ClickedNo);
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
