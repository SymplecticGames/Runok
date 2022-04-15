using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemHeadBoss : MonoBehaviour
{
    private Animator animator;

    private bool isBeingHitted;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
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

            animator.SetBool("isHit", true);

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

        animator.SetBool("isHit", false);
    }
}
