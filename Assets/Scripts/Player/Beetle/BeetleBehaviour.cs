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
    public float shootCooldown;
    private BulletPool bulletPool;
    public float shootElapsedTime;

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
                animator.SetBool("BackRay", false);
                animator.SetBool("FrontRay", false);
                break;
            case LumMode.LightShot:
                if (shootPressed && shootElapsedTime > shootCooldown)
                {
                    animator.SetBool("BackRay", true);

                    bulletPool.SpawnBullet();
                    shootElapsedTime = 0;
                }
                else
                    animator.SetBool("BackRay", false);

                shootElapsedTime += Time.deltaTime;
                break;
            case LumMode.LightImpulse:
                animator.SetBool("BackRay", backRayPressed);
                animator.SetBool("FrontRay", frontRayPressed);

                if (frontRayPressed)
                {
                    // Impulse backwards
                    

                    // Ray fw
                    frontRay.SetActive(true);
                }

                if (backRayPressed)
                {
                    // Impulse forward
                    

                    // Ray fw
                    backRay.SetActive(true);
                }

                break;
        }
    }

    public void ChangeLumMode(LumMode newMode)
    {
        currentLumMode = newMode;
        Debug.Log(currentLumMode);
    }

    #region Animation Events

    private void DeactivateRays()
    {
        frontRay.SetActive(false);
        backRay.SetActive(false);
    }

    #endregion
}
