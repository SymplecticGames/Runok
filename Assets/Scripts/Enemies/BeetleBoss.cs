using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private BeetleBossPattern spiralPattern;

    [SerializeField]
    private BeetleBossPattern circleBezier;

    [SerializeField]
    private BeetleBossPattern flowerPattern;

    private BeetleBossPattern currentBossPattern;

    #endregion

    private float elapsedTime;

    // Bezier Follow
    private BezierFollow bezier;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        bulletPool = GetComponent<BulletPool>();
        bezier = GetComponentInParent<BezierFollow>();

        // Bullet Spawn
        EnableBulletSpawn();

        anim = GetComponentInChildren<Animator>();
        anim.SetBool("isBoss", true);

        GameManager.instance.player.SwapCharacter();
        GameManager.instance.player.currentCharacter.isInBoss = true;
    }

    // Update is called once per frame
    void Update()
    {
        currentBossPattern = flowerPattern;
        bezier.enabled = currentBossPattern.enableBezier;

        elapsedTime += Time.deltaTime;
        if (elapsedTime > 5.0f)
        {
            spiralPattern.angleOffset = -spiralPattern.angleOffset;
            elapsedTime = 0.0f;
        }
    }

    public void EnableBulletSpawn()
    {
        StartCoroutine(BulletSpawn());
    }

    public void DisableBulletSpawn()
    {
        StopCoroutine(BulletSpawn());
    }

    private IEnumerator BulletSpawn()
    {
        float offset = 0.0f;

        while (true)
        {
            yield return new WaitForSeconds(currentBossPattern.spawnRatio);

            float angle = offset;
            float angleStep = 360.0f / currentBossPattern.waveSize;

            for (int i = 0; i < currentBossPattern.waveSize; i++)
            {
                Vector3 bulletDirection = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
                float speedOffset = Mathf.Abs(Mathf.Sin(currentBossPattern.cornerCount * angle * (Mathf.PI / 360.0f))) * speedFactor;

                bulletPool.SpawnBullet(bulletDirection, speedOffset);

                angle += angleStep;
            }

            offset += currentBossPattern.angleOffset;
        }
    }
}
