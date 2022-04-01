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

    // Shoot light bullets
    [SerializeField]
    private float shootCooldown;
    private BulletPool bulletPool;
    private float shootElapsedTime;

    // Light bullets
    [SerializeField]
    private float rayCooldown;
    private float fwRayElapsedTime;
    private float bwRayElapsedTime;

    [SerializeField]
    private GameObject frontRay;

    [SerializeField]
    private GameObject backRay;

    [HideInInspector]
    public bool shootPressed;

    [HideInInspector]
    public bool frontRayPressed;

    [HideInInspector]
    public bool backRayPressed;

    [HideInInspector]
    public bool shootingBackRay = false;

    [HideInInspector]
    public bool shootingFrontRay = false;

    private GenericBehaviour charBehaviour;

    public LumMode currentLumMode;

    private Animator animator;

    [SerializeField]
    private AnimationClip rayClip;

    [SerializeField]
    private float impulseFactor;

    private float forwardFactor;

    private float backwardFactor;

    private Vector3 additionalVel = Vector3.zero;

    private GameObject beetleLight;

    // Start is called before the first frame update
    void Start()
    {
        verticalSpeed = downVerticalSpeed;

        charBehaviour = GetComponent<GenericBehaviour>();

        bulletPool = GetComponent<BulletPool>();
        shootElapsedTime = shootCooldown;

        bwRayElapsedTime = rayCooldown;
        fwRayElapsedTime = rayCooldown;

        animator = GetComponent<Animator>();

        beetleLight = GameObject.FindGameObjectWithTag("RadialLight");
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

            additionalVel = new Vector3(0.0f, verticalSpeed * factorByDist, 0.0f);
        }
        else
            additionalVel = Vector3.zero;

        switch (currentLumMode)
        {
            case LumMode.RadialLight:
                if (!beetleLight.activeInHierarchy) beetleLight.SetActive(true);

                animator.SetBool("BackRay", false);
                animator.SetBool("FrontRay", false);
                break;
            case LumMode.LightShot:
                if (beetleLight.activeInHierarchy) beetleLight.SetActive(false);

                if (shootPressed && shootElapsedTime > shootCooldown)
                {
                    animator.SetBool("BackRay", true);

                    bulletPool.SpawnBullet();
                    shootElapsedTime = 0.0f;
                }
                else
                    animator.SetBool("BackRay", false);

                shootElapsedTime += Time.deltaTime;
                break;
            case LumMode.LightImpulse:
                if (beetleLight.activeInHierarchy) beetleLight.SetActive(false);
                
                additionalVel += charBehaviour.currentForwardTarget * impulseFactor * forwardFactor;
                additionalVel += charBehaviour.currentForwardTarget * impulseFactor * backwardFactor;

                if (frontRayPressed && !shootingFrontRay && fwRayElapsedTime > rayCooldown)
                {
                    shootingFrontRay = true;
                    animator.SetBool("FrontRay", true);

                    StartCoroutine(ImpulseBwAndFrontRay(rayClip.length));
                }
                else
                    animator.SetBool("FrontRay", false);

                if (backRayPressed && !shootingBackRay && bwRayElapsedTime > rayCooldown)
                {
                    shootingBackRay = true;
                    animator.SetBool("BackRay", true);

                    StartCoroutine(ImpulseFwAndBackRay(rayClip.length));
                }
                else
                    animator.SetBool("BackRay", false);

                fwRayElapsedTime += Time.deltaTime;
                bwRayElapsedTime += Time.deltaTime;

                break;
        }

        charBehaviour.SetAdditionalVel(additionalVel);
    }

    public void ChangeLumMode(LumMode newMode)
    {
        currentLumMode = newMode;
    }

    // Impulse Backward and FrontRay
    private IEnumerator ImpulseBwAndFrontRay(float rayDuration)
    {
        // Impulse backwards
        backwardFactor = -1.0f;

        // Front Ray
        frontRay.SetActive(true);

        yield return new WaitForSeconds(rayDuration);

        DeactivateFrontRay();
    }

    private void DeactivateFrontRay()
    {
        frontRay.SetActive(false);
        fwRayElapsedTime = 0.0f;
        shootingFrontRay = false;
        backwardFactor = 0.0f;
    }

    // Impulse Forward and Back Ray
    private IEnumerator ImpulseFwAndBackRay(float rayDuration)
    {
        // Impulse forward
        forwardFactor = 1.0f;

        // Back Ray
        backRay.SetActive(true);

        yield return new WaitForSeconds(rayDuration);

        DeactivateBackRay();
    }

    private void DeactivateBackRay()
    {
        backRay.SetActive(false);
        bwRayElapsedTime = 0.0f;
        shootingBackRay = false;
        forwardFactor = 0.0f;
    }
}
