using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalancePlatform : MonoBehaviour
{
    private List<Weight> weights;

    [HideInInspector]
    public float totalWeight;

    [SerializeField]
    private Collider explicitSolidCollider;

    [HideInInspector]
    public Vector3 restPose;

    // Lerp position

    private Vector3 startPos;

    private Vector3 targetPos;

    private float lerpStep;

    private bool isLerping;

    [SerializeField]
    private float lerpSpeed = 3.0f;

    private Transform childPlatform;

    private void Awake()
    {
        if (!explicitSolidCollider)
            explicitSolidCollider = GetComponent<Collider>();

        weights = new List<Weight>();

        restPose = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        childPlatform = transform.GetChild(0);
        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Child platform position
        float childY = 0.5f * (restPose.y + transform.position.y);
        Vector3 newChildPos = childPlatform.position;
        newChildPos.y = childY;
        childPlatform.position = newChildPos;

        // Main platform position lerp
        if (isLerping)
        {
            if (lerpStep < 1.0f)
            {
                lerpStep += Time.deltaTime * lerpSpeed;
                transform.position = Vector3.Lerp(startPos, targetPos, lerpStep);
            }
            else
            {
                isLerping = false;
                transform.position = targetPos;
            }
        }

        if (!explicitSolidCollider)
            return;

        Collider[] weightCols = Physics.OverlapBox(explicitSolidCollider.bounds.center, explicitSolidCollider.bounds.extents * 1.5f, Quaternion.identity, Physics.AllLayers, QueryTriggerInteraction.Ignore);

        totalWeight = 0.0f;
        weights.Clear();
        foreach (Collider col in weightCols)
        {
            if (col.TryGetComponent(out Weight weight) && !weights.Contains(weight))
            {
                weights.Add(weight);
                totalWeight += weight.GetStackedWeight(ref weights);
            }
        }
    }

    public void SetTargetPos(Vector3 newPos)
    {
        startPos = transform.position;
        targetPos = newPos;
        lerpStep = 0.0f;
        isLerping = true;
    }

    public void ResetPlatform()
    {
        isLerping = false;
        transform.position = restPose;
    }
}
