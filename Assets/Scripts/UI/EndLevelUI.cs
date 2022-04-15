using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndLevelUI : MonoBehaviour
{
    public Text golemText;
    public Text beetleText;

    public Animator parchmentAnim;
    
    private string[] golem = new[] {"Material de plomo", "Material de madera"};
    private string[] beetle = new[] {"Disparo de luz", "Impulso de luz"};
    
    
    // Start is called before the first frame update
    void Start()
    {
        golemText.text = golem[ProgressManager.instance.currentLevel];
        beetleText.text = beetle[ProgressManager.instance.currentLevel];

        StartCoroutine(WaitForEndOfAnim());
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
        SceneManager.LoadScene("Hub");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
