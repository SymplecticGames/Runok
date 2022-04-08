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

    private void Awake()
    {
        if (!explicitSolidCollider)
            explicitSolidCollider = GetComponent<Collider>();

        weights = new List<Weight>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!explicitSolidCollider)
            return;

        Collider[] weightCols = Physics.OverlapBox(explicitSolidCollider.bounds.center, explicitSolidCollider.bounds.extents, Quaternion.identity, Physics.AllLayers, QueryTriggerInteraction.Ignore);

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
}
