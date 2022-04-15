using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InterludioManager : MonoBehaviour
{
    public Image skipButton;

    public List<Image> kbTags;
    public List<Image> xboxTags;
    public List<Image> psTags;

    public float damp = 0.5f;

    [SerializeField] public Sprite[] interludioImages;
    [SerializeField] public string[] interludioText;

    public Vector4 imagePositions_X;
    public GameObject finalText;

    public GameObject _imageGO;
    public Text _text;

    private int _actualSlide = 0;
    private float _targetPosition;
    private bool _submited;
    private bool _nextImageGroup;
    private bool _finalState;

    private bool _endedFadeIn;
    
    private AudioSource _interludioAS;

    // Start is called before the first frame update
    void Start()
    {
        _text.text = interludioText[0];
        _targetPosition = imagePositions_X[0];

        _interludioAS = GetComponent<AudioSource>();
        _interludioAS.loop = true;

        // transition
        StartCoroutine(WaitToPlay());
    }


    IEnumerator WaitToPlay()
    {
        GetComponent<Animator>().SetTrigger("doIdleDark");
        yield return new WaitForSeconds(1.0f);
        GetComponent<Animator>().SetTrigger("doFadeOut");
        // start music
        _interludioAS.clip = AudioManager.audioInstance.GetSoundTrackSound(SoundTrackAudioTag.interludios1);
        _interludioAS.Play();
        _endedFadeIn = true;
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        _submited = true;
    }


    public void OnDeviceChange(PlayerInput context)
    {
        if (!DeviceControlsManager.instance)
            return;

        DeviceControlsManager.instance.UpdateDeviceConnection(context);
        DeviceControlsManager.instance.SetTagsInScene(kbTags, xboxTags, psTags);
    }

    public void OnSkip(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        StartCoroutine(HighLightSkipButton());
    }

    IEnumerator HighLightSkipButton()
    {
        skipButton.color = new Color(0.384f, 0.1294118f, 0.1176471f);
        yield return new WaitForSeconds(0.5f);
        skipButton.color = Color.black;
        _actualSlide = interludioImages.Length * 3;
        _submited = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (_endedFadeIn)
        {
            if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle") || _finalState)
            {
                GetComponent<Animator>().ResetTrigger("doFadeOut");
                GetComponent<Animator>().ResetTrigger("doFadeIn");

                Vector3 pos = _imageGO.transform.localPosition;

                if (_submited)
                {
                    if (_finalState)
                    {
                        if (_interludioAS.loop)
                        {
                            _interludioAS.loop = false;
                            _interludioAS.clip = AudioManager.audioInstance.GetSoundTrackSound(SoundTrackAudioTag
                                .interludios3_endfinalText);
                            _interludioAS.Play();
                            StartCoroutine(LoadSceneDelay(_interludioAS.clip.length));
                        }
                    }
                    else
                    {
                        if (_actualSlide < interludioImages.Length * 3 - 1)
                        {
                            _actualSlide++;
                            var idx = _actualSlide % interludioImages.Length;

                            _targetPosition = imagePositions_X[idx];
                            _nextImageGroup = idx == 0;
                            if (!_nextImageGroup)
                                _text.text = interludioText[_actualSlide];
                        }
                        else
                        {
                            GetComponent<Animator>().SetTrigger("doFadeIn");
                            _finalState = true;
                            finalText.SetActive(true);
                            if (_interludioAS.loop)
                            {
                                _interludioAS.loop = false;
                                _interludioAS.clip =
                                    AudioManager.audioInstance.GetSoundTrackSound(SoundTrackAudioTag
                                        .interludios3_finalText_firstTime);
                                _interludioAS.Play();
                                StartCoroutine(WaitForLoop(_interludioAS.clip.length));
                            }
                        }
                        _submited = false;
                    }
                }

                if (Mathf.Abs(pos.x - _targetPosition) >= 0.01f && !_nextImageGroup)
                {
                    pos = Vector2.Lerp(pos, new Vector3(_targetPosition, pos.y, pos.z), Time.deltaTime * damp);
                    _imageGO.transform.localPosition = pos;
                }

                if (_nextImageGroup)
                {
                    // ---------------------------------- Fade in transition ---------------------------------- //
                    GetComponent<Animator>().SetTrigger("doFadeIn");
                    _nextImageGroup = false;
                    // ---------------------------------------------------------------------------------------- //
                }
            }
            else
            {
                if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idleDark") && _actualSlide < interludioImages.Length * 3 - 1)
                {
                    GetComponent<Animator>().SetTrigger("doFadeOut");
                    _text.text = interludioText[_actualSlide];
                    _targetPosition = imagePositions_X[0];
                    var pos = _imageGO.transform.localPosition;
                    pos = new Vector3(_targetPosition, pos.y, pos.z);
                    _imageGO.transform.localPosition = pos;
                    _imageGO.GetComponent<Image>().sprite = interludioImages[_actualSlide / interludioImages.Length];
                    
                    //change music playing
                    _interludioAS.clip =
                        AudioManager.audioInstance.GetSoundTrackSound(
                            (SoundTrackAudioTag) (_actualSlide / interludioImages.Length));
                    _interludioAS.Play();
                }
            }
        }
    }

    IEnumerator LoadSceneDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ProgressManager.instance.firstTime = false;
        SceneManager.LoadScene("Hub");
    }

    IEnumerator WaitForLoop(float delay)
    {
        yield return new WaitForSeconds(delay);
        _interludioAS.loop = true;
        _interludioAS.clip =
            AudioManager.audioInstance.GetSoundTrackSound(SoundTrackAudioTag
                .interludios3_finalText);
        _interludioAS.Play();
    }
}
