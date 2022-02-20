using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10.0f, LayerMask.GetMask("LevelGeometry")))
        {
            GetComponent<Rigidbody>().position = new Vector3(hit.point.x, hit.point.y + 3.0f, hit.point.z);
        }
    }
}
