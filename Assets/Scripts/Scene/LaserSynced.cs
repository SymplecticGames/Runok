using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSynced : MonoBehaviour
{
    private GameObject extensible;

    private List<AudioSource> audioSources;

    [SerializeField]
    private float startOfCycleOffset;

    [SerializeField]
    private float laserOnDuration = 1.0f;

    [SerializeField]
    private float laserOffDuration = 1.0f;

    [SerializeField]
    private bool initialLaserState = true;

    private float lastCycleEndTime = 0.0f;

    private float cycleDuration;

    private bool currentLaserState;

    private bool isInCycle;

    // Start is called before the first frame update
    void Start()
    {
        extensible = transform.GetChild(0).gameObject;
        audioSources = new List<AudioSource>(GetComponentsInChildren<AudioSource>());

        cycleDuration = laserOffDuration + laserOnDuration;

        currentLaserState = initialLaserState;

        StartCoroutine(StartCycleDelayed(startOfCycleOffset));
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInCycle)
            return;

        // Full cycle
        if (Time.time - lastCycleEndTime < cycleDuration)
        {
            // End of first half of cycle
            if (Time.time - lastCycleEndTime > (initialLaserState ? laserOnDuration : laserOffDuration))
                EnableLaser(!initialLaserState);
        }
        else
        {
            // Start new cycle
            lastCycleEndTime = Time.time;

            EnableLaser(initialLaserState);
        }
    }

    private void EnableLaser(bool enabled)
    {
        if (currentLaserState == enabled)
            return;

        currentLaserState = enabled;

        extensible.SetActive(enabled);

        foreach (AudioSource audio in audioSources)
            audio.enabled = enabled;
    }

    private IEnumerator StartCycleDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Start new cycle
        lastCycleEndTime = Time.time;
        isInCycle = true;
    }
}
