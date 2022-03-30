using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LumMode
{
    RadialLight = 1,
    LightShot = 2,
    LightImpulse = 3
}

public class BeetleBehaviour : MonoBehaviour
{
    [SerializeField]
    private float targetHeight;

    [SerializeField]
    private float upVerticalSpeed;

    [SerializeField]
    private float downVerticalSpeed;

    private float verticalSpeed;

    private float originalCoodown;

    // Shoot light bullets
    [SerializeField]
    public float shootCooldown;
    private BulletPool bulletPool;
    public float shootElapsedTime;

    [SerializeField]
    private GameObject forwardRay;

    [SerializeField]
    private GameObject backwardRay;

    [HideInInspector]
    public bool fwSkillPressed;

    [HideInInspector]
    public bool bwSkillPressed;

    private GenericBehaviour charBehaviour;

    public LumMode currentLumMode;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        currentLumMode = LumMode.RadialLight;

        verticalSpeed = downVerticalSpeed;

        charBehaviour = GetComponent<GenericBehaviour>();

        bulletPool = GetComponent<BulletPool>();
        shootElapsedTime = shootCooldown;

        animator = GetComponent<Animator>();

        originalCoodown = shootCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - 1.0f, transform.position.z);

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 7.0f, LayerMask.GetMask("LevelGeometry")))
        {
            float distance = this.transform.position.y - hit.point.y;

            float factorByDist = targetHeight - distance;
            if (factorByDist < 0.0f)
                verticalSpeed = downVerticalSpeed;
            else
                verticalSpeed = upVerticalSpeed;

            charBehaviour.SetAdditionalVel(new Vector3(0.0f, verticalSpeed * factorByDist, 0.0f));
        }
        else
            charBehaviour.SetAdditionalVel(Vector3.zero);

        switch (currentLumMode)
        {
            case LumMode.RadialLight:
                break;
            case LumMode.LightImpulse:
                shootCooldown = 0;

                if (fwSkillPressed)
                {
                    // Impulse
                    Debug.Log("bw ray");

                    // Ray fw
                    backwardRay.SetActive(true);
                }

                if (bwSkillPressed)
                {
                    // Impulse
                    Debug.Log("fw ray");

                    // Ray fw
                    forwardRay.SetActive(true);
                }

                break;
            case LumMode.LightShot:
                shootCooldown = originalCoodown;

                if (fwSkillPressed && shootElapsedTime > shootCooldown)
                {
                    bulletPool.SpawnBullet();
                    shootElapsedTime = 0;
                }

                shootElapsedTime += Time.deltaTime;
                break;
        }
    }

    public void ChangeLumMode(LumMode newMode)
    {
        currentLumMode = newMode;
        Debug.Log(currentLumMode);
    }

    private void DeactivateRays()
    {
        forwardRay.SetActive(false);
        backwardRay.SetActive(false);
    }

    private void SetShootFalse()
    {
        animator.SetBool("Shoot", false);
    }

    private void SetRayFalse()
    {
        animator.SetBool("Ray", false);
    }
}
