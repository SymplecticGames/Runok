using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<MeshRenderer>();

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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled)
            return;

        if (!other.CompareTag("Hitter"))
            return;

        if (GameManager.instance.player.currentCharacter.TryGetComponent(out GolemBehaviour golem))
        {
            // Pushable
            if (golem.currentMaterial == GolemMaterial.Terracotta)
                rb.AddForce(golem.transform.forward * forceToApply, ForceMode.Impulse);

            // Breakable
            if (golem.currentMaterial == GolemMaterial.Plumber)
            {
                // Cute break method
                if (cubeRespawn)
                    ResetBlock();
                else
                {
                    StartCoroutine(destroyDelay());
                }
            }

            // Extensible
            if (golem.currentMaterial == GolemMaterial.Wooden && !startGrowing)
            {
                currentBox = (currentBox + 1) % boxes.Length;

                if (currentBox <= 0)
                {
                    for (int i = 1; i < initialBoxPos.Length; i++)
                        boxes[i].SetActive(false);
                }
                else
                {
                    startGrowing = true;
                    boxes[currentBox].SetActive(true);
                }
            }
        }
        else
        {
            // Play sound of hitting a wall
        }
    }

    IEnumerator destroyDelay()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(gameObject);
    }
    
    public void ResetBlock()
    {
        GetComponent<Collider>().enabled = false;
        rb.useGravity = false;
        rend.enabled = false;
        transform.position = cubeRespawn.position;
    }

    public void SpawnBlock()
    {
        GetComponent<Collider>().enabled = true;
        rb.useGravity = true;
        rend.enabled = true;
        transform.position = cubeRespawn.position;
    }

}
