using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    public Button previousButton;
    public Button nextButton;
    public Button skip_closeButton;

    // 0-3->Lvl1 {{0-> env1,  1-> env2,  2-> cntrls1,  3-> cntrls2  4-> cntrls2(1)}},  5->Lvl2,  6->Lvl4  
    public CanvasGroup[] clues;

    public GameObject CanvasInGameUI;
    
    private LateralMenuUI lateralMenu;
    private PauseMenuUI pauseMenu;
    
    private int clueIndex;

    private int maxLength;

    private void Awake()
    {

    }

    IEnumerator WaitToShow()
    {
        yield return new WaitForSeconds(0.5f);
        if (GameManager.instance)
        {
            GameManager.instance.player.PausePlayer();
            GameManager.instance.pause();
        }
        gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;

        lateralMenu = CanvasInGameUI.GetComponent<LateralMenuUI>();
        pauseMenu = CanvasInGameUI.GetComponent<PauseMenuUI>();

        lateralMenu.showingTutorial = true;
        pauseMenu.showingTutorial = true;

        switch (ProgressManager.instance.currentLevel)
        {
            case 0:
                clueIndex = 0;
                maxLength = 5;
                break;

            case 1:
                clueIndex = 5;
                maxLength = 6;
                nextButton.gameObject.SetActive(false);
                EventSystem.current.SetSelectedGameObject(previousButton.gameObject);
                nextButton.interactable = false;
                skip_closeButton.GetComponentInChildren<Text>().text = "Cerrar pistas";
                break;

            case 3:
                if (!SceneManager.GetActiveScene().name.Contains("Boss"))
                {
                    clueIndex = 6;
                    maxLength = 7;
                    nextButton.gameObject.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(previousButton.gameObject);
                    nextButton.interactable = false;
                    skip_closeButton.GetComponentInChildren<Text>().text = "Cerrar pistas";
                }
                break;
        }

        previousButton.gameObject.SetActive(false);
        previousButton.interactable = false;
        EventSystem.current.SetSelectedGameObject(nextButton.gameObject);

        if(!nextButton.gameObject.activeInHierarchy)
            EventSystem.current.SetSelectedGameObject(skip_closeButton.gameObject);
        ShowGroup();

    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;

        if (ProgressManager.instance.currentLevel != 2)
            StartCoroutine(WaitToShow());
        else
            gameObject.SetActive(false);
        
    }

    public void ClickedSkip_Close()
    {
        // disable tutorial panel
        lateralMenu.showingTutorial = false;
        pauseMenu.showingTutorial = false;
        gameObject.SetActive(false);
        GameManager.instance.player.ResumePlayer();
        GameManager.instance.play();
        
    }

    public void ClickedPrevious()
    {
        AudioManager.audioInstance.PlayUISound(UIAudioTag.click);
        AudioManager.audioInstance.PlayUISound(UIAudioTag.click);
        if (clueIndex == 1)
        {
            previousButton.gameObject.SetActive(false);
            previousButton.interactable = false;
            EventSystem.current.SetSelectedGameObject(nextButton.gameObject);
        }
        else if (clueIndex == maxLength - 1)
        {
            nextButton.gameObject.SetActive(true);
            nextButton.interactable = true;
            skip_closeButton.GetComponentInChildren<Text>().text = "Saltar pistas";
        }
        clueIndex--;
        
        ShowGroup();
    }

    public void ClickedNext()
    {
        AudioManager.audioInstance.PlayUISound(UIAudioTag.click);
        if (clueIndex <= maxLength - 1)
        {
            if (clueIndex == 0)
            {
                previousButton.gameObject.SetActive(true);
                previousButton.interactable = true;
            }
            else if (clueIndex == maxLength - 2)
            {
                nextButton.gameObject.SetActive(false);
                nextButton.interactable = false;
                skip_closeButton.GetComponentInChildren<Text>().text = "Cerrar pistas";
                EventSystem.current.SetSelectedGameObject(previousButton.gameObject);
            }
            clueIndex++;
            ShowGroup();
        }
        
    }
    

    private void ShowGroup()
    {
        for (int i = 0; i < clues.Length; i++)
        {
            clues[i].alpha = (clueIndex == i) ? 1.0f : 0.0f;
        }
    }
    
    // Update is called once per frame
    void Update()
    {

    }
}
