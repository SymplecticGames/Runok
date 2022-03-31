using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GolemMaterial
{
    Terracotta = 1,
    Plumber = 2,
    Wooden = 3
}

[System.Serializable]
public struct GolemMaterialStats
{
    public GolemMaterialStats(int hits, float coolDown, int jump, float weight)
    {
        this.hitsNeededToKill = hits;
        this.hitCoolDown = coolDown;
        this.jumps = jump;
        this.weight = weight;
    }

    public int hitsNeededToKill;
    public float hitCoolDown;
    public int jumps;
    public float weight;
}

public class GolemBehaviour : MonoBehaviour
{
    public Transform beetleRestPose;

    [Header("Hit control")]

    [SerializeField]
    private Collider leftHitRegister;

    [SerializeField]
    private Collider rightHitRegister;

    [Header("Materials")]
    public GolemMaterial currentMaterial;

    [HideInInspector]
    public GolemMaterialStats golemStats;

    // All materials default stats
    [SerializeField]
    private GolemMaterialStats TerracottaStats;

    [SerializeField]
    private GolemMaterialStats PlumberStats;

    [SerializeField]
    private GolemMaterialStats WoodenStats;

    private GenericBehaviour genericBehaviour;

    private Animator animator;

    private bool canDoCombo = true;

    private int comboCounter;

    private float lastComboHitTime;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        genericBehaviour = GetComponent<GenericBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        // Material change
        switch (currentMaterial)
        {
            case GolemMaterial.Terracotta:
                golemStats = TerracottaStats;
                break;

            case GolemMaterial.Plumber:
                golemStats = PlumberStats;
                break;

            case GolemMaterial.Wooden:
                golemStats = WoodenStats;
                break;
        }

        // Material dependant stats
        genericBehaviour.movementFactor = 1.0f / golemStats.weight;
        genericBehaviour.maxJumps = genericBehaviour.controller.isGrounded ? golemStats.jumps : golemStats.jumps - 1;

        // Animator controller update
        animator.SetBool("canDoCombo", canDoCombo);
        animator.SetInteger("comboCounter", comboCounter);

        // Hit cooldown control
        if (Time.time - lastComboHitTime > golemStats.hitCoolDown)
        {
            comboCounter = 0;
            canDoCombo = true;
        }
    }

    public void GolemHit()
    {
        if (!genericBehaviour.controller.isGrounded || !canDoCombo || comboCounter >= 3)
            return;

        canDoCombo = false;
        lastComboHitTime = Time.time;
        comboCounter++;
    }

    public void ChangeMaterial(GolemMaterial newMaterial)
    {
        currentMaterial = newMaterial;

        Debug.Log(currentMaterial);
    }

    #region Animation Events

    public void EnableLeftHitWindow()
    {
        leftHitRegister.enabled = true;
    }

    public void DisableLeftHitWindow()
    {
        leftHitRegister.enabled = false;
    }

    public void EnableRightHitWindow()
    {
        rightHitRegister.enabled = true;
    }

    public void DisableRightHitWindow()
    {
        rightHitRegister.enabled = false;
    }

    private void ResetHitCombo()
    {
        canDoCombo = true;
    }

    #endregion
}
