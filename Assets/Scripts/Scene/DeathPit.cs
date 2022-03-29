using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPit : MonoBehaviour
{
    private PlayerManager player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        // If what entered was not the golem or the beetle
        if (!other.CompareTag("Golem") && !other.CompareTag("Beetle"))
            return;

        player.Die();
    }
}
