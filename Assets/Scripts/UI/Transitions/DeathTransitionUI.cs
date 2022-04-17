using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathTransitionUI : MonoBehaviour
{
    public Text text;

    public void HoverReappear()
    {
        AudioManager.audioInstance.PlayUISound(UIAudioTag.hover);
    }

    // Start is called before the first frame update
    void Start()
    {
        text.gameObject.SetActive(false);
        StartCoroutine(WaitToShow());
    }

    IEnumerator WaitToShow()
    {
        yield return new WaitForSeconds(0.5f);
        text.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
