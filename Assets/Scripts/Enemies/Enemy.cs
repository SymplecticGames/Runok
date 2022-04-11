using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    /////////////////////////////////////////  p u b l i c   v a r i a b l e s  ////////////////////////////////////////
    public bool allowWalking;

    [SerializeField]
    private bool shadowEnemy;

    // spawn variables
    public bool spawnAfterTime;
    public float spawnTime = 10.0f;

    // respawn variables
    public bool respawnAfterPlayerDeath;

    ////////////////////////////////////////  p r i v a t e   v a r i a b l e s  ///////////////////////////////////////
    // number of hits recieved
    private int _hitCounter;

    // respawn timer
    private float _activeSpawnTimer;

    // variable to control when the enemy is defeated
    [HideInInspector]
    public bool _isDefeated;

    // original spawn transform
    private Transform _spawnTransform;

    // For enable and disable component, to stop bezier movement
    private BezierFollow bezier;

    private CharacterController controller;

    private Animator animator;

    private SkinnedMeshRenderer enemyMesh;

    [SerializeField]
    private Material shadowMat;

    private bool isBeingHitted;

    //////////////////////////////////////////////////  p r o g r a m  /////////////////////////////////////////////////
    ///
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        bezier = GetComponent<BezierFollow>();
        enemyMesh = GetComponentInChildren<SkinnedMeshRenderer>();

        bezier.enabled = allowWalking;

        if (shadowEnemy) {
            var mat = enemyMesh.materials;
            mat[2] = shadowMat;
            enemyMesh.materials = mat;
        }

        _spawnTransform = gameObject.transform;

        if (spawnAfterTime)
        {
            HideEnemy();
        }
        else
        {
            SpawnEnemy();
        }

        if (respawnAfterPlayerDeath) GameManager.instance.respawnableEnemy(this);

        animator.SetBool("allowWalking", allowWalking);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled)
            return;

        if (other.CompareTag("Golem") || other.CompareTag("Beetle"))
            GameManager.instance.player.Die();

        // if the trigger is hit by the golem, the enemy dies when the number of hits is greater or equal
        // to the number of hits the golem needs (with that material) to defeat it.
        if (other.CompareTag("Hitter") && !shadowEnemy && !isBeingHitted)
        {
            // get reference to golem
            GolemBehaviour golem = other.GetComponentInParent<GolemBehaviour>();

            // set hittingAir to false
            golem.PlayHitSomethingSound();
            
            // update hit counter
            _hitCounter++;

            bezier.enabled = false;
            animator.SetBool("isHit", true);

            // Disable enemy collider
            isBeingHitted = true;

            GetComponent<AudioSource>().clip = AudioManager.audioInstance.GetCharSound(CharaudioTag.enemyHit);
            GetComponent<AudioSource>().Play();
            
            // if hits are enough to defeat the enemy, then, do deafeated animation
            if (_hitCounter >= golem.golemStats.hitsNeededToKill)
            {
                // the enemy is defeated
                Die();
            }
            
        }

        if (other.CompareTag("Bullet"))
        {
            other.GetComponent<LightBullet>().SetInactive();

            if (shadowEnemy)
            {
                // update hit counter
                _hitCounter++;

                bezier.enabled = false;
                animator.SetBool("isHit", true);
                
                GetComponent<AudioSource>().Play();
                
                // if hits are enough to defeat the enemy, then, do deafeated animation
                if (_hitCounter >= 2)
                {
                    // the enemy is defeated
                    Die();
                }
                
            }
        }

        if (other.CompareTag("Ray"))
        {
            if (shadowEnemy)
            {
                GetComponent<AudioSource>().Play();
                Die();
            }
        }
    }

    public void Die()
    {
        _isDefeated = true;
        HideEnemy();
    }

    public void HideEnemy()
    {
        // make enemy invisible
        foreach (Renderer visualPart in GetComponentsInChildren<Renderer>())
            visualPart.enabled = false;

        // Disable CC and collider
        gameObject.GetComponent<CharacterController>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
        GetComponent<AudioSource>().mute = true;
    }

    public void SpawnEnemy()
    {
        // reset variables
        _hitCounter = 0;
        _isDefeated = false;
        gameObject.transform.position = _spawnTransform.position;

        // make enemy visible
        foreach (Renderer visualPart in GetComponentsInChildren<Renderer>())
            visualPart.enabled = true;

        // enable CC
        gameObject.GetComponent<CharacterController>().enabled = true;
        gameObject.GetComponent<Collider>().enabled = true;

        bezier.enabled = allowWalking;
        GetComponent<AudioSource>().mute = false;
    }

    private void EnableMovement()
    {
        bezier.enabled = allowWalking;
        animator.SetBool("isHit", false);
        isBeingHitted = false;
    }
    
    public void PlayEnemyIdleSound()
    {
        GetComponent<AudioSource>().clip = AudioManager.audioInstance.GetCharSound(CharaudioTag.enemyIdle);
        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnAfterTime && !_isDefeated && !gameObject.GetComponent<CharacterController>().enabled)
        {
            _activeSpawnTimer += Time.deltaTime;

            if (_activeSpawnTimer >= spawnTime)
            {
                SpawnEnemy();
            }
        }

        if (controller.enabled && !allowWalking)
            controller.Move(GameManager.gravity * Time.deltaTime);

        animator.SetBool("isFalling", !controller.isGrounded);
    }
}
