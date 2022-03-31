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
        if (other.CompareTag("Bullet"))
        {
            LightBullet bullet = other.GetComponent<LightBullet>();
            Vector3 reflected = new Vector3(bullet.GetDirection().x, bullet.GetDirection().y, -bullet.GetDirection().z);
            other.GetComponent<LightBullet>().SetDirection(reflected);
        }
    }
}
