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
    
    startCountDown =16,
    countDown =17,
    endCountDown =18,

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
    punchAir = 7,
}

public enum ObjaudioTag
{
    pushBox = 0,
    destroyBox = 1,
    growBox = 2,
}

public class AudioManager : MonoBehaviour
{
    [HideInInspector]
    public static AudioManager audioInstance;

    public bool allLoaded;

    private AudioSource _audioSource;

    private AudioClip[] _clipsUI;
    private AudioClip[] _clipsChar;
    private AudioClip[] _clipsObj;

    private string _soundEffectsUIPath;
    private string _soundEffectsCharPath;
    private string _soundEffectsObjPath;

    private AudioSource _countDownAs;

    private void Awake()
    {
        audioInstance = this;

        _soundEffectsUIPath = "SoundEffects/UI/";
        _soundEffectsCharPath = "SoundEffects/Characters/";
        _soundEffectsObjPath = "SoundEffects/Objects/";
        
        string[] namesUI = System.Enum.GetNames(typeof(UIaudioTag));
        string[] namesChar = System.Enum.GetNames(typeof(CharaudioTag));
        string[] namesObj = System.Enum.GetNames(typeof(ObjaudioTag));
        
        _clipsUI = new AudioClip[namesUI.Length];
        _clipsChar = new AudioClip[namesChar.Length];
        _clipsObj = new AudioClip[namesObj.Length];

        int i = 0;
        foreach (var aTag in namesUI)
        {
            _clipsUI[i] = Resources.Load<AudioClip>(_soundEffectsUIPath + aTag);
            Debug.Log(_clipsUI[i]);
            i++;
        }
        
        
        i = 0;
        foreach (var cTag in namesChar)
        {
            _clipsChar[i] = Resources.Load<AudioClip>(_soundEffectsCharPath + cTag);
            i++;
        }
        
        i = 0;
        foreach (var oTag in namesObj)
        {
            _clipsObj[i] = Resources.Load<AudioClip>(_soundEffectsObjPath + oTag);
            i++;
        }

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
        _audioSource.clip = _clipsUI[(int)audio];
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
        _audioSource.clip = _clipsUI[(int)uiaudio];
        _audioSource.Play();

    }
    

    public void PlayCharSound(CharaudioTag audio)
    {
        _audioSource.volume = 0.05f;
        _audioSource.clip = _clipsChar[(int)audio];
        _audioSource.Play();
    }

    public void PlayCountDown(float endCountDown)
    {
        _countDownAs = gameObject.AddComponent<AudioSource>();
        _countDownAs.loop = false;
        _countDownAs.playOnAwake = false;
        StartCoroutine(CountDown(endCountDown));

    }

    IEnumerator CountDown(float endCountDown)
    {
        _countDownAs.clip = _clipsUI[(int)UIaudioTag.startCountDown];
        _countDownAs.Play();
        yield return new WaitForSeconds(_countDownAs.clip.length);
        
        _countDownAs.clip = _clipsUI[(int)UIaudioTag.countDown];
        _countDownAs.loop = true;
        _countDownAs.Play();
        yield return new WaitForSeconds(endCountDown);

        if (_countDownAs != null)
        {
            _countDownAs.loop = false;
            _countDownAs.clip = _clipsUI[(int) UIaudioTag.endCountDown];
            _countDownAs.Play();
            yield return new WaitForSeconds(_countDownAs.clip.length);
        }

    }
    
    public void StopCountDown()
    {
        if (_countDownAs != null)
        {
            Destroy(_countDownAs);
            
        }

    }

    IEnumerator DestroyCountDownDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(_countDownAs);
    }
    
    public AudioClip GetObjSound(ObjaudioTag audio)
    {
        return _clipsObj[(int)audio];
    }

    public void SetAudioSourcePitch(float pitch)
    {
        _audioSource.pitch = pitch;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
