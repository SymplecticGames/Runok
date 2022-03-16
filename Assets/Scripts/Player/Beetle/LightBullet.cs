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

    // Update is called once per frame
    void Update()
    {
        // Bullet movement
        transform.position += direction * speed * Time.deltaTime;

        if(!despawned)
            StartCoroutine(SetInactiveDelayed());
    }

    public void SetDirection(Vector3 bulletDirection)
    {
        this.direction = bulletDirection;
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
    }

    private IEnumerator SetInactiveDelayed()
    {
        despawned = true;
        yield return new WaitForSeconds(lifeTime);

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
    }
}
