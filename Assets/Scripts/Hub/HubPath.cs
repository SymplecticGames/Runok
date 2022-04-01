using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubPath : MonoBehaviour
{
    [SerializeField]
    private float vertexCount;

    private LineRenderer lineRenderer;
    private Vector3[] points;
    List<Vector3> pointList;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        points = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(points);
        pointList = new List<Vector3>();

        for (float t = 0; t <= 1; t += 1 / vertexCount)
        {
            Vector3 pos = Mathf.Pow(1 - t, 3) * points[0] + 3 * Mathf.Pow(1 - t, 2) * t * points[1] + 3 * (1 - t) * Mathf.Pow(t, 2) * points[2] + Mathf.Pow(t, 3) * points[3];
            pointList.Add(pos);
        }

        lineRenderer.positionCount = pointList.Count;
        lineRenderer.SetPositions(pointList.ToArray());
    }

    // Update is called once per frame
    void Update()
    {

    }

}

