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
    [SerializeField]
    private GameObject[] boxes;
    private Vector3[] initialBoxPos;

    [SerializeField]
    private float growingFactor;

    private bool startGrowing;
    private float growingStep;
    private int currentBox;

    #endregion

    private AudioSource pushAudioS;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();

        pushAudioS = gameObject.AddComponent<AudioSource>();
        pushAudioS.volume = 0.0f;
        pushAudioS.loop = true;
        pushAudioS.clip = AudioManager.audioInstance.GetObjSound(ObjaudioTag.pushBox);
        pushAudioS.Play();
        

        initialBoxPos = new Vector3[boxes.Length];

        for (int i = 0; i < initialBoxPos.Length; i++)
            initialBoxPos[i] = boxes[i].transform.localPosition;

        if (cubeRespawn && !startActive)
            ResetBlock();
    }

    // Update is called once per frame
    void Update()
    {
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
            pushAudioS.volume = Mathf.Clamp(vel.magnitude, 0.0f, 1.0f);
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
                rb.AddForce(golem.transform.forward * forceToApply, ForceMode.Impulse);

            // Breakable
            if (golem.currentMaterial == GolemMaterial.Plumber)
            {
                audioSource.clip = AudioManager.audioInstance.GetObjSound(ObjaudioTag.destroyBox);
                // Cute break method
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
                    audioSource.clip = AudioManager.audioInstance.GetObjSound(ObjaudioTag.growBox);
                    audioSource.Play();
                    StartCoroutine(AudioManager.audioInstance.ResetPitch(audioSource, 0.2f));
                }
                else
                {
                    startGrowing = true;
                    boxes[currentBox].SetActive(true);
                    audioSource.clip = AudioManager.audioInstance.GetObjSound(ObjaudioTag.growBox);
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

        rend.enabled = false;
        if (cubeRespawn) transform.position = cubeRespawn.position;
    }

    public void ResetBlock()
    {
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
    }

}
