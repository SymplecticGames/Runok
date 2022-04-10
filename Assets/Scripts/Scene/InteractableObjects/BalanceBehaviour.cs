using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceBehaviour : MonoBehaviour
{
    [SerializeField]
    private List<BalancePlatform> balancePlatforms;

    [SerializeField]
    [Tooltip("Distance from the rest pose to the max point above and the min point below")]
    private float maxHeight = 10.0f;

    [SerializeField]
    [Tooltip("Value that represents how much displacement will occur when applying a weight. Zero means the balance locks")]
    private float looseness = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float totalWeights = 0.0f;
        foreach (BalancePlatform plat in balancePlatforms)
            totalWeights += plat.totalWeight;

        float weightExpectedPerPlat = totalWeights / balancePlatforms.Count;

        foreach (BalancePlatform plat in balancePlatforms)
        {
            float overWeight = weightExpectedPerPlat - plat.totalWeight;
            float targetY = Mathf.Clamp(overWeight * looseness, -maxHeight, maxHeight);
            Vector3 targetPos = plat.restPose + new Vector3(0.0f, targetY, 0.0f);

            plat.SetTargetPos(targetPos);
        }
    }

    public void ResetBalance()
    {
        foreach (BalancePlatform plat in balancePlatforms)
            plat.ResetPlatform();
    }
}
