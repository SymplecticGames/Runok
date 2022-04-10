using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weight : MonoBehaviour
{
    public float weightValue = 1.0f;

    [SerializeField]
    private Collider explicitSolidCollider;

    private void Awake()
    {
        if (!explicitSolidCollider)
            explicitSolidCollider = GetComponent<Collider>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public float GetStackedWeight(ref List<Weight> weightsRef)
    {
        float stackedValue = 0.0f;

        Collider[] weightCols = Physics.OverlapBox(explicitSolidCollider.bounds.center, explicitSolidCollider.bounds.extents * 1.5f, Quaternion.identity, Physics.AllLayers, QueryTriggerInteraction.Ignore);

        foreach (Collider col in weightCols)
        {
            if (col.TryGetComponent(out Weight weight) && !weightsRef.Contains(weight))
            {
                weightsRef.Add(weight);
                stackedValue += weight.GetStackedWeight(ref weightsRef);
            }
        }

        return stackedValue + weightValue;
    }
}
