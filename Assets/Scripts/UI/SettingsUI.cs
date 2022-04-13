using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{

    public static SettingsUI settingsInstance;
    
    [HideInInspector]
    public float _cmSensitivityFactor = 1.0f;
    
    // music, sounds, cameraSensibility
    public List<Slider> sliders;

    // screen resolution
    public Dropdown screenResolution;
    
    // resolutions
    private int[] _widths = new[] {1920, 1280, 960, 568};
    private int[] _heights = new[] {1080, 800, 540, 329};

    // Start is called before the first frame update
    void Start()
    {

        if (!settingsInstance)
        {
            settingsInstance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
        
        //set resolution text:
        if (settingsInstance)
        {
            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            for (int i = 0; i < _widths.Length; i++)
            {
                options.Add(new Dropdown.OptionData(_widths[i] + "x" + _heights[i]));
            }

            screenResolution.options = options;
        }

        // // DEBUG /////////////////////////////////////////////////////////////////////////////////
        // Resolution[] resolutions = Screen.resolutions;
        //
        // // Print the resolutions
        // foreach (var res in resolutions)
        // {
        //     Debug.Log(res.width + "x" + res.height + " : " + res.refreshRate);
        // }
        // /////////////////////////////////////////////////////////////////////////////////
    }
    
    
    public void SetScreenSize(int index)
    {
        if (settingsInstance)
        {
            int width = _widths[index];
            int height = _heights[index];
            Screen.SetResolution(width, height, true);
        }
    }

    public void SetMusicVolume(float value)
    {
        AudioManager.audioInstance.musicFactor = value;
    }

    public void SetSoundEffectsVolume(float value)
    {
        AudioManager.audioInstance.soundEffectsFactor = value;
    }

    public void SetCameraSensitivity(float value)
    {
        // 0--> 2
        _cmSensitivityFactor = Mathf.Clamp(value, 0.1f, 2.0f);


        if (GameManager.instance)
        {
            GameManager.instance.ChangeSensitivity(_cmSensitivityFactor);
        }

    }

    public void GoBack()
    {
        AudioManager.audioInstance.PlayUISound(UIAudioTag.click);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
