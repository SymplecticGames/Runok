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
        Debug.Log("P1: " + balancePlatforms[0].totalWeight + " | P2: " + balancePlatforms[1].totalWeight);
    }
}
