using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPit : MonoBehaviour
{
    [SerializeField]
    private PlayerManager player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    // If what entered was not the golem or the beetle
    //    if (!other.CompareTag("Golem") && !other.CompareTag("Beetle"))
    //        return;

    //    // If the deathpit is lava, and the golem got inside it
    //    if (CompareTag("Lava") && player.currentCharacter.TryGetComponent(out GolemBehaviour golem) && golem.currentMaterial == GolemMaterial.Stone)
    //    {
    //        golem.insideLava = true;
    //        return;
    //    }

    //    player.Die();
    //}

    private void OnTriggerStay(Collider other)
    {
        // If what entered was not the golem or the beetle
        if (!other.CompareTag("Golem") && !other.CompareTag("Beetle"))
            return;

        // If the deathpit is lava, and the golem got inside it
        if (CompareTag("Lava") && player.currentCharacter.TryGetComponent(out GolemBehaviour golem) && golem.currentMaterial == GolemMaterial.Stone)
        {
            golem.insideLava = true;
            return;
        }

        player.Die();
    }

    private void OnTriggerExit(Collider other)
    {
        // If what entered was not the golem or the beetle
        if (!other.CompareTag("Golem") && !other.CompareTag("Beetle"))
            return;

        // If the deathpit is lava, and the golem got inside it
        if (CompareTag("Lava") && player.currentCharacter.TryGetComponent(out GolemBehaviour golem) && golem.currentMaterial == GolemMaterial.Stone)
            golem.insideLava = false;
    }
}
