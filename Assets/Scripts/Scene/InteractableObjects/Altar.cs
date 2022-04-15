using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Altar : MonoBehaviour
{
    public Color disabledColor;
    public Color enabledColor;
    public Color enteredColor;

    // GO reference to the altar's portal
    [SerializeField]
    private GameObject _altarPortal;

    [SerializeField]
    private GameObject _altarParchment;

    private Collider altarCollider;
    
    // action to do the Camera Pan
    [SerializeField]
    private UnityEvent action;

    private Color defaultColor;

    private Material mat;

    private float _baseVolume;
    
    //////////////////////////////////////////////////  p r o g r a m  /////////////////////////////////////////////////
    /// 
    // Start is called before the first frame update
    void Start()
    {
        mat = _altarPortal.GetComponent<MeshRenderer>().material;
        mat.color = disabledColor;
        _altarParchment.SetActive(false);
        altarCollider = GetComponent<Collider>();

        // get altar portal GOs
        disableAltar();

        defaultColor = mat.color;
        
        _baseVolume = GetComponent<AudioSource>().volume;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void enableAltar()
    {
        altarCollider.isTrigger = true;
        _altarParchment.SetActive(true);
        defaultColor = enabledColor;
        mat.color = defaultColor;
        action.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled)
            return;

        if (!other.CompareTag("Golem") && !other.CompareTag("Beetle"))
            return;

        mat.color = enteredColor;

        // End Level
        if (SceneManager.GetActiveScene().name == "Level1" && !ProgressManager.instance.completedLevel[0])
        {
            ProgressManager.instance.completedLevel[0] = true;
            ProgressManager.instance.currentCompletedLevels++;
        }
        if (SceneManager.GetActiveScene().name == "Level2" && !ProgressManager.instance.completedLevel[1])
        {
            ProgressManager.instance.completedLevel[1] = true;
            ProgressManager.instance.currentCompletedLevels++;
        }
        if (SceneManager.GetActiveScene().name == "Level3" && !ProgressManager.instance.completedLevel[2])
        {
            ProgressManager.instance.completedLevel[2] = true;
            ProgressManager.instance.currentCompletedLevels++;
        }
        if (SceneManager.GetActiveScene().name == "Level4" && !ProgressManager.instance.completedLevel[3])
        {
            ProgressManager.instance.completedLevel[3] = true;
            ProgressManager.instance.currentCompletedLevels++;
        }

        // Save game each time that an altar is triggered
        ProgressManager.instance.SaveGame();
        
        GetComponent<AudioSource>().volume = _baseVolume * AudioManager.audioInstance.soundEffectsFactor;
        GetComponent<AudioSource>().Play();
        SceneTransition.instance.LoadScene("NewAbilityObtained");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!enabled)
            return;

        mat.color = defaultColor;
    }

    public void disableAltar()
    {
        altarCollider.isTrigger = false;
    }
    
}
