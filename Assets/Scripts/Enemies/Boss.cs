using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    #region Boss_Bullet_Hell

    private BulletPool bulletPool;

    [Space]
    [Header("Bullet Hell Variables")]
    [SerializeField] private int waveSize;
    [SerializeField] private int cornerCount;
    [SerializeField] private float spawnRatio;
    [SerializeField] private float angleOffset;

    #endregion

    private float elapsedTime;

    // Bezier Follow
    //private BezierFollow bezier;

    // Start is called before the first frame update
    void Start()
    {
        // Bullet Spawn
        bulletPool = GetComponent<BulletPool>();
        EnableBulletSpawn();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ChangeBulletPattern(int waveSize, int cornerCount, float spawnRatio, float angleOffset)
    {
        this.waveSize = waveSize;
        this.cornerCount = cornerCount;
        this.spawnRatio = spawnRatio;
        this.angleOffset = angleOffset;
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
            yield return new WaitForSeconds(spawnRatio);

            float angle = offset;
            float angleStep = 360.0f / waveSize;

            for (int i = 0; i < waveSize; i++)
            {
                Vector3 bulletDirection = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
                float speedOffset = Mathf.Abs(Mathf.Sin(cornerCount * angle * (Mathf.PI / 360.0f)));

                bulletPool.SpawnBullet(bulletDirection, speedOffset);

                angle += angleStep;
            }

            offset += angleOffset;
        }
    }
}
