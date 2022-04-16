using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingsUI : MonoBehaviour
{

    // music, sounds, cameraSensibility
    public List<Slider> sliders;

    // screen resolution
    public Dropdown screenResolution;
    
    // highlight options
    public Image generalMusic;
    public Image soundEffects;
    public Image sensitivity;
    public Image resolution;
    
    private float sensitivityFactor;

    public void Start()
    {
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        for (int i = 0; i < SettingsManager.settingsInstance._widths.Length; i++)
        {
            options.Add(new Dropdown.OptionData(SettingsManager.settingsInstance._widths[i] + "x" + SettingsManager.settingsInstance._heights[i]));
        }

        screenResolution.options = options;
        
        // // DEBUG /////////////////////////////////////////////////////////////////////////////////
        // Resolution[] resolutions = Screen.resolutions;
        //
        // // Print the resolutions
        // foreach (var res in resolutions)
        // {
        //     Debug.Log(res.width + "x" + res.height + " : " + res.refreshRate);
        // }
        // /////////////////////////////////////////////////////////////////////////////////
        
        
        // stablish setttings state:

        SetMusicVolume(SettingsManager.settingsInstance.generalMusicValue);
        sliders[0].value = SettingsManager.settingsInstance.generalMusicValue;
        
        SetSoundEffectsVolume(SettingsManager.settingsInstance.soundEffectsValue);
        sliders[1].value = SettingsManager.settingsInstance.soundEffectsValue;
        
        sensitivityFactor = SettingsManager.settingsInstance.cmSensitivityValue;
        SetCameraSensitivity(sensitivityFactor);
        sliders[2].value = SettingsManager.settingsInstance.cmSensitivityValue;
        
        SetScreenSize(SettingsManager.settingsInstance.resolutionIdx);
        screenResolution.value = SettingsManager.settingsInstance.resolutionIdx;

    }

    public void HoverGeneralMusic()
    {
        generalMusic.enabled = true;
        soundEffects.enabled = false;
        sensitivity.enabled = false;
        resolution.enabled = false;

        AudioManager.audioInstance.PlayUISound(UIAudioTag.hover);
    }

    public void HoverSoundEffect()
    {
        generalMusic.enabled = false;
        soundEffects.enabled = true;
        sensitivity.enabled = false;
        resolution.enabled = false;

        AudioManager.audioInstance.PlayUISound(UIAudioTag.hover);
    }

    public void HoverSensitivity()
    {
        generalMusic.enabled = false;
        soundEffects.enabled = false;
        sensitivity.enabled = true;
        resolution.enabled = false;

        AudioManager.audioInstance.PlayUISound(UIAudioTag.hover);
    }

    public void HoverResolution()
    {
        generalMusic.enabled = false;
        soundEffects.enabled = false;
        sensitivity.enabled = false;
        resolution.enabled = true;

        AudioManager.audioInstance.PlayUISound(UIAudioTag.hover);
    }

    public void HoverGoBack()
    {
        generalMusic.enabled = false;
        soundEffects.enabled = false;
        sensitivity.enabled = false;
        resolution.enabled = false;

        AudioManager.audioInstance.PlayUISound(UIAudioTag.hover);
    }

    public void SetScreenSize(int index)
    {
        int width = SettingsManager.settingsInstance._widths[index];
        int height = SettingsManager.settingsInstance._heights[index];
        Screen.SetResolution(width, height, true);
        if (SettingsManager.settingsInstance)
            SettingsManager.settingsInstance.resolutionIdx = index;
    }

    public void SetMusicVolume(float value)
    {

        AudioManager.audioInstance.SetGeneralMusicVolume(value);
        if (SettingsManager.settingsInstance)
        {
            SettingsManager.settingsInstance.generalMusicValue = value;
        }
    }

    public void SetSoundEffectsVolume(float value)
    {
        AudioManager.audioInstance.soundEffectsFactor = value;
        if (SettingsManager.settingsInstance)
            SettingsManager.settingsInstance.soundEffectsValue = value;
    }

    public void SetCameraSensitivity(float value)
    {
        // 0--> 2
        sensitivityFactor = Mathf.Clamp(value, 0.1f, 2.0f);


        if (GameManager.instance)
        {
            GameManager.instance.ChangeSensitivity(sensitivityFactor);
        }
        
        if (SettingsManager.settingsInstance)
            SettingsManager.settingsInstance.cmSensitivityValue = sensitivityFactor;

    }

    public void GoBack()
    {
        AudioManager.audioInstance.PlayUISound(UIAudioTag.click);

    }
    

}