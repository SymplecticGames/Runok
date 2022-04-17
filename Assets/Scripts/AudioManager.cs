using System.Collections;
using UnityEngine;

public enum UIAudioTag
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

public enum CharAudioTag
{
    genericJump = 0,
    terracottaStomp = 1,
    plumberStomp = 2,
    woodenStomp = 3,
    terracottaHit = 4,
    plumberHit = 5,
    woodenHit = 6,
    punchAir = 7,
    
    enemyHit = 8,
    enemyIdle = 9,
    
    laserHit = 10,
}

public enum ObjAudioTag
{
    pushBox = 0,
    destroyBox = 1,
    growBox = 2,
    
    brokenPlatform = 3,
    crackedPlatform = 4,
    
    laserBeam =5,
}

public enum SoundTrackAudioTag
{
    interludios1 = 0,
    interludios2 = 1,
    interludios3 = 2,
    interludios3_finalText = 3,
    interludios3_finalText_firstTime = 4,
    interludios3_endfinalText = 5,
    desert = 6,

}

public class AudioManager : MonoBehaviour
{
    [HideInInspector]
    public static AudioManager audioInstance;

    [HideInInspector]
    public float soundEffectsFactor = 1.0f;
    
    [HideInInspector]
    public float musicFactor = 1.0f;
    
    public bool allLoaded;

    public AudioSource beetleAudioSource;
    private float _beetleAsBaseVolume;
    
    private AudioSource _audioSource;

    private AudioClip[] _clipsUI;
    private AudioClip[] _clipsChar;
    private AudioClip[] _clipsObj;
    private AudioClip[] _clipsSoundTrack;

    private string _soundEffectsUIPath;
    private string _soundEffectsCharPath;
    private string _soundEffectsObjPath;
    private string _soundTrackPath;

    private AudioSource _countDownAs;

    private void Awake()
    {
        audioInstance = this;

        _soundEffectsUIPath = "SoundEffects/UI/";
        _soundEffectsCharPath = "SoundEffects/Characters/";
        _soundEffectsObjPath = "SoundEffects/Objects/";
        _soundTrackPath = "SoundTracks/";
        
        string[] namesUI = System.Enum.GetNames(typeof(UIAudioTag));
        string[] namesChar = System.Enum.GetNames(typeof(CharAudioTag));
        string[] namesObj = System.Enum.GetNames(typeof(ObjAudioTag));
        string[] namesSoundTrack = System.Enum.GetNames(typeof(SoundTrackAudioTag));
        
        _clipsUI = new AudioClip[namesUI.Length];
        _clipsChar = new AudioClip[namesChar.Length];
        _clipsObj = new AudioClip[namesObj.Length];
        _clipsSoundTrack = new AudioClip[namesSoundTrack.Length];

        int i = 0;
        foreach (var aTag in namesUI)
        {
            _clipsUI[i] = Resources.Load<AudioClip>(_soundEffectsUIPath + aTag);
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
        
        i = 0;
        foreach (var sTag in namesSoundTrack)
        {
            _clipsSoundTrack[i] = Resources.Load<AudioClip>(_soundTrackPath + sTag);
            i++;
        }

        allLoaded = true;

        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.loop = false;
        _audioSource.playOnAwake = false;
        
        if (beetleAudioSource)
            _beetleAsBaseVolume = beetleAudioSource.volume;
    }


    // Start is called before the first frame update
    void Start()
    {
        if (beetleAudioSource)
        {
            beetleAudioSource.volume =  _beetleAsBaseVolume * soundEffectsFactor;
        }
    }

    public void ChangeBeetleVolume()
    {
        if (beetleAudioSource)
        {
            beetleAudioSource.volume =  _beetleAsBaseVolume * soundEffectsFactor;
        }
    }

    public void PlayUISound(UIAudioTag audio)
    {
        _audioSource.loop = false;
        _audioSource.volume = 0.1f * soundEffectsFactor;
        _audioSource.clip = _clipsUI[(int)audio];
        _audioSource.Play();
    }

    public void PlayAbilitySound(int ability, bool isGolem)
    {
        _audioSource.loop = false;
        _audioSource.playOnAwake = false;
        UIAudioTag uiaudio = UIAudioTag.selectedTerracotta;



        if (isGolem)
        {
            switch (ability)
            {
                case 1:
                    uiaudio = UIAudioTag.selectedTerracotta;
                    break;
                case 2:
                    uiaudio = UIAudioTag.selectedPlumber;
                    break;
                case 3:
                    uiaudio = UIAudioTag.selectedWooden;
                    break;
                default: break;
            }
        }
        else
        {
            switch (ability)
            {
                case 1:
                    uiaudio = UIAudioTag.selectedRadial;
                    break;
                case 2:
                    uiaudio = UIAudioTag.selectedShoot;
                    break;
                case 3:
                    uiaudio = UIAudioTag.selectedLaser;
                    break;
                default: break;
            }
        }
        _audioSource.volume = 0.1f * soundEffectsFactor;
        _audioSource.clip = _clipsUI[(int)uiaudio];
        _audioSource.Play();

    }
    

    public void PlayCharSound(CharAudioTag audio)
    {
        _audioSource.volume = 0.05f * soundEffectsFactor;
        _audioSource.clip = _clipsChar[(int)audio];
        _audioSource.Play();
    }

    public void PlayCountDown(float endCountDown)
    {
        _countDownAs = gameObject.AddComponent<AudioSource>();
        _countDownAs.loop = false;
        _countDownAs.playOnAwake = false;
        _countDownAs.volume = 1.0f * soundEffectsFactor;
        StartCoroutine(CountDown(endCountDown));

    }

    IEnumerator CountDown(float endCountDown)
    {
        _countDownAs.clip = _clipsUI[(int)UIAudioTag.startCountDown];
        _countDownAs.Play();
        yield return new WaitForSeconds(_countDownAs.clip.length);
        
        _countDownAs.clip = _clipsUI[(int)UIAudioTag.countDown];
        _countDownAs.loop = true;
        _countDownAs.Play();
        yield return new WaitForSeconds(endCountDown);

        if (_countDownAs != null)
        {
            _countDownAs.loop = false;
            _countDownAs.clip = _clipsUI[(int) UIAudioTag.endCountDown];
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
    
    public AudioClip GetObjSound(ObjAudioTag audio)
    {
        return _clipsObj[(int)audio];
    }
    
    public AudioClip GetCharSound(CharAudioTag audio)
    {
        return _clipsChar[(int)audio];
    }
    
    public AudioClip GetSoundTrackSound(SoundTrackAudioTag audio)
    {
        return _clipsSoundTrack[(int)audio];
    }

    public void SetAudioSourcePitch(AudioSource aS, float pitch)
    {
        aS.pitch = pitch;
    }
    public IEnumerator ResetPitch(AudioSource aS, float delay)
    {
        yield return new WaitForSeconds(delay);
        SetAudioSourcePitch(aS, 1.0f);
    }

    public AudioSource GetAudioSource()
    {
        return _audioSource;
    }

    public void PlayLaserBeamSound(AudioSource laserBeamAs)
    {
        laserBeamAs.mute = false;
        laserBeamAs.volume = 1.0f * soundEffectsFactor;
        laserBeamAs.loop = true;
        laserBeamAs.clip = GetObjSound(ObjAudioTag.laserBeam);
        laserBeamAs.Play();
    }
    public void StopLaserBeamSound(AudioSource laserBeamAs)
    {
        laserBeamAs.mute = true;
    }
    
    public void PlayLaserHitSound()
    {
        AudioSource laserAs = gameObject.AddComponent<AudioSource>();
        laserAs.volume = 1.0f * soundEffectsFactor;
        laserAs.loop = false;
        laserAs.clip = GetCharSound(CharAudioTag.laserHit);
        laserAs.Play();
        DestroyLaserAS(laserAs);
    }

    IEnumerator DestroyLaserAS(AudioSource aS)
    {
        yield return new WaitForSeconds(GetCharSound(CharAudioTag.laserHit).length);
        Destroy(aS);
    }

    public void SetGeneralMusicVolume(float newMusicFactor)
    {

        musicFactor = newMusicFactor;        

        if (GameManager.instance)
        {
            GameManager.instance.musicBaseVolume = musicFactor;
            GameManager.instance.targetMusicBaseVolume = 0.05f * musicFactor;
            GameManager.instance.musicSource.volume *= musicFactor;
        }
    }
    
    // Update is called once per frame
    void Update()
    {

    }
}
