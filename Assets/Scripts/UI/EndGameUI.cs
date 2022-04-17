using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    public Transform cameraTarget;

    public Text textsDown;
    public Text textsMiddle;
    public Text textsUp;

    public Button continueButton;
        
    private Animator _cameraAnim;

    [SerializeField]
    private Animator fadePanelAnim;

    public void NextAnim()
    {
        AudioManager.audioInstance.PlayUISound(UIAudioTag.click);
        if (_cameraAnim.GetCurrentAnimatorStateInfo(0).IsName("down"))
        {
            textsDown.gameObject.SetActive(false);
         _cameraAnim.SetTrigger("goToMiddle");  

        }
        else if (_cameraAnim.GetCurrentAnimatorStateInfo(0).IsName("middleIdle"))
        {
            _cameraAnim.SetTrigger("goUp");  
            textsMiddle.gameObject.SetActive(false);
        }else if (_cameraAnim.GetCurrentAnimatorStateInfo(0).IsName("middleUp"))
        {
            StartCoroutine(GoToMenu());
        }
    }

    private IEnumerator GoToMenu()
    {
        fadePanelAnim.SetTrigger("doFadeIn");

        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene("MainMenu");
    }

    public void DisableButton()
    {
        continueButton.gameObject.SetActive(false);
    }
    
    public void EnableButton()
    {
        continueButton.gameObject.SetActive(true);
    }

    public void ShowMiddleText()
    {
        textsMiddle.gameObject.SetActive(true);
    }
    
    public void ShowLastText()
    {
        textsUp.gameObject.SetActive(true);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        textsDown.gameObject.SetActive(true);
        textsMiddle.gameObject.SetActive(false);
        textsUp.gameObject.SetActive(false);

        fadePanelAnim.SetTrigger("doFadeOut");

        _cameraAnim = Camera.main.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {


    }
}
