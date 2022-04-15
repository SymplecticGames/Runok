using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemArmBoss : MonoBehaviour
{
    private bool _rotationStarted;

    private MeshFilter meshFilter;

    private Transform hand;

    [SerializeField]
    private float maxHandRotSpeed = 5.0f;

    private float currentHandRotSpeed;

    private float pivotHandRotSpeed;

    private float handRotSpeedLerpTime;

    [SerializeField]
    private float handRotSpeedLerpAccel = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        hand = transform.GetChild(0);

        meshFilter = hand.GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_rotationStarted)
        {
            if (handRotSpeedLerpTime < 1.0f)
            {
                currentHandRotSpeed = Mathf.Lerp(0.0f, maxHandRotSpeed, handRotSpeedLerpTime);
                handRotSpeedLerpTime += Time.deltaTime * handRotSpeedLerpAccel;
            }
            else
                currentHandRotSpeed = maxHandRotSpeed;

            Vector3 localHandCenter = meshFilter.mesh.bounds.center;
            hand.RotateAround(hand.TransformPoint(localHandCenter), transform.up, Time.deltaTime * 100 * currentHandRotSpeed);
        }
        else
        {
            if (handRotSpeedLerpTime <= 1.0f)
            {
                currentHandRotSpeed = Mathf.Lerp(pivotHandRotSpeed, 0.0f, handRotSpeedLerpTime);
                handRotSpeedLerpTime += Time.deltaTime * handRotSpeedLerpAccel;

                Vector3 localHandCenter = meshFilter.mesh.bounds.center;
                hand.RotateAround(hand.TransformPoint(localHandCenter), transform.up, Time.deltaTime * 100 * currentHandRotSpeed);
            }
        }
    }

    public void StartHandRotation()
    {
        _rotationStarted = true;
        handRotSpeedLerpTime = 0.0f;
    }

    public void EndHandRotation()
    {
        _rotationStarted = false;
        handRotSpeedLerpTime = 0.0f;
        pivotHandRotSpeed = currentHandRotSpeed;
    }
}
