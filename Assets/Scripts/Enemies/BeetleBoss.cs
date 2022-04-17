using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct BeetleBossPattern
{
    public BeetleBossPattern(int waveSize, int cornerCount, float spawnRatio, float angleOffset, bool enableBezier)
    {
        this.waveSize = waveSize;
        this.cornerCount = cornerCount;
        this.spawnRatio = spawnRatio;
        this.angleOffset = angleOffset;
        this.enableBezier = enableBezier;
    }

    public int waveSize;
    public int cornerCount;
    public float spawnRatio;
    public float angleOffset;
    public bool enableBezier;
}

public class BeetleBoss : MonoBehaviour
{
    #region Boss_Bullet_Hell

    private BulletPool bulletPool;
    private float speedFactor = 2.0f;

    [Space]
    [Header("Bullet Hell Variables")]
    [SerializeField]
    private BeetleBossPattern[] bossPatterns;

    [SerializeField]
    private int hitsPerPattern;

    [SerializeField]
    private int lifeCount;

    private int currentBossPattern;
    private int hits;

    #endregion

    [Space]
    [SerializeField]
    private GameObject[] forceShields;
    private int currentShield;

    private float elapsedTime;

    private AudioSource audiosrc;
    private AudioClip spawnClip;
    private AudioClip damageClip;

    [Space]
    [SerializeField]
    AudioSource bossMusic;

    // Bezier Follow
    private BezierFollow bezier;

    private Animator anim;

    [HideInInspector]
    public bool isShooting;

    private bool alreadyDead;

    [SerializeField]
    private Color[] particlesColor;

    // Start is called before the first frame update
    void Start()
    {
        bulletPool = GetComponent<BulletPool>();
        bezier = GetComponentInParent<BezierFollow>();

        anim = GetComponentInChildren<Animator>();
        anim.SetBool("isBoss", true);

        GameManager.instance.player.SwapCharacter();
        GameManager.instance.player.currentCharacter.isInBoss = true;

        hits = hitsPerPattern;

        audiosrc = GetComponent<AudioSource>();
        spawnClip = (AudioClip)Resources.Load("SoundEffects/UI/selectedShoot");
        damageClip = (AudioClip)Resources.Load("SoundEffects/Characters/laserHit");
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 5.0f)
        {
            bossPatterns[0].angleOffset = -bossPatterns[0].angleOffset;
            elapsedTime = 0.0f;
        }

        if (bossPatterns[currentBossPattern].enableBezier)
            bezier.enabled = true;
        else
            bezier.enabled = false;
    }

    public void EnableBulletSpawn()
    {
        StartCoroutine(BulletSpawn());
        isShooting = true;
    }

    public void DisableBulletSpawn()
    {
        StopAllCoroutines();
    }

    public void DeactivateForceShield()
    {
        if(currentShield < forceShields.Length)
            StartCoroutine(DelayedForceShield());
    }

    private IEnumerator DelayedForceShield()
    {
        yield return new WaitForSeconds(1.0f);
        audiosrc.volume = AudioManager.audioInstance.soundEffectsFactor;
        audiosrc.Play();
        ParticlesGenerator.instance.InstantiateParticles(transform.position, particlesColor[0], particlesColor[1], 5.0f - currentShield, 8.0f - currentShield, 6.0f - currentShield);
        forceShields[currentShield].SetActive(false);
        currentShield++;

        if (currentShield == forceShields.Length)
        {
            yield return new WaitForSeconds(audiosrc.clip.length);
            bossMusic.Play();
            EnableBulletSpawn();
        }     
    }

    public IEnumerator BulletSpawn()
    {
        float offset = 0.0f;

        while (enabled)
        {
            audiosrc.clip = spawnClip;
            audiosrc.volume = AudioManager.audioInstance.soundEffectsFactor;
            audiosrc.Play();
            yield return new WaitForSeconds(bossPatterns[currentBossPattern].spawnRatio);

            float angle = offset;
            float angleStep = 360.0f / bossPatterns[currentBossPattern].waveSize;

            for (int i = 0; i < bossPatterns[currentBossPattern].waveSize; i++)
            {
                Vector3 bulletDirection = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
                float speedOffset = Mathf.Abs(Mathf.Sin(bossPatterns[currentBossPattern].cornerCount * angle * (Mathf.PI / 360.0f))) * speedFactor;

                bulletPool.SpawnBullet(bulletDirection, speedOffset);

                angle += angleStep;
            }

            offset += bossPatterns[currentBossPattern].angleOffset;
        }
    }

    private void GetDamage()
    {
        if (!isShooting)
            return;

        audiosrc.clip = damageClip;
        audiosrc.volume = AudioManager.audioInstance.soundEffectsFactor;
        audiosrc.Play();

        if(hits > 0)
            hits--;
        else
        {
            lifeCount--;
            if (lifeCount <= 0 && !alreadyDead)
                StartCoroutine(DelayedDeath());
            else
            {
                currentBossPattern++;

                hits = hitsPerPattern;
            }
        }

    }

    private IEnumerator DelayedDeath()
    {
        alreadyDead = true;

        // Death
        yield return new WaitForSeconds(1.0f);

        ParticlesGenerator.instance.InstantiateParticles(transform.position, particlesColor[0], particlesColor[1], 5.0f - currentShield, 8.0f - currentShield, 6.0f - currentShield);
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;

        yield return new WaitForSeconds(2.0f);

        // Fade and Next Scene
        GameManager.instance.player.fadePanelAnim.SetTrigger("doFadeIn");

        yield return new WaitForSeconds(0.2f);

        SceneManager.LoadScene("EndLevel");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            other.GetComponent<LightBullet>().SetInactive();
            GetDamage();
        }
    }
}
