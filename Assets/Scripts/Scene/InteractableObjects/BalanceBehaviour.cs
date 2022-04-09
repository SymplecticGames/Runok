using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceBehaviour : MonoBehaviour
{
    [SerializeField]
    private List<BalancePlatform> balancePlatforms;



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
            Vector3 targetPos = plat.restPose + new Vector3(0.0f, overWeight, 0.0f);

            plat.SetTargetPos(targetPos);
        }
    }
}
