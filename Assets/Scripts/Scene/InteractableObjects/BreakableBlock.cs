using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled)
            return;

        if (!other.CompareTag("Hitter"))
            return;

        if (GameManager.instance.player.currentCharacter.TryGetComponent(out GolemBehaviour golem))
        {
            if(golem.currentMaterial == GolemMaterial.Plumber)
            {
                // Break
                Destroy(gameObject);
            }
        }
        else
        {
            // Play sound of hitting a wall
        }
    }
}
