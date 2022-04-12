using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{

    private float _baseVolume;
    
    // Start is called before the first frame update
    void Start()
    {
        _baseVolume = GetComponent<AudioSource>().volume;
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
            GetComponent<AudioSource>().volume = _baseVolume * AudioManager.audioInstance.soundEffectsFactor;
            GetComponent<AudioSource>().Play();
            
            LightBullet bullet = other.GetComponent<LightBullet>();

            Vector3 normal = -transform.forward;
            Vector3 reflected = bullet.GetDirection() - 2.0f * Vector3.Dot(bullet.GetDirection(), normal) * normal;
            other.GetComponent<LightBullet>().SetDirection(reflected);
        }
    }
}
