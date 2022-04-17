using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathTransitionUI : MonoBehaviour
{

    public Button button;
    public Text text;

    public void Reappear()
    {
        if (ProgressManager.instance)
        {
            switch (ProgressManager.instance.currentLevel)
            {
                case 0:
                    SceneTransition.instance.LoadScene("Level1");
                    break;
                case 1:
                    SceneTransition.instance.LoadScene("Level2");
                    break;
                case 2:
                    SceneTransition.instance.LoadScene("Level3");
                    break;
                case 3:
                    if (ProgressManager.instance.fightingBoss)
                        SceneTransition.instance.LoadScene("GolemBoss");
                    else
                        SceneTransition.instance.LoadScene("Level4");
                    break;
            }

            AudioManager.audioInstance.PlayUISound(UIAudioTag.click);
        }
    }

    public void HoverReappear()
    {
        AudioManager.audioInstance.PlayUISound(UIAudioTag.hover);
    }

    // Start is called before the first frame update
    void Start()
    {
        button.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
        StartCoroutine(WaitToShow());
    }

    IEnumerator WaitToShow()
    {
        yield return new WaitForSeconds(0.5f);
        button.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
