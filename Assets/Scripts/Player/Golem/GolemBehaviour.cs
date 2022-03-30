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

    // CoolDown
    private float coolDownTimer;

    private bool inCoolDown;

    [SerializeField]
    private Collider leftHitRegister;

    [SerializeField]
    private Collider rightHitRegister;

    [HideInInspector]
    public bool hitPressed;

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

    private Animator animator;

    [HideInInspector]
    public bool continueCombo = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    // Update is called once per frame
    void Update()
    {
        animator.SetBool("continueCombo", continueCombo);

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
    }

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

    public void ChangeMaterial(GolemMaterial newMaterial)
    {
        currentMaterial = newMaterial;

        Debug.Log(currentMaterial);
    }

    private void ResetHitCombo()
    {
        continueCombo = true;
    }
}
