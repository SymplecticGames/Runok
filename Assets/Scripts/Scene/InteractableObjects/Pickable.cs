using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pickable : MonoBehaviour
{
    [SerializeField]
    private bool respawnAfterTime;

    [SerializeField]
    private float respawnTime = 10.0f;

    [SerializeField]
    private UnityEvent action;

    private float activeRespawnTimer;

    private Collider trigger;

    private float _baseVolume;

    // Start is called before the first frame update
    void Start()
    {
        trigger = GetComponent<Collider>();
        
        _baseVolume = GetComponent<AudioSource>().volume;
    }

    // Update is called once per frame
    void Update()
    {
        if (respawnAfterTime && !trigger.enabled)
        {
            activeRespawnTimer += Time.deltaTime;

            if (activeRespawnTimer >= respawnTime)
            {
                EnablePickable(0.0f);

                // if pickable is a rune
                if (gameObject.CompareTag("Runa"))
                    GameManager.instance.respawnedPickedRune();
            }
        }
    }

    // Every pickable object should contain necessary info, like the index of the rune so it won't be instantiated when you reload the level, or the amount of energy it provides
    private void OnTriggerEnter(Collider other)
    {
        if (!enabled)
            return;

        if (other.CompareTag("Golem") || other.CompareTag("Beetle"))
        {
            GetComponent<AudioSource>().volume = _baseVolume * AudioManager.audioInstance.soundEffectsFactor;
            GetComponent<AudioSource>().Play();   
            
            action.Invoke();

            trigger.enabled = false; // We force it to be disabled before the delay
            DisablePickable(0.2f);

            activeRespawnTimer = 0.0f;

            // if pickable is a rune
            if (gameObject.CompareTag("Runa"))
            {
                GameManager.instance.pickedRune();
                AudioManager.audioInstance.StopCountDown();
            }
        }
    }

    public void EnablePickable(float delay)
    {
        StartCoroutine(EnablePickableDelayed(delay));
    }

    private IEnumerator EnablePickableDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Play particles/animation

        trigger.enabled = true;

        foreach (Renderer visualPart in GetComponentsInChildren<Renderer>())
            visualPart.enabled = true;
    }

    public void DisablePickable(float delay)
    {
        StartCoroutine(DisablePickableDelayed(delay));
    }

    private IEnumerator DisablePickableDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Play particles/animation

        trigger.enabled = false;

        foreach (Renderer visualPart in GetComponentsInChildren<Renderer>())
            visualPart.enabled = false;
    }
}
