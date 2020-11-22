using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulItem;

public class Fighter : MonoBehaviour
{
    AnimationPlayer animationPlayer;
    public string lastAttack;
    PlayerInventory inventory;
    PlayerStats stats;
    bool canDoCombo;
    bool comboFlag;
    private void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
    }
    private void Start()
    {
        stats = FindObjectOfType<PlayerStats>();
        //animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        animationPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<AnimationPlayer>();
    }
    void HandleWeaponCombo(WeaponItem weapon)
    {
        if (comboFlag)
        {
            animationPlayer.GetAnimator().SetBool("CanDoCombo", false);
            if (lastAttack == weapon.On_Light_Attack_1)
            {
                animationPlayer.PlayerTargetAnim(weapon.On_Light_Attack_2, true);
            }
            else if(lastAttack == weapon.On_Heavy_Attack_1)
            {
                animationPlayer.PlayerTargetAnim(weapon.On_Heavy_Attack_2, true);
            }
        }
    }
    public void HandleLightAttack(WeaponItem weapon)
    {
        animationPlayer.PlayerTargetAnim(weapon.On_Light_Attack_1, true);
        lastAttack = weapon.On_Light_Attack_1;
    }
    public void HandleHeavyAttack(WeaponItem weapon)
    {
        animationPlayer.PlayerTargetAnim(weapon.On_Heavy_Attack_1, true);
        lastAttack = weapon.On_Heavy_Attack_1;
    }
    private void Update()
    {
        canDoCombo = animationPlayer.GetAnimator().GetBool("CanDoCombo");
        if (gameObject.GetComponent<Movement>().GetRollFlag() == false)
        {
            HandleAttacks();
        }
    }
    public bool GetCanDoCombo()
    {
        return canDoCombo;
    }
    void HandleAttacks()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (stats.currentStamina >= inventory.rightWeapon.baseStaminaDrain * inventory.rightWeapon.lightAttackMultiplier)
            {
                
                if (canDoCombo)
                {
                    comboFlag = true;
                    HandleWeaponCombo(inventory.rightWeapon);
                    comboFlag = false;
                }
                else
                {
                    if (canDoCombo)
                        return;
                    if (animationPlayer.GetAnimator().GetBool("IsInteracting"))
                        return;
                    HandleLightAttack(inventory.rightWeapon);
                }
            }

        }
        if (Input.GetMouseButtonDown(1))
        {
            if (stats.currentStamina >= inventory.rightWeapon.baseStaminaDrain * inventory.rightWeapon.heavyAttackMultiplier)
            {
                
                if (canDoCombo)
                {
                    comboFlag = true;
                    HandleWeaponCombo(inventory.rightWeapon);
                    comboFlag = false;
                }
                else
                {
                    if (canDoCombo)
                        return;
                    if (animationPlayer.GetAnimator().GetBool("IsInteracting"))
                        return;
                    HandleHeavyAttack(inventory.rightWeapon);
                }
            }
        }
    }
    public void TakeHeavyStaminaAnimationEvent()
    {
        stats.TakeStamina(inventory.rightWeapon.baseStaminaDrain * inventory.rightWeapon.heavyAttackMultiplier);
    }
    public void TakeLightStaminaAnimationEvent()
    {
        stats.TakeStamina(inventory.rightWeapon.baseStaminaDrain * inventory.rightWeapon.lightAttackMultiplier);
    }
}
