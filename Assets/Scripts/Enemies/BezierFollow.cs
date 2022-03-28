using System.Collections;
using UnityEngine;

public class BezierFollow : MonoBehaviour
{
    [SerializeField]
    private Transform[] routes;

    [SerializeField]
    private float speedFactor;

    private int routeToGo;
    private float tParam;

    private Vector3 p0;
    private Vector3 p1;
    private Vector3 p2;
    private Vector3 p3;

    // Start is called before the first frame update
    void Start()
    {
        routeToGo = 0;

        p0 = routes[routeToGo].GetChild(0).position;
        p1 = routes[routeToGo].GetChild(1).position;
        p2 = routes[routeToGo].GetChild(2).position;
        p3 = routes[routeToGo].GetChild(3).position;
    }

    // Update is called once per frame
    void Update()
    {
        tParam += Time.deltaTime * speedFactor;

        transform.position = Mathf.Pow(1 - tParam, 3) * p0 + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + Mathf.Pow(tParam, 3) * p3;
        transform.forward = Mathf.Pow(1 - tParam, 2) * p0 - (3 * Mathf.Pow(tParam, 2) - 4 * tParam + 1) * p1 - (2 * tParam - 3 * Mathf.Pow(tParam, 2)) * p2 - Mathf.Pow(tParam, 2) * p3;

        if (tParam >= 1.0f)
        {
            tParam = 0f;
            routeToGo += 1;

            if (routeToGo > routes.Length - 1)
                routeToGo = 0;

            p0 = routes[routeToGo].GetChild(0).position;
            p1 = routes[routeToGo].GetChild(1).position;
            p2 = routes[routeToGo].GetChild(2).position;
            p3 = routes[routeToGo].GetChild(3).position;
        }
    }
}
