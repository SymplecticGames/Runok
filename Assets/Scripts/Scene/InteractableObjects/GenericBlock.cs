using System.Collections;
using UnityEngine;

public class GenericBlock : MonoBehaviour
{
    #region Generic_Variables

    [Space]
    [Header("Generic")]
    [SerializeField]
    private Transform cubeRespawn;
    private Rigidbody rb;
    private MeshRenderer rend;

    AudioSource audioSource;

    private Weight boxWeight;
    private float boxWeightBaseValue;

    [SerializeField]
    private bool startActive = false;

    #endregion

    #region Pushable_Variables

    [Space]
    [Header("Pushable")]
    [SerializeField]
    private float forceToApply;

    // Angles for constraint direction
    private float forwardAngle;
    private float leftAngle;
    private float backwardAngle;
    private float rightAngle;

    #endregion

    #region Breakable_Variables
    #endregion

    #region Extensible_Variables

    [Space]
    [Header("Extensible")]
    [SerializeField] private GameObject[] boxes;
    [SerializeField] private float growingFactor;
    [SerializeField][Range(1, 3)] private int initialStackBoxes;


    private Vector3[] initialBoxPos;
    private bool startGrowing;
    private float growingStep;
    private int currentBox;

    #endregion

    private AudioSource pushAudioS;

    private float _pushBaseVolume;
    private float _baseVolume;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
        boxWeight = GetComponent<Weight>();

        boxWeightBaseValue = boxWeight.weightValue;

        pushAudioS = gameObject.AddComponent<AudioSource>();
        pushAudioS.volume = 0.0f;
        pushAudioS.maxDistance = 10.0f;
        pushAudioS.spatialBlend = 1.0f;
        pushAudioS.rolloffMode = AudioRolloffMode.Linear;
        pushAudioS.loop = true;
        pushAudioS.clip = AudioManager.audioInstance.GetObjSound(ObjAudioTag.pushBox);

        _pushBaseVolume = 1.0f;
        _baseVolume = GetComponent<AudioSource>().volume;
        pushAudioS.Play();

        initialBoxPos = new Vector3[boxes.Length];

        for (int i = 0; i < initialBoxPos.Length; i++)
            initialBoxPos[i] = boxes[i].transform.localPosition;

        if (cubeRespawn && !startActive)
            ResetBlock();

        for (int i = 1; i < initialStackBoxes; i++)
            boxes[i].SetActive(true);

        currentBox = initialStackBoxes - 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Update box weight using the current extension of the block
        boxWeight.weightValue = boxWeightBaseValue * (currentBox + 1);

        // For lerping box positions in extensible mode
        if (startGrowing)
        {
            growingStep += Time.deltaTime * growingFactor;
            boxes[currentBox].transform.localPosition = Vector3.Lerp(Vector3.zero, initialBoxPos[currentBox], growingStep);

            if (growingStep >= 1.0f)
            {
                startGrowing = false;
                growingStep = 0.0f;
            }
        }

        if (Mathf.Abs(rb.velocity.y) >= 0.09f)
        {
            pushAudioS.volume = 0.0f;
        }
        else
        {
            Vector3 vel = rb.velocity;
            vel.y = 0.0f;
            pushAudioS.volume = Mathf.Clamp(vel.magnitude, 0.0f, 1.0f) * AudioManager.audioInstance.soundEffectsFactor;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled)
            return;

        if (!other.CompareTag("Hitter"))
            return;

        if (GameManager.instance.player.currentCharacter.TryGetComponent(out GolemBehaviour golem))
        {
            // set hittingAir to false
            golem.PlayHitSomethingSound();
            
            // Pushable
            if (golem.currentMaterial == GolemMaterial.Terracotta)
            {
                Vector3 offset = new Vector3(0.0f, -1.5f, -1.0f);
                ParticlesGenerator.instance.InstantiateParticles(transform.position + offset, Color.white, Color.gray, 0.1f, 0.5f);
                rb.AddForce(golem.transform.forward * forceToApply, ForceMode.Impulse);
            }
                

            // Breakable
            if (golem.currentMaterial == GolemMaterial.Plumber)
            {
                audioSource.clip = AudioManager.audioInstance.GetObjSound(ObjAudioTag.destroyBox);
                ParticlesGenerator.instance.InstantiateParticles(transform.position, Color.white, Color.gray, 2.0f, 3.0f, 4.0f);
                ResetBlock();
            }

            // Extensible
            if (golem.currentMaterial == GolemMaterial.Wooden && !startGrowing)
            {
                currentBox = (currentBox + 1) % boxes.Length;

                if (currentBox <= 0)
                {
                    for (int i = 1; i < initialBoxPos.Length; i++)
                        boxes[i].SetActive(false);
                    AudioManager.audioInstance.SetAudioSourcePitch(audioSource, 1.5f);
                    audioSource.clip = AudioManager.audioInstance.GetObjSound(ObjAudioTag.growBox);
                    audioSource.volume = _baseVolume * AudioManager.audioInstance.soundEffectsFactor;
                    audioSource.Play();
                    StartCoroutine(AudioManager.audioInstance.ResetPitch(audioSource, 0.2f));
                }
                else
                {
                    startGrowing = true;
                    boxes[currentBox].SetActive(true);
                    audioSource.clip = AudioManager.audioInstance.GetObjSound(ObjAudioTag.growBox);
                    GetComponent<AudioSource>().volume = _baseVolume * AudioManager.audioInstance.soundEffectsFactor;
                    audioSource.volume = _baseVolume * AudioManager.audioInstance.soundEffectsFactor;;
                    audioSource.Play();
                    
                }
            }
        }
        else
        {
            // Play sound of hitting a wall
        }
    }

    IEnumerator DestroyDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if (transform.childCount == 3)
            transform.GetChild(2).gameObject.SetActive(false);
        
        rend.enabled = false;
        if (cubeRespawn) transform.position = cubeRespawn.position;
    }

    public void ResetBlock()
    {
        audioSource.volume = _baseVolume * AudioManager.audioInstance.soundEffectsFactor;
        audioSource.Play();
        GetComponent<Collider>().enabled = false;
        rb.useGravity = false;

        StartCoroutine(DestroyDelay(0.0f/*audioSource.clip.length*/));
    }

    public void SpawnBlock()
    {
        GetComponent<Collider>().enabled = true;
        rb.useGravity = true;
        rend.enabled = true;
        transform.position = cubeRespawn.position;

        for (int i = 1; i < boxes.Length; i++)
            boxes[i].SetActive(false);
        
        for (int i = 1; i < initialStackBoxes; i++)
            boxes[i].SetActive(true);

        currentBox = initialStackBoxes - 1;

        if (transform.childCount == 3)
            transform.GetChild(2).gameObject.SetActive(true);
    }

}
