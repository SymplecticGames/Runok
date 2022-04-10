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

    [SerializeField]
    private SkinnedMeshRenderer golemMesh;

    [SerializeField]
    private Material[] golemBaseMaterials;

    private Material targetMaterial;

    private bool isChangingMaterial;

    private float materialLerpStep;

    private Material previousMaterial;

    private Weight weight;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        genericBehaviour = GetComponent<GenericBehaviour>();
        weight = GetComponent<Weight>();

        ChangeMaterial(currentMaterial);
    }

    // Update is called once per frame
    void Update()
    {
        // Material dependant stats
        weight.weightValue = golemStats.weight * 10;
        genericBehaviour.movementFactor = -.2f + 1.0f / golemStats.weight;
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

        // Material lerp (Not working)
        //if (isChangingMaterial)
        //    if (materialLerpStep < 1.0f)
        //    {
        //        materialLerpStep += Time.deltaTime;
        //        golemMesh.material.Lerp(previousMaterial, targetMaterial, materialLerpStep);
        //    }
        //    else
        //    {
        //        golemMesh.material = targetMaterial;
        //        isChangingMaterial = false;
        //    }
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
        previousMaterial = golemBaseMaterials[(int)currentMaterial - 1];
        targetMaterial = golemBaseMaterials[(int)newMaterial - 1];

        isChangingMaterial = true;
        materialLerpStep = 0.0f;

        golemMesh.material = targetMaterial;

        switch (newMaterial)
        {
            case GolemMaterial.Terracotta:
                golemStats = TerracottaStats;

                animator.SetLayerWeight(0, 1.0f);
                animator.SetLayerWeight(1, 0.0f);
                animator.SetLayerWeight(2, 0.0f);
                break;
            case GolemMaterial.Plumber:
                golemStats = PlumberStats;

                animator.SetLayerWeight(0, 0.0f);
                animator.SetLayerWeight(1, 1.0f);
                animator.SetLayerWeight(2, 0.0f);
                break;
            case GolemMaterial.Wooden:
                golemStats = WoodenStats;

                animator.SetLayerWeight(0, 0.0f);
                animator.SetLayerWeight(1, 0.0f);
                animator.SetLayerWeight(2, 1.0f);
                break;
        }

        currentMaterial = newMaterial;
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

    public void PlayHitSound()
    {
        AudioManager.audioInstance.PlayCharSound(CharaudioTag.punchAir);
    }

    public void PlayHitSomethingSound()
    {
        switch (currentMaterial)
        {
            case GolemMaterial.Terracotta:
                AudioManager.audioInstance.PlayCharSound(CharaudioTag.terracottaHit);
                break;
            case GolemMaterial.Plumber:
                AudioManager.audioInstance.PlayCharSound(CharaudioTag.plumberHit);
                break;
            case GolemMaterial.Wooden:
                AudioManager.audioInstance.PlayCharSound(CharaudioTag.woodenHit);
                break;
        }
    }

    public void PlayStompSound()
    {
        if (genericBehaviour.controller.isGrounded){
            switch (currentMaterial)
            {
                case GolemMaterial.Terracotta:
                    AudioManager.audioInstance.PlayCharSound(CharaudioTag.terracottaStomp);
                    break;
                case GolemMaterial.Plumber:
                    AudioManager.audioInstance.PlayCharSound(CharaudioTag.plumberStomp);
                    break;
                case GolemMaterial.Wooden:
                    AudioManager.audioInstance.PlayCharSound(CharaudioTag.woodenStomp);
                    break;
            }
        }
    }
    
    public void PlayLandSound()
    {
        if (genericBehaviour.controller.isGrounded){
            AudioManager.audioInstance.SetAudioSourcePitch(AudioManager.audioInstance.GetAudioSource(), 1.5f);
            switch (currentMaterial)
            {
                case GolemMaterial.Terracotta:
                    AudioManager.audioInstance.PlayCharSound(CharaudioTag.terracottaStomp);
                    break;
                case GolemMaterial.Plumber:
                    AudioManager.audioInstance.PlayCharSound(CharaudioTag.plumberStomp);
                    
                    break;
                case GolemMaterial.Wooden:
                    AudioManager.audioInstance.PlayCharSound(CharaudioTag.woodenStomp);
                    break;
            }

            StartCoroutine(AudioManager.audioInstance.ResetPitch(AudioManager.audioInstance.GetAudioSource(),0.2f));
        }
    }
    
    
    private void ResetHitCombo()
    {
        canDoCombo = true;
    }

    #endregion
}
