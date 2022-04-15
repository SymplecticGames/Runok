using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using Scene = UnityEditor.SearchService.Scene;

public class EndLevelUI : MonoBehaviour
{

    public Text golemText;
    public Text beetleText;
    public Button button;

    private string[] _golemAbility = {"Material de plomo", "Material de madera"};
    private string[] _beetleAbility = {"Disparo de luz", "Impulso de luz"};

    public void GoToHub()
    {
        Debug.Log("HOLA");
        SceneTransition.instance.LoadScene("Hub");
    }


    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(GoToHub);

        golemText.text = _golemAbility[ProgressManager.instance.currentLevel];
        beetleText.text = _beetleAbility[ProgressManager.instance.currentLevel];
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
