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

    [Header("Fog")]
    [SerializeField]
    private GameObject fogObject;

    [SerializeField]
    private Material[] fogMaterials;

    private Material previousFogMaterial;

    private Material targetFogMaterial;

    private Animator goleAnim;
    private bool back;

    private float tParam;
    private bool isMoving;
    private bool isEnteringLevel;

    private Vector3 p0;
    private Vector3 p1;
    private Vector3 p2;
    private Vector3 p3;

    // Start is called before the first frame update
    void Start()
    {
        goleAnim = GetComponentInChildren<Animator>();

        transform.position = routes[ProgressManager.instance.currentLevel].GetChild(0).position;
        fogObject.GetComponent<MeshRenderer>().material = fogMaterials[ProgressManager.instance.currentLevel];
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            tParam += Time.deltaTime * speedFactor;

            transform.position = Mathf.Pow(1 - tParam, 3) * p0 + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + Mathf.Pow(tParam, 3) * p3;
            golem.forward = -(Mathf.Pow(1 - tParam, 2) * p0 - (3 * Mathf.Pow(tParam, 2) - 4 * tParam + 1) * p1 - (2 * tParam - 3 * Mathf.Pow(tParam, 2)) * p2 - Mathf.Pow(tParam, 2) * p3);

            fogObject.GetComponent<MeshRenderer>().material.Lerp(previousFogMaterial, targetFogMaterial, tParam);
        }

        if (tParam >= 1.0f)
        {
            tParam = 0f;
            isMoving = false;
            goleAnim.SetBool("isWalking", false);
            golem.forward = -Vector3.forward;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!context.performed || isMoving || isEnteringLevel)
            return;

        float direction = context.ReadValue<Vector2>().y;

        // Go to next level
        if (direction > 0 && ProgressManager.instance.currentLevel < routes.Length - 1 && ProgressManager.instance.currentLevel < ProgressManager.instance.currentCompletedLevels)
        {
            back = false;
            AssignRoute();
            goleAnim.SetBool("isWalking", true);
            isMoving = true;
        }
        // Go to previous level
        else if (direction < 0 && ProgressManager.instance.currentLevel > 0)
        {
            back = true;
            AssignRoute();
            goleAnim.SetBool("isWalking", true);
            isMoving = true;
        }
    }

    public void OnEnterLevel(InputAction.CallbackContext context)
    {
        if (!isMoving && context.performed)
        {
            isEnteringLevel = true;

            AudioManager.audioInstance.PlayUISound(UIaudioTag.enterLevel);

            switch (ProgressManager.instance.currentLevel)
            {
                case 0: // nivel 1
                    StartCoroutine(SceneTransition.sceneTransitioninstance.LoadScene("Level1"));
                    break;

                case 1: // nivel 2
                    StartCoroutine(SceneTransition.sceneTransitioninstance.LoadScene("Level2"));
                    break;

                case 2: // nivel 3
                    StartCoroutine(SceneTransition.sceneTransitioninstance.LoadScene("Level3"));
                    break;

                case 3: // nivel 4
                    //StartCoroutine(SceneTransition.sceneTransitioninstance.LoadScene("Level4"));
                    StartCoroutine(SceneTransition.sceneTransitioninstance.LoadScene("TestPlayground"));
                    break;

                default: break;
            }
        }
    }

    public void OnExitLevel(InputAction.CallbackContext context)
    {
        if (!isMoving && context.performed)
        {
            AudioManager.audioInstance.PlayUISound(UIaudioTag.exitHub);
            StartCoroutine(SceneTransition.sceneTransitioninstance.LoadScene("MainMenu"));
        }
    }

    private void AssignRoute()
    {
        if (back)
        {
            previousFogMaterial = fogMaterials[ProgressManager.instance.currentLevel];
            ProgressManager.instance.currentLevel--;
            targetFogMaterial = fogMaterials[ProgressManager.instance.currentLevel];

            p0 = backRoutes[ProgressManager.instance.currentLevel].GetChild(0).position;
            p1 = backRoutes[ProgressManager.instance.currentLevel].GetChild(1).position;
            p2 = backRoutes[ProgressManager.instance.currentLevel].GetChild(2).position;
            p3 = backRoutes[ProgressManager.instance.currentLevel].GetChild(3).position;
        }
        else
        {
            p0 = routes[ProgressManager.instance.currentLevel].GetChild(0).position;
            p1 = routes[ProgressManager.instance.currentLevel].GetChild(1).position;
            p2 = routes[ProgressManager.instance.currentLevel].GetChild(2).position;
            p3 = routes[ProgressManager.instance.currentLevel].GetChild(3).position;

            previousFogMaterial = fogMaterials[ProgressManager.instance.currentLevel];
            ProgressManager.instance.currentLevel++;
            targetFogMaterial = fogMaterials[ProgressManager.instance.currentLevel];
        }
    }
}
