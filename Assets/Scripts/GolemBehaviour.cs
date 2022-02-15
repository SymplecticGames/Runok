using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GolemBehaviour : MonoBehaviour
{
    [SerializeField]
    private float colliderLifetime;

    private float lifeTimer;

    [SerializeField]
    private Collider hitRegister;

    [HideInInspector]
    public bool hitPressed;

    // Start is called before the first frame update
    void Start()
    {
        lifeTimer = colliderLifetime;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer += Time.deltaTime;

        if (lifeTimer >= colliderLifetime)
        {
            hitRegister.enabled = false;
            lifeTimer = colliderLifetime;

            if (hitPressed)
            {
                hitRegister.enabled = true;
                lifeTimer = 0.0f;
            }
        }
    }
}
