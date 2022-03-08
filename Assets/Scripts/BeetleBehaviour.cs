using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LumMode
{
    None = 0,
    RadialLight = 1,
    LightImpulse = 2,
    LightShot = 3
}

public class BeetleBehaviour : MonoBehaviour
{
    [SerializeField]
    private float targetHeight;

    [SerializeField]
    private float verticalSpeed;

    private GenericBehaviour charBehaviour;

    public LumMode currentLumMode;

    // Start is called before the first frame update
    void Start()
    {
        currentLumMode = LumMode.None;

        charBehaviour = GetComponent<GenericBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - 1.0f, transform.position.z);

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 5.0f, LayerMask.GetMask("LevelGeometry")))
        {
            float distance = this.transform.position.y - hit.point.y;

            float factorByDist = targetHeight - distance;

            charBehaviour.SetAdditionalVel(new Vector3(0.0f, verticalSpeed * factorByDist, 0.0f));
        }
    }

    public void ChangeLumMode(LumMode newMode)
    {
        currentLumMode = newMode;
    }
}
