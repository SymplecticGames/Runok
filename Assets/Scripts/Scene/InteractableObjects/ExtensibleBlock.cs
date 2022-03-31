using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtensibleBlock : MonoBehaviour
{
    [SerializeField]
    private GameObject[] boxes;

    [SerializeField]
    private Transform[] boxHeights;

    private bool startGrowing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startGrowing)
        {
            startGrowing = false;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Hitter"))
            return;

        if (GameManager.instance.player.currentCharacter.TryGetComponent(out GolemBehaviour golem))
        {
            if (golem.currentMaterial == GolemMaterial.Wooden)
            {
                // Scaling
                Destroy(gameObject);
            }
        }
        else
        {
            // Play sound of hitting a wall
        }
    }
}
