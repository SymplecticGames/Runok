using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewAbilityObtainedUI : MonoBehaviour
{

    public Text title;
    public Text subText;
    public Text golemText;
    public Text beetleText;
    public Button button;

    public GameObject parchment12;
    public GameObject parchment3;
    
    
    private string[] _golemAbility = {"Material de plomo", "Material de madera", "Material de terracota, plomo y madera"};
    private string[] _beetleAbility = {"Disparo de luz", "Impulso de luz", "Luz de área, disparo de luz e impulso de luz"};
    
    public void GoToHub()
    {
        if (SceneTransition.instance)
        {
            Cursor.visible = true;
            SceneTransition.instance.LoadScene("Hub");
        }
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(GoToHub);
        
        if (ProgressManager.instance.currentLevel < ProgressManager.instance.completedLevel.Length-1)
        {
            golemText.text = _golemAbility[ProgressManager.instance.currentLevel];
            beetleText.text = _beetleAbility[ProgressManager.instance.currentLevel];

            if (ProgressManager.instance.currentLevel < _golemAbility.Length - 1)
            {
                parchment12.gameObject.SetActive(true);
                parchment3.gameObject.SetActive(false);
                title.text = "¡Habilidades recuperadas!";
                subText.gameObject.SetActive(false);
            }
            else if (ProgressManager.instance.currentLevel == ProgressManager.instance.completedLevel.Length - 2)
            {
                parchment12.gameObject.SetActive(false);
                parchment3.gameObject.SetActive(true);
                title.text = "¡Instrucciones reparadas!";
                subText.gameObject.SetActive(true);

            }

            StartCoroutine(WaitForEndOfAnim());
        }
        else
        {
            // go to the end game scene
            Cursor.visible = true;
            SceneManager.LoadScene("EndLevel");
        }
    }

    IEnumerator WaitForEndOfAnim()
    {
        GetComponent<CanvasGroup>().alpha = 0.0f;
        yield return new WaitForSeconds(2.0f);
        GetComponent<CanvasGroup>().alpha = 1.0f;
        
    }
    public void ClickedContinue()
    {
        AudioManager.audioInstance.PlayUISound(UIAudioTag.click);
        Cursor.visible = true;
        SceneManager.LoadScene("Hub");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
