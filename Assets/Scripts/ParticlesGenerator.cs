using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesGenerator : MonoBehaviour
{
    public static ParticlesGenerator instance;

    [SerializeField]
    private GameObject genericSmoke;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public void InstantiateParticles(Vector3 spawnPos, Color minColor, Color maxColor, float minSize = 0.5f, float maxSize = 1.0f, float startSpeed = 1.0f)
    {
        GameObject smokeObj = Instantiate(genericSmoke, spawnPos, Quaternion.identity);
        ParticleSystem smokeParticles = smokeObj.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = smokeParticles.main;
        ParticleSystem.MinMaxGradient startColor = new ParticleSystem.MinMaxGradient(minColor, maxColor);
        main.startColor = startColor;
        main.startSize = new ParticleSystem.MinMaxCurve(minSize, maxSize);
        main.startSpeed = startSpeed;

        smokeParticles.Play();
        StartCoroutine(DelayedDestroyParticles(smokeParticles));
    }

    private IEnumerator DelayedDestroyParticles(ParticleSystem particles)
    {
        yield return new WaitForSeconds(particles.main.duration);
        Destroy(particles.gameObject);
    }
}
