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

    // Start is called before the first frame update
    void Start()
    {
        trigger = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (respawnAfterTime && !trigger.enabled)
        {
            activeRespawnTimer += Time.deltaTime;

            if (activeRespawnTimer >= respawnTime)
            {
                foreach (Renderer visualPart in GetComponentsInChildren<Renderer>())
                    visualPart.enabled = true;

                // Play particles/animation

                trigger.enabled = true;
                
                // if pickable is a rune
                if (gameObject.CompareTag("Runa"))
                {
                    
                    GameManager.instance.respawnedPickedRune();
                    
                }
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
            trigger.enabled = false;

            action.Invoke();

            StartCoroutine(HidePickableDelayed(0.2f));
            
            // if pickable is a rune
            if (gameObject.CompareTag("Runa"))
            {

                GameManager.instance.pickedRune();
                
            }
        }
    }

    private IEnumerator HidePickableDelayed(float delay)
    {
        // Play particles/animation

        yield return new WaitForSeconds(delay);

        foreach (Renderer visualPart in GetComponentsInChildren<Renderer>())
            visualPart.enabled = false;

        activeRespawnTimer = 0.0f;
    }
}
