using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    /////////////////////////////////////////  p u b l i c   v a r i a b l e s  ////////////////////////////////////////
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
    private bool _isDefeated;
    
    // original spawn transform
    private Transform _spawnTransform;

    // For enable and disable component, to stop bezier movement
    private BezierFollow bezier;

    //////////////////////////////////////////////////  p r o g r a m  /////////////////////////////////////////////////
    ///
    // Start is called before the first frame update
    void Start()
    {
        bezier = GetComponent<BezierFollow>();
        _spawnTransform = gameObject.transform;
        
        if (spawnAfterTime)
        {
            HideEnemy();
        }
        else
        {
            SpawnEnemy();
        }

    }

    private void OnTriggerEnter(Collider other)
    {

        // if the trigger is hit by the golem, the enemy dies when the number of hits is greater or equal
        // to the number of hits the golem needs (with that material) to defeat it.
        if (other.CompareTag("Hitter"))
        {
            // get reference to golem
            GolemBehaviour golem = other.GetComponentInParent<GolemBehaviour>();

            // update hit counter
            _hitCounter++;
            Debug.Log(_hitCounter);
            // if hits are enough to defeat the enemy, then, do deafeated animation
            if (_hitCounter >= golem.golemStats.hitsNeededToKill)
            {
                // the enemy is defeated
                Die();
            }
        }

    }
    
    public void Die()
    {

        _isDefeated = true;
        
        HideEnemy();

        // if this enemy respawns after player death, its stored in GameManager
        if (respawnAfterPlayerDeath)
        {
            GameManager.instance.defeatedRespawnableEnemy(gameObject);
        }

        /////// defeated animation ///////
        /*
         *           P O R   I M P L E M E N T A R
         */
    }

    public void HideEnemy()
    {
        // make enemy invisible
        foreach (Renderer visualPart in GetComponentsInChildren<Renderer>())
            visualPart.enabled = false;

        // disenable CC
        gameObject.GetComponent<CharacterController>().enabled = false;
        
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
    }
    
    /*
     *
     *
     *               A Ñ A D I R  E L  M O V I M I E N T O   D E   L O S   E N E M I G O S
     *
     * 
     */

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
    }
}
