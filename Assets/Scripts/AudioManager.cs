using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum UIaudioTag
{
    click = 0,
    hover = 1,
    
    enterLevel = 2,
    exitHub = 3,
    
    openPauseMenu = 4,
    closePauseMenu = 5,
    showConfirmationPanel = 6,
    
    instructions = 7,
    
    openSelectionWheel = 8,
    hoverSelectionWheel = 9,
    selectedTerracotta = 10,
    selectedPlumber = 11,
    selectedWooden = 12,
    selectedRadial = 13,
    selectedShoot = 14,
    selectedLaser = 15,
    
}

public enum ObjaudioTag
{
    activation = 0,
    teleport = 1,

    pickParchment = 2,
    pickRune = 3,
    
    pressButton = 4,
    destroyBox = 5,
    
    laserBeam = 6,
    
    altar = 7,
    
}

public enum CharaudioTag
{
    beetleFlutter = 0,
    
    terracottaStomp = 1,
    plumberStomp = 2,
    woodenStomp = 3,    
    terracottaHit = 4,
    plumberHit = 5,
    woodenHit = 6,
    
    elMen = 7,
    
}

public class AudioManager : MonoBehaviour
{
    [HideInInspector]
    public static AudioManager audioInstance;

    private AudioSource _audioSource;
    
    private AudioClip[] clipsUI;
    private AudioClip[] clipsObj;
    private AudioClip[] clipsChar;
    
    private string _soundEffectsUIPath;
    private string _soundEffectsObjPath;
    private string _soundEffectsCharPath;
    
    
    private void Awake()
    {
        audioInstance = this;

        _soundEffectsUIPath = Application.dataPath + "/Sounds/SoundEffects/UI/";
        _soundEffectsObjPath = Application.dataPath + "/Sounds/SoundEffects/Objects/";
        _soundEffectsCharPath = Application.dataPath + "/Sounds/SoundEffects/Characters/";

        string[] namesUI = System.Enum.GetNames(typeof(UIaudioTag));
        string[] namesObj = System.Enum.GetNames(typeof(ObjaudioTag));
        string[] namesChar = System.Enum.GetNames(typeof(CharaudioTag));
        
        
        clipsUI = new AudioClip[namesUI.Length];
        clipsObj = new AudioClip[namesObj.Length];
        clipsChar = new AudioClip[namesChar.Length];

        int i = 0;
        foreach (var aTag in namesUI)
        {
            StartCoroutine(LoadUIAudio(aTag, i));
            i++;
        }
        
        // i = 0;
        // foreach (var oTag in namesObj)
        // {
        //     StartCoroutine(LoadUIAudio(oTag, i));
        //      i++;
        // }
        //
        // i = 0;
        // foreach (var cTag in namesChar)
        // {
        //     StartCoroutine(LoadUIAudio(cTag, i));
        //      i++;
        // }

    }


    // Start is called before the first frame update
    void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
    }
    
    public void PlayUISound(UIaudioTag audio)
    {
        _audioSource.loop = false;
        _audioSource.playOnAwake = false;        
        _audioSource.clip = clipsUI[(int) audio];
        _audioSource.Play();
        
        // string audioName = System.Enum.GetName(typeof(UIaudioTag), audio);
        // switch (audio)
        // {
        //         case UIaudioTag.click:
        //             
        //             break;
        //         case UIaudioTag.hover: break;
        //
        //         case UIaudioTag.enterLevel: break;
        //         case UIaudioTag.exitHub: break;
        //         
        //         case UIaudioTag.openPauseMenu: break;
        //         case UIaudioTag.closePauseMenu: break;
        //         case UIaudioTag.showConfirmationPanel: break;
        //         
        //         case UIaudioTag.instructions: break;
        //         
        //         case UIaudioTag.openSelectionWheel: break;
        //         case UIaudioTag.hoverSelectionWheel: break;
        //         case UIaudioTag.selectedTerracotta: break;
        //         case UIaudioTag.selectedPlumber: break;
        //         case UIaudioTag.selectedWooden: break;
        //         case UIaudioTag.selectedRadial: break;
        //         case UIaudioTag.selectedShoot: break;
        //         case UIaudioTag.selectedLaser: break;
        //     
        // }
    }
    
    public void PlayAbilitySound(int ability, bool isGolem)
    {
        _audioSource.loop = false;
        _audioSource.playOnAwake = false;
        UIaudioTag uiaudio = UIaudioTag.selectedTerracotta;
        


        if (isGolem)
        {
            switch (ability)
            {
                case 1: uiaudio = UIaudioTag.selectedTerracotta;
                        break;
                case 2: uiaudio = UIaudioTag.selectedPlumber;
                        break;
                case 3: uiaudio = UIaudioTag.selectedWooden;
                        break;
                default: break;
            }
        }
        else
        {
            switch (ability)
            {
                case 1: uiaudio = UIaudioTag.selectedRadial;
                    break;
                case 2: uiaudio = UIaudioTag.selectedLaser;
                    break;
                case 3: uiaudio = UIaudioTag.selectedShoot;
                    break;
                default: break;
            }
        }
        _audioSource.clip = clipsUI[(int) uiaudio];
        _audioSource.Play();
        
    }

    // public void PlayObjSound(ObjaudioTag audio)
    // {
    //     string audioName = System.Enum.GetName(typeof(ObjaudioTag), audio);
    //     switch (audio)
    //     {
    //         case ObjaudioTag.activation: break;
    //         case ObjaudioTag.teleport: break;
    //         
    //         case ObjaudioTag.pickParchment: break;
    //         case ObjaudioTag.pickRune: break;
    //         
    //         case ObjaudioTag.pressButton: break;
    //         case ObjaudioTag.destroyBox: break;
    //
    //         case ObjaudioTag.laserBeam: break;
    //
    //         case ObjaudioTag.altar: break;
    //         
    //     }
    // }

    // public void PlayCharSound(CharaudioTag audio)
    // {
    //     string audioName = System.Enum.GetName(typeof(CharaudioTag), audio);
    //     switch (audio)
    //     {
    //         case CharaudioTag.beetleFlutter : break;
    //         
    //         case CharaudioTag.terracottaStomp : break;
    //         case CharaudioTag.plumberStomp : break;
    //         case CharaudioTag.woodenStomp : break;
    //         case CharaudioTag.terracottaHit : break;
    //         case CharaudioTag.plumberHit : break;
    //         case CharaudioTag.woodenHit:  break;
    //         
    //         case CharaudioTag.elMen:  break;
    //         
    //     }
    // }

    private IEnumerator LoadUIAudio(string audioName, int index)
    {

        WWW request = GetAudioFromFile(_soundEffectsUIPath, audioName);
        yield return request;

        
        clipsUI[index] = request.GetAudioClip();
        clipsUI[index].name = audioName;
    }

    private WWW GetAudioFromFile(string path, string filename)
    {
        string audioToLoad = string.Format(path + "{0}", filename + ".wav");
        WWW request = new WWW(audioToLoad);
        return request;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
