using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    private Queue<GameObject> bulletPool;
    private LightBullet bulletComponent;

    [Header("Pool Properties")]
    [SerializeField] Object bullet;
    [SerializeField] int poolSize;

    [Header("Bullets properties")]
    [SerializeField] float speed;
    [SerializeField] int lifeTime;
    [SerializeField] int damage;

    void Start()
    {
        bulletPool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = (GameObject)Instantiate(bullet);

            bulletComponent = obj.GetComponent<LightBullet>();
            bulletComponent.SetLifeTime(lifeTime);
            bulletComponent.SetDamage(damage);

            obj.SetActive(false);
            bulletPool.Enqueue(obj);
        }
    }

    public GameObject SpawnBullet()
    {
        GameObject objToSpawn = bulletPool.Dequeue();

        objToSpawn.SetActive(true);
        objToSpawn.transform.position = transform.position;
        objToSpawn.transform.rotation = transform.rotation;

        bulletComponent = objToSpawn.GetComponent<LightBullet>();
        objToSpawn.GetComponent<LightBullet>().SetDirection(transform.forward);
        objToSpawn.GetComponent<LightBullet>().SetSpeed(speed);
        objToSpawn.GetComponent<LightBullet>().SetDespawned(false);

        bulletPool.Enqueue(objToSpawn);

        return objToSpawn;
    }

    public void ClearPool()
    {
        foreach (GameObject go in bulletPool)
        {
            go.SetActive(false);
        }
    }
}
