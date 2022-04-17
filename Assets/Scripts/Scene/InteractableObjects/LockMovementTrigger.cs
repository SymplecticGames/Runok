using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockMovementTrigger : MonoBehaviour
{
    [SerializeField]
    private float delay = 1.0f;

    private float firstTriggerTime;

    private float isInTriggerCheck;

    private bool alreadyOutOfTrigger;

    // Start is called before the first frame update
    void Start()
    {
        firstTriggerTime = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - isInTriggerCheck > 0.2f && alreadyOutOfTrigger) // Time passed since it got out of the trigger
        {
            alreadyOutOfTrigger = true;
            GameManager.instance.player.golemBehaviour.canMove = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled)
            return;

        if (firstTriggerTime == -1)
            firstTriggerTime = Time.time;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!enabled)
            return;

        if (!other.CompareTag("Golem"))
            return;

        alreadyOutOfTrigger = false;

        if (firstTriggerTime != -1 && Time.time - firstTriggerTime >= delay)
        {
            isInTriggerCheck = Time.time;
            GameManager.instance.player.golemBehaviour.canMove = false;

            FindObjectOfType<DeathPit>().enabled = false;
            FindObjectOfType<PassToBoss>().enabled = true;
        }
    }
}
