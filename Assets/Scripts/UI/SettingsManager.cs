using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{

    public static SettingsManager settingsInstance;
    
    [HideInInspector]
    public float generalMusicValue;
    [HideInInspector]
    public float soundEffectsValue;
    [HideInInspector]
    public float cmSensitivityValue;
    [HideInInspector]
    public int resolutionIdx;
    
    [HideInInspector]
    public int[] _widths = new[] {1920, 1280, 960, 568};
    [HideInInspector]
    public int[] _heights = new[] {1080, 800, 540, 329};
    
    // Start is called before the first frame update
    private void Awake()
    {
        
        if (!settingsInstance)
        {
            settingsInstance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            // sesitivity
            if (GameManager.instance)
            {
                GameManager.instance.ChangeSensitivity(settingsInstance.cmSensitivityValue);
            }
            
            // soundeffects
            AudioManager.audioInstance.soundEffectsFactor = settingsInstance.soundEffectsValue;
            
            //general music
            AudioManager.audioInstance.musicFactor = settingsInstance.generalMusicValue;

            // resolution
            int width = _widths[settingsInstance.resolutionIdx];
            int height = _heights[settingsInstance.resolutionIdx];
            Screen.SetResolution(width, height, true);
            
            Destroy(this);
        }
        
    }

    public void Start()
    {

         generalMusicValue = 1.0f;
         soundEffectsValue = 1.0f;
         cmSensitivityValue = 1.0f;
         resolutionIdx = 0;
    }



}
