using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleMenu : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("isBoss", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
