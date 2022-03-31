using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressureButton : MonoBehaviour
{
    [SerializeField]
    private Transform pressedPos;

    [SerializeField]
    private Collider solidButton;

    [SerializeField]
    private UnityEvent action;

    private Vector3 initialPos;

    private bool isPlumber;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = solidButton.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.player.golemBehaviour.TryGetComponent(out GolemBehaviour golem) && golem.currentMaterial == GolemMaterial.Plumber)
        {
            isPlumber = true;
            solidButton.enabled = false;
        }
        else
        {
            isPlumber = false;
            solidButton.enabled = true;

            solidButton.transform.position = initialPos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Golem") || !isPlumber)
            return;

        solidButton.transform.position = pressedPos.position;

        action.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Golem"))
            return;

        solidButton.transform.position = initialPos;
    }
}
