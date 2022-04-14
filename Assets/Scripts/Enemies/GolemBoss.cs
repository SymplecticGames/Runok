using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBoss : MonoBehaviour
{
    [SerializeField]
    public bool beaten;

    [SerializeField]
    private GolemHeadBoss head;

    [SerializeField]
    private GolemArmBoss leftArm;

    [SerializeField]
    private GolemArmBoss rightArm;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.player.input.actions.FindAction("SwapCharacter").Disable();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void EndOfHit()
    {
        head.EndOfHit();
    }
}
