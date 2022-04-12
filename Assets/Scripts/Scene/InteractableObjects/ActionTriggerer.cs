using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionTriggerer : MonoBehaviour
{
    [SerializeField]
    public List<string> triggererTags;

    [SerializeField]
    private UnityEvent onHitAction;

    [SerializeField]
    private UnityEvent onReleaseAction;

    [HideInInspector]
    public bool isEnabled = true;

    private float _baseVolume;
    
    // Start is called before the first frame update
    void Start()
    {
        _baseVolume = GetComponent<AudioSource>().volume;

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled)
            return;

        if (triggererTags.Contains(other.tag) && isEnabled)
        {
            onHitAction.Invoke();
            GetComponent<AudioSource>().volume = _baseVolume * AudioManager.audioInstance.soundEffectsFactor;
            GetComponent<AudioSource>().Play();

            if (other.CompareTag("Hitter") &&
                GameManager.instance.player.currentCharacter.TryGetComponent(out GolemBehaviour golem))
            {
                // set hittingAir to false
                golem.PlayHitSomethingSound();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!enabled)
            return;

        if (triggererTags.Contains(other.tag) && isEnabled)
            onReleaseAction.Invoke();
    }

    public void Enable(float delay)
    {
        StartCoroutine(EnableDelayed(delay));
    }

    private IEnumerator EnableDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        isEnabled = true;
    }

    public void Disable(float delay)
    {
        StartCoroutine(DisableDelayed(delay));
    }

    private IEnumerator DisableDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        isEnabled = false;
    }
}
