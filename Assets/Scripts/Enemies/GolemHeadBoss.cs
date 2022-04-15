using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemHeadBoss : MonoBehaviour
{
    private GolemBoss golemBoss;

    private bool isBeingHitted;

    private SpriteRenderer runeSymbol;

    // Start is called before the first frame update
    void Start()
    {
        golemBoss = GetComponentInParent<GolemBoss>();
        runeSymbol = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled)
            return;

        if (other.CompareTag("Hitter") && !isBeingHitted)
        {
            // get reference to golem
            GolemBehaviour golem = other.GetComponentInParent<GolemBehaviour>();

            // set hittingAir to false
            golem.PlayHitSomethingSound();

            // update hit counter

            golemBoss.headAnim.SetBool("isHit", true);

            // Disable boss collider
            isBeingHitted = true;

            // Change for boss hit sound (si es que hay uno... xd)
            //GetComponent<AudioSource>().clip = AudioManager.audioInstance.GetCharSound(CharAudioTag.enemyHit);
            //GetComponent<AudioSource>().Play();
        }
    }

    public void EndOfHit()
    {
        isBeingHitted = false;

        golemBoss.headAnim.SetBool("isHit", false);
    }

    public void SetRuneActive(bool active, float delay)
    {
        StartCoroutine(SetRuneActiveSync(active, delay));
    }

    private IEnumerator SetRuneActiveSync(bool active, float delay)
    {
        yield return new WaitForSeconds(delay);

        Color col = runeSymbol.color;

        if (!active)
            col.a = 0.1f;
        else
            col.a = 1.0f;

        runeSymbol.color = col;
    }
}
