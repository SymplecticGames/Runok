using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled)
            return;

        if (other.CompareTag("Bullet"))
        {
            LightBullet bullet = other.GetComponent<LightBullet>();

            Vector3 normal = -transform.forward;
            Vector3 reflected = bullet.GetDirection() - 2.0f * Vector3.Dot(bullet.GetDirection(), normal) * normal;
            other.GetComponent<LightBullet>().SetDirection(reflected);
        }
    }
}
