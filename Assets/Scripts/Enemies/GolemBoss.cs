using UnityEngine;

public class GolemBoss : MonoBehaviour
{
    [HideInInspector]
    public bool beaten;

    [SerializeField]
    private GolemHeadBoss head;

    [SerializeField]
    private GolemArmBoss leftArm;

    [SerializeField]
    private GolemArmBoss rightArm;

    [HideInInspector]
    public Animator headAnim;

    private Animator armsAnim;

    private float bossIdleTimer;

    private float bossTiredTimer;

    private int attacksDoneCounter;

    [Header("Properties")]
    [SerializeField]
    private int maxHealth = 10;

    private int currentHealth;

    [SerializeField]
    private float timeBetweenAttacks = 2.0f;

    [SerializeField]
    private float timeSpentTired = 5.0f;

    [SerializeField]
    private int maxContinousAttacks = 3;

    // Start is called before the first frame update
    void Start()
    {
        armsAnim = GetComponent<Animator>();
        headAnim = head.GetComponent<Animator>();

        bossIdleTimer = 0.0f;
        bossTiredTimer = 0.0f;
        attacksDoneCounter = 0;

        currentHealth = maxHealth;

        GameManager.instance.player.input.actions.FindAction("SwapCharacter").Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (beaten)
        {
            headAnim.SetBool("Die", true);
            return;
        }

        // The boss is idling
        if (!armsAnim.GetBool("SwipeLeft") && !armsAnim.GetBool("SwipeRight") && !armsAnim.GetBool("SmashLeft") && !armsAnim.GetBool("SmashRight") && !headAnim.GetBool("isTired"))
        {
            bossIdleTimer += Time.deltaTime;

            if (bossIdleTimer > timeBetweenAttacks)
            {
                bossIdleTimer = 0.0f;
                attacksDoneCounter++;

                int rand = Random.Range(0, 4); // 4 different patterns

                if (attacksDoneCounter > maxContinousAttacks)
                    rand = -1;

                switch (rand)
                {
                    case -1:
                        attacksDoneCounter = 0;
                        head.SetRuneActive(true, 1.0f);
                        headAnim.SetBool("isTired", true);
                        break;
                    case 0:
                        armsAnim.SetBool("SwipeLeft", true);
                        break;
                    case 1:
                        armsAnim.SetBool("SwipeRight", true);
                        break;
                    case 2:
                        armsAnim.SetBool("SmashLeft", true);
                        break;
                    case 3:
                        armsAnim.SetBool("SmashRight", true);
                        break;
                }
            }
        }
        else if (headAnim.GetBool("isTired"))
        {
            bossTiredTimer += Time.deltaTime;

            if (bossTiredTimer > timeSpentTired)
            {
                bossTiredTimer = 0.0f;
                head.SetRuneActive(false, 0.0f);
                headAnim.SetBool("isTired", false);
            }
        }
    }

    public void LoseHealth()
    {
        currentHealth--;

        if (currentHealth < 0)
        {
            beaten = true;
            GameManager.instance.player.input.actions.FindAction("SwapCharacter").Enable();
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

    private void EndLeftHandSmash()
    {
        leftArm.EndHandRotation();
        armsAnim.SetBool("SmashLeft", false);
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

    private void EndRightHandSmash()
    {
        rightArm.EndHandRotation();
        armsAnim.SetBool("SmashRight", false);
    }
}
