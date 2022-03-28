using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleHoleManager : MonoBehaviour
{

    /////////////////////////////////////////  p u b l i c   v a r i a b l e s  ////////////////////////////////////////
    // neighbour hole (trigger) variable
    public GameObject neighbourHole;
    
    // path variables
    public GameObject path;
    public bool followPath;
    
    // time variables
    public bool doWaitTime;
    public float waitTime;
    
    ////////////////////////////////////////  p r i v a t e   v a r i a b l e s  ///////////////////////////////////////
    // boolean variables to determine states
    private bool _cameFromNeighbour;
    
    // neighbourHole script to change its state variables
    private BeetleHoleManager _neighbourHoleScript;

    // time variable
    private float _waitedTime = 0;

    //////////////////////////////////////////////////  p r o g r a m  /////////////////////////////////////////////////
    /// 
    // Start is called before the first frame update
    void Start()
    {
        // get neighbour trigger hole script component
        _neighbourHoleScript = neighbourHole.GetComponent<BeetleHoleManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Beetle") && !_cameFromNeighbour)
        {
            if (!doWaitTime)
            {
                // movement to neighbour hole
                moveToNeighbourHole(other.gameObject);
            }
            else
            {
                // reset wait time
                _waitedTime = 0;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Beetle") && _cameFromNeighbour)
        {
            // reset values
            _cameFromNeighbour = false;
            
            // reset wait time
            _waitedTime = 0;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // increase waitedTime if required and if beetle didnt came from the neighbour hole
        if (doWaitTime && other.CompareTag("Beetle") && !_cameFromNeighbour)
        {
            _waitedTime += Time.deltaTime;

            if (doWaitTime && _waitedTime >= waitTime)
            {
                moveToNeighbourHole(other.gameObject);
            }
        }
    }
    
    
    void moveToNeighbourHole(GameObject other)
    {
        if (followPath)
        {   // do followPath
                    
        }
        else
        {   // do normal hole teleport animation
            doHoleTeleport(other, transform.position);
            _neighbourHoleScript.doHoleTeleport(other, neighbourHole.transform.position);
        }

        // change neighbour hole state
        _neighbourHoleScript._cameFromNeighbour = true;
    }
    void doHoleTeleport(GameObject gO, Vector3 pos)
    {
        // get GO character controler component
        CharacterController cc = gO.GetComponent<CharacterController>();
        
        // disable cc to do the 'teleport'
        cc.enabled = false;
        
        // teleport
        gO.transform.position = pos;
        
        // enable cc once teleport is made
        cc.enabled = true;
    }
    
    // Update is called once per frame
    void Update()
    {
    }
}
