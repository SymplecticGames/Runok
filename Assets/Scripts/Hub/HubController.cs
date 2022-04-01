using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HubController : MonoBehaviour
{
    [SerializeField]
    private Transform[] routes;

    [SerializeField]
    private Transform[] backRoutes;

    [SerializeField]
    private Transform golem;

    [SerializeField]
    private float speedFactor;

    private Animator goleAnim;
    private bool back;

    private int routeToGo = -1;
    private float tParam;
    private bool continueRoute;

    private Vector3 p0;
    private Vector3 p1;
    private Vector3 p2;
    private Vector3 p3;

    // Start is called before the first frame update
    void Start()
    {
        goleAnim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (continueRoute)
        {
            tParam += Time.deltaTime * speedFactor;

            transform.position = Mathf.Pow(1 - tParam, 3) * p0 + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + Mathf.Pow(tParam, 3) * p3;
            golem.forward = -(Mathf.Pow(1 - tParam, 2) * p0 - (3 * Mathf.Pow(tParam, 2) - 4 * tParam + 1) * p1 - (2 * tParam - 3 * Mathf.Pow(tParam, 2)) * p2 - Mathf.Pow(tParam, 2) * p3);
        }

        if (tParam >= 1.0f)
        {
            tParam = 0f;
            continueRoute = false;
            goleAnim.SetBool("isWalking", false);
            golem.forward = -Vector3.forward;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!context.performed || continueRoute)
            return;

        float direction = context.ReadValue<Vector2>().y;

        if (direction > 0 && routeToGo < routes.Length - 1)
        {
            back = false;
            AssignRoute();
            goleAnim.SetBool("isWalking", true);
            continueRoute = true;
        }

        else if (direction < 0 && routeToGo >= 0)
        {
            back = true;
            AssignRoute();
            goleAnim.SetBool("isWalking", true);
            continueRoute = true;
        }
    }

    private void AssignRoute()
    {
        if (back)
        {
            p0 = backRoutes[routeToGo].GetChild(0).position;
            p1 = backRoutes[routeToGo].GetChild(1).position;
            p2 = backRoutes[routeToGo].GetChild(2).position;
            p3 = backRoutes[routeToGo].GetChild(3).position;

            routeToGo--;
        }
        else
        {
            routeToGo++;

            p0 = routes[routeToGo].GetChild(0).position;
            p1 = routes[routeToGo].GetChild(1).position;
            p2 = routes[routeToGo].GetChild(2).position;
            p3 = routes[routeToGo].GetChild(3).position;
        }
    }
}
