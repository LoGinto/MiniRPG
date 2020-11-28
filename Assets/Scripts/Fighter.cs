using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulItem;

public class Fighter : MonoBehaviour
{
    AnimationPlayer animationPlayer;
    public string lastAttack;
    PlayerInventory inventory;
    WeaponSlotManager slotManager;
    PlayerStats stats;
    bool canDoCombo;
    bool comboFlag;
    bool handleTwoHand = false;
    bool twoHandInput;
    private void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
        slotManager = FindObjectOfType<WeaponSlotManager>();      
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
            else if(lastAttack == weapon.On_Light_THA_01)
            {
                //play light attack number 2
                animationPlayer.PlayerTargetAnim(weapon.On_Light_THA_02, true);
            }
            else if(lastAttack == weapon.On_Heavy_THA_01)
            {
                //play heavy attack number 2 
                animationPlayer.PlayerTargetAnim(weapon.On_Heavy_THA_02, true);
            }
        }
        
    }
    public void HandleLightAttack(WeaponItem weapon)
    {
        slotManager.SetAttackingWeaponItem(weapon);
        if (handleTwoHand)
        {
            animationPlayer.PlayerTargetAnim(weapon.On_Light_THA_01,true);
            lastAttack = weapon.On_Light_THA_01;
        }
        else
        {
            animationPlayer.PlayerTargetAnim(weapon.On_Light_Attack_1, true);
            lastAttack = weapon.On_Light_Attack_1;
        }
    }
    public void HandleHeavyAttack(WeaponItem weapon)
    {
        slotManager.SetAttackingWeaponItem(weapon);
        if (handleTwoHand) {
            animationPlayer.PlayerTargetAnim(weapon.On_Heavy_THA_01, true);
            lastAttack = weapon.On_Heavy_THA_01;
        }
        else
        {
            animationPlayer.PlayerTargetAnim(weapon.On_Heavy_Attack_1, true);
            lastAttack = weapon.On_Heavy_Attack_1;
        }
    }
    private void Update()
    {
        canDoCombo = animationPlayer.GetAnimator().GetBool("CanDoCombo");
        if (gameObject.GetComponent<Movement>().GetRollFlag() == false)
        {
            HandleAttacks();
        }
        HandleTwoHandInput(); 
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
    void HandleTwoHandInput()
    {
        twoHandInput = Input.GetKeyDown(KeyCode.T);
        if (twoHandInput == true)
        {
            handleTwoHand = !handleTwoHand;
        }
        if (handleTwoHand)
        {
            slotManager.LoadWeaponOnSlot(inventory.rightWeapon, false);
        }
        else
        {
            slotManager.LoadWeaponOnSlot(inventory.rightWeapon, false);
            slotManager.LoadWeaponOnSlot(inventory.leftWeapon, true);
        }
    }
    public bool GetHandlingWithTwoHandValue()
    {
        return handleTwoHand;
    }
}
