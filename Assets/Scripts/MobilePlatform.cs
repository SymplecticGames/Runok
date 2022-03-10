using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilePlatform : MonoBehaviour
{

    private float _startingPos;
    private float _finalPos;
    [SerializeField] private float _increment;
    private float _currentInc;
    private bool _movementStarted;
    
    // Start is called before the first frame update
    void Start()
    {
        _startingPos = transform.position.x;
        _finalPos = transform.position.x - 7.0f;
        _movementStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_movementStarted)
        {
            if (transform.position.x <= _finalPos) _currentInc = _increment;
            if (transform.position.x >= _startingPos) _currentInc = -_increment;
            transform.position = new Vector3(transform.position.x + _currentInc * Time.deltaTime, transform.position.y,
                transform.position.z);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Golem"))
        {
            other.GetComponent<GenericBehaviour>().SetAdditionalVel(new Vector3(_currentInc, 0.0f, 0.0f));
        }
    }
 
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Golem"))
        {
            other.GetComponent<GenericBehaviour>().SetAdditionalVel(Vector3.zero);
        }
    }

    public void StartMovingPlatform()
    {
        StartCoroutine(MovePlatform());
    }

    private IEnumerator MovePlatform()
    {
        yield return new WaitForSeconds(1.0f);
        _currentInc = _increment;
        _movementStarted = true;
    }
}
