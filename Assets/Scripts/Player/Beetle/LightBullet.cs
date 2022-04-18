using System.Collections;
using UnityEngine;

public class LightBullet : MonoBehaviour
{
    // Bullet Variables
    private Vector3 direction;
    private float speed;
    private float lifeTime;
    private float damage;
    private bool despawned;

    private float elapsedTime;

    [SerializeField]
    private Color[] particlesColor;

    // Update is called once per frame
    void Update()
    {
        // Bullet movement
        transform.position += direction * speed * Time.deltaTime;

        if (!despawned)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > lifeTime)
            {
                SetInactive();
                elapsedTime = 0.0f;
            }
        }
    }

    public void SetDirection(Vector3 bulletDirection)
    {
        this.direction = bulletDirection;
    }

    public Vector3 GetDirection()
    {
        return direction;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetLifeTime(float lifeTime)
    {
        this.lifeTime = lifeTime;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetDespawned(bool despawned)
    {
        this.despawned = despawned;
        elapsedTime = 0.0f;
    }

    public void SetInactive()
    {
        despawned = true;

        // Spawn Particles
        ParticlesGenerator.instance.InstantiateParticles(transform.position, particlesColor[0], particlesColor[1], 0.1f, 0.3f, 0.3f);

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("LevelGeometry") || other.gameObject.layer == LayerMask.NameToLayer("Block"))
            SetInactive();

        if (CompareTag("BossBullet") && other.CompareTag("Beetle") && GameManager.instance.player.canTakeDamage)
        {
            SetInactive();
            GameManager.instance.player.Die();
        }
    }
}
