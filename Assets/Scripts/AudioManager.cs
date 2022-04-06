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
    beetleFlutter = 0,

    terracottaStomp = 1,
    plumberStomp = 2,
    woodenStomp = 3,
    terracottaHit = 4,
    plumberHit = 5,
    woodenHit = 6,

    // elMen = 7,

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
    private string _soundEffectsObjPath;
    private string _soundEffectsCharPath;


    private void Awake()
    {
        audioInstance = this;

        _soundEffectsUIPath = Application.dataPath + "/Sounds/SoundEffects/UI/";
        _soundEffectsCharPath = Application.dataPath + "/Sounds/SoundEffects/Characters/";

        string[] namesUI = System.Enum.GetNames(typeof(UIaudioTag));
        string[] namesChar = System.Enum.GetNames(typeof(CharaudioTag));


        //clipsUI = new AudioClip[namesUI.Length];
        //clipsChar = new AudioClip[namesChar.Length];

        //int i = 0;
        //foreach (var aTag in namesUI)
        //{
        //    StartCoroutine(LoadUIAudio(aTag, i, 1));
        //    i++;
        //}
        

        //i = 0;
        //foreach (var cTag in namesChar)
        //{
        //    StartCoroutine(LoadUIAudio(cTag, i, 3));
        //    i++;
        //}

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

    private IEnumerator LoadUIAudio(string audioName, int index, int tagIdx)
    {


        WWW request;

        switch (tagIdx)
        {
            case 1: // UI
                request = GetAudioFromFile(_soundEffectsUIPath, audioName);
                yield return request;
                clipsUI[index] = request.GetAudioClip();
                clipsUI[index].name = audioName;
                break;
            case 3: // char
                request = GetAudioFromFile(_soundEffectsCharPath, audioName);
                yield return request;
                clipsChar[index] = request.GetAudioClip();
                clipsChar[index].name = audioName;
                break;
        }

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
