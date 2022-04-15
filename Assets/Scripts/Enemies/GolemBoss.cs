using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBoss : MonoBehaviour
{
    [SerializeField]
    public bool beaten;

    [SerializeField]
    private GolemHeadBoss head;

    [SerializeField]
    private GolemArmBoss leftArm;

    [SerializeField]
    private GolemArmBoss rightArm;

    private Animator headAnim;

    private Animator armsAnim;

    private float bossIdleTimer;

    private float bossTiredTimer;

    private int attacksDoneCounter;

    // Start is called before the first frame update
    void Start()
    {
        armsAnim = GetComponent<Animator>();
        headAnim = head.GetComponent<Animator>();

        bossIdleTimer = 0.0f;
        bossTiredTimer = 0.0f;
        attacksDoneCounter = 0;

        GameManager.instance.player.input.actions.FindAction("SwapCharacter").Disable();
    }

    // Update is called once per frame
    void Update()
    {
        // The boss is idling
        if (!armsAnim.GetBool("SwipeLeft") && !armsAnim.GetBool("SwipeRight") && !headAnim.GetBool("isTired"))
        {
            bossIdleTimer += Time.deltaTime;

            // Time in Idle
            if (bossIdleTimer > 2.0f)
            {
                int rand = Random.Range(0, 2); // 2 possibilities (Swipe L, Swipe R)

                if (attacksDoneCounter > 2)
                    rand = 2;

                if (rand == 0)
                    armsAnim.SetBool("SwipeLeft", true);
                else if (rand == 1)
                    armsAnim.SetBool("SwipeRight", true);
                else if (rand == 2)
                {
                    attacksDoneCounter = 0;
                    head.SetRuneActive(true, 1.0f);
                    headAnim.SetBool("isTired", true);
                }

                bossIdleTimer = 0.0f;
                attacksDoneCounter++;
            }
        }
        else if (headAnim.GetBool("isTired"))
        {
            bossTiredTimer += Time.deltaTime;

            // Time in Tired
            if (bossTiredTimer > 5.0f)
            {
                bossTiredTimer = 0.0f;
                head.SetRuneActive(false, 0.0f);
                headAnim.SetBool("isTired", false);
            }
        }
    }

    private void EndOfHit()
    {
        head.EndOfHit();
    }

    private void StartLeftHandRotation()
    {
        leftArm.StartHandRotation();
    }

    private void EndLeftHandRotation()
    {
        leftArm.EndHandRotation();
        armsAnim.SetBool("SwipeLeft", false);
    }

    private void StartRightHandRotation()
    {
        rightArm.StartHandRotation();
    }

    private void EndRightHandRotation()
    {
        rightArm.EndHandRotation();
        armsAnim.SetBool("SwipeRight", false);
    }
}
