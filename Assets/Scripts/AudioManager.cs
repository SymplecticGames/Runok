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

public enum CharaudioTag
{
    genericJump = 0,
    terracottaStomp = 1,
    plumberStomp = 2,
    woodenStomp = 3,
    terracottaHit = 4,
    plumberHit = 5,
    woodenHit = 6,
}

public class AudioManager : MonoBehaviour
{
    [HideInInspector]
    public static AudioManager audioInstance;

    public bool allLoaded;

    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip[] clipsUI;
    [SerializeField]
    private AudioClip[] clipsChar;

    private string _soundEffectsUIPath;
    private string _soundEffectsCharPath;


    private void Awake()
    {
        audioInstance = this;

        // _soundEffectsUIPath = "SoundEffects/UI/";
        // _soundEffectsCharPath = "SoundEffects/Characters/";
        //
        // string[] namesUI = System.Enum.GetNames(typeof(UIaudioTag));
        // string[] namesChar = System.Enum.GetNames(typeof(CharaudioTag));
        
        // clipsUI = new AudioClip[namesUI.Length];
        // clipsChar = new AudioClip[namesChar.Length];

        // int i = 0;
        // foreach (var aTag in namesUI)
        // {
        //     clipsUI[i] = Resources.Load<AudioClip>("Assets/Resources/SoundEffects/UI/click.wav");
        //     Debug.Log(clipsUI[i]);
        //     i++;
        // }
        //
        //
        // i = 0;
        // foreach (var cTag in namesChar)
        // {
        //     clipsChar[i] = Resources.Load<AudioClip>(_soundEffectsCharPath + namesChar);
        //     i++;
        // }

        allLoaded = true;

        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.loop = false;
        _audioSource.playOnAwake = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayUISound(UIaudioTag audio)
    {
        _audioSource.loop = false;
        _audioSource.volume = 0.1f;
        _audioSource.clip = clipsUI[(int)audio];
        _audioSource.Play();
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
                case 1:
                    uiaudio = UIaudioTag.selectedTerracotta;
                    break;
                case 2:
                    uiaudio = UIaudioTag.selectedPlumber;
                    break;
                case 3:
                    uiaudio = UIaudioTag.selectedWooden;
                    break;
                default: break;
            }
        }
        else
        {
            switch (ability)
            {
                case 1:
                    uiaudio = UIaudioTag.selectedRadial;
                    break;
                case 2:
                    uiaudio = UIaudioTag.selectedLaser;
                    break;
                case 3:
                    uiaudio = UIaudioTag.selectedShoot;
                    break;
                default: break;
            }
        }
        _audioSource.clip = clipsUI[(int)uiaudio];
        _audioSource.Play();

    }
    

    public void PlayCharSound(CharaudioTag audio)
    {
        _audioSource.volume = 0.05f;
        _audioSource.clip = clipsChar[(int)audio];
        _audioSource.Play();
    }
    
    // Update is called once per frame
    void Update()
    {

    }
}
