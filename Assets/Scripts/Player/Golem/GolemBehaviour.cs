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

    [Space]

    [Header("Hit control")]
    // Hit Window
    [SerializeField]
    [Tooltip("How long the collider stays active after pressing hit, in seconds")]
    private float hitWindow;

    private float hitWindowTimer;

    private bool inHitWindow;

    // CoolDown
    private float coolDownTimer;

    private bool inCoolDown;

    [SerializeField]
    private Collider hitRegister;

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

    [HideInInspector]
    public bool insideLava;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
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

        // Player presses hit. Hit window starts
        if (hitPressed && !inCoolDown)
        {
            hitRegister.enabled = true;
            inHitWindow = true;
            inCoolDown = true;
            hitWindowTimer = 0.0f;
            coolDownTimer = 0.0f;
        }

        if (inHitWindow)
        {
            hitWindowTimer += Time.deltaTime;
            if (hitWindowTimer >= hitWindow)
            {
                // The hit window ends
                hitRegister.enabled = false;
                inHitWindow = false;
            }
        }

        if (inCoolDown)
        {
            coolDownTimer += Time.deltaTime;
            if (coolDownTimer >= golemStats.hitCoolDown)
            {
                // The cooldown ends
                inCoolDown = false;
            }
        }
    }

    public void ChangeMaterial(GolemMaterial newMaterial)
    {
        currentMaterial = newMaterial;

        Debug.Log(currentMaterial);
    }
}
