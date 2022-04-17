using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathCountdown : MonoBehaviour
{
    private Text text;

    private string dots = "...";

    private float countDownTimer = 5;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        countDownTimer -= Time.deltaTime;

        int number = Mathf.CeilToInt(countDownTimer);
        float decimalPart = number - countDownTimer;

        if (decimalPart >= 0.0f && decimalPart < 0.33f)
            dots = ".";
        else if (decimalPart >= 0.33f && decimalPart < 0.66f)
            dots = "..";
        else
            dots = "...";

        if (countDownTimer <= 0.0f)
            Reappear();
        else
            text.text = "Reapareciendo en " + number + dots;
    }

    private void Reappear()
    {
        AudioManager.audioInstance.PlayUISound(UIAudioTag.click);

        if (ProgressManager.instance)
        {
            Cursor.visible = false;
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

        }
    }
}
