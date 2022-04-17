using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private bool alreadyDead;

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
            if (!alreadyDead) headAnim.SetTrigger("Die");
            alreadyDead = true;
            return;
        }

        if (headAnim.GetBool("isTired"))
        {
            bossTiredTimer += Time.deltaTime;

            if (bossTiredTimer > timeSpentTired)
            {
                bossTiredTimer = 0.0f;
                head.SetRuneActive(false, 0.0f);
                headAnim.SetBool("isTired", false);
            }
        }
        else if (armsAnim.GetCurrentAnimatorStateInfo(0).IsTag("IdleTag"))
        {
            bossIdleTimer += Time.deltaTime;

            if (bossIdleTimer > timeBetweenAttacks)
            {
                bossIdleTimer = 0.0f;
                attacksDoneCounter++;

                int rand = UnityEngine.Random.Range(0, 4); // 4 different patterns

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
                        armsAnim.SetTrigger("SwipeLeft");
                        GetComponent<AudioSource>().clip = AudioManager.audioInstance.GetObjSound(ObjAudioTag.golemBossPush);
                        GetComponent<AudioSource>().Play();
                        break;
                    case 1:
                        armsAnim.SetTrigger("SwipeRight");
                        GetComponent<AudioSource>().clip = AudioManager.audioInstance.GetObjSound(ObjAudioTag.golemBossPush);
                        GetComponent<AudioSource>().Play();
                        break;
                    case 2:
                        armsAnim.SetTrigger("SmashLeft");
                        break;
                    case 3:
                        armsAnim.SetTrigger("SmashRight");
                        break;
                }
            }
        }
    }

    public void playSmashSound()
    {
        GetComponent<AudioSource>().clip = AudioManager.audioInstance.GetObjSound(ObjAudioTag.golemBossHit);
        GetComponent<AudioSource>().Play();
    }

    public void GoToBeetleBoss()
    {
        SceneManager.LoadScene("BeetleBoss");
    }

    public void LoseHealth(int healthLost)
    {
        currentHealth -= healthLost;

        if (currentHealth <= 0)
        {
            beaten = true;
            GameManager.instance.player.input.actions.FindAction("SwapCharacter").Enable();
        }
    }

    public bool IsSwiping()
    {
        return armsAnim.GetCurrentAnimatorStateInfo(0).IsTag("SwipeAttack");
    }

    public void HitPlumber()
    {
        armsAnim.SetTrigger("HitPlumber");
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
    }

    private void StartRightHandRotation()
    {
        rightArm.StartHandRotation();
    }

    private void EndRightHandRotation()
    {
        rightArm.EndHandRotation();
    }
}
