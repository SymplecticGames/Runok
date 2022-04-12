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
    public float waitSecToAutoChange = 60f;

    [SerializeField] public Sprite[] interludioImages;
    [SerializeField] public string[] interludioText;

    public Vector4 imagePositions_X;
    public GameObject finalText;

    private GameObject _fadePanel;
    private GameObject _imageGO;
    private Text _text;

    private float _counter = 0.0f;
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
        _fadePanel = gameObject.transform.GetChild(1).gameObject;
        _imageGO = gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
        _text = gameObject.transform.GetChild(0).gameObject.transform.GetChild(3).GetComponent<Text>();
        _text.text = interludioText[0];
        _targetPosition = imagePositions_X[0];
        
        if (!DeviceControlsManager.instance)
            return;

        DeviceControlsManager.instance.SetTagsInScene(kbTags, xboxTags, psTags);

        _interludioAS = GetComponent<AudioSource>();

        // transition
        StartCoroutine(WaitToPlay());
    }


    IEnumerator WaitToPlay()
    {
        _fadePanel.GetComponent<Animator>().SetTrigger("doIdleDark");
        yield return new WaitForSeconds(1.0f);
        _fadePanel.GetComponent<Animator>().SetTrigger("doFadeOut");
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
        if (!DeviceControlsManager.instance) return;

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
            if (_fadePanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle") || _finalState)
            {
                _fadePanel.GetComponent<Animator>().ResetTrigger("doFadeOut");
                _fadePanel.GetComponent<Animator>().ResetTrigger("doFadeIn");

                _counter += Time.deltaTime;
                Vector3 pos = _imageGO.transform.localPosition;

                if (_submited || _counter >= waitSecToAutoChange)
                {
                    if (_finalState)
                    {

                        if (_interludioAS.loop)
                        {
                            _interludioAS.clip =
                                AudioManager.audioInstance.GetSoundTrackSound(SoundTrackAudioTag
                                    .interludios3_endfinalText);
                            _interludioAS.Play();
                            _interludioAS.loop = false;
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
                            _text.text = interludioText[_actualSlide];
                            _nextImageGroup = idx == 0;
                        }
                        else
                        {

                            _fadePanel.GetComponent<Animator>().SetTrigger("doFadeIn");
                            _finalState = true;
                            finalText.SetActive(true);
                            _interludioAS.clip =
                                AudioManager.audioInstance.GetSoundTrackSound(SoundTrackAudioTag
                                    .interludios3_finalText);
                            _interludioAS.Play();
                            _interludioAS.loop = true;
                        }

                        _counter = 0.0f;
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
                    _fadePanel.GetComponent<Animator>().SetTrigger("doFadeIn");
                    _counter = 0.0f;
                    _nextImageGroup = false;
                    // -------------------------------------------------------------------------------------------------- //
                }
            }
            else
            {
                if (_fadePanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idleDark"))
                {
                    _text.text = interludioText[_actualSlide];
                    _targetPosition = imagePositions_X[0];
                    var pos = _imageGO.transform.localPosition;
                    pos = new Vector3(_targetPosition, pos.y, pos.z);
                    _imageGO.transform.localPosition = pos;
                    _imageGO.GetComponent<Image>().sprite = interludioImages[_actualSlide / interludioImages.Length];
                    _fadePanel.GetComponent<Animator>().SetTrigger("doFadeOut");

                    //change music playing
                    _interludioAS.clip =
                        AudioManager.audioInstance.GetSoundTrackSound(
                            (SoundTrackAudioTag) (_actualSlide / interludioImages.Length));
                    _interludioAS.Play();
                }

                _counter = 0.0f;
            }
        }

    }

    IEnumerator LoadSceneDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ProgressManager.instance.firstTime = false;
        SceneManager.LoadScene("Hub");
    }
}
