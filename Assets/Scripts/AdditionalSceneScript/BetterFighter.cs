using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterFighter : MonoBehaviour
{
    //refactored fighter
    public WeaponObject weaponObject;
    [HideInInspector]Animator animator;
    [HideInInspector]PlayerStats stats;
    [SerializeField] float attackSpeed = 1.5f;
    [HideInInspector] AnimationPlayer animationPlayer;
    public Transform equipmentParent;
    private int index = -1;
    [HideInInspector]private bool twoHand = false;
    [SerializeField] KeyCode forwardKey = KeyCode.O;
    [SerializeField] KeyCode backKey = KeyCode.L;
    [SerializeField] KeyCode pickupKey = KeyCode.E;
    public List<WeaponObject> weaponsInBackPack = new List<WeaponObject>();
    string lastAttack = "";
    const string lightAttack1 = "LightAttack1";
    const string lightAttack2 = "LightAttack2";
    const string heavyAttack1 = "HeavyAttack1";
    const string heavyAttack2 = "HeavyAttack2";
    bool canDoCombo;
    bool comboFlag;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        stats = GetComponent<PlayerStats>();
        //handle first weapon as well
        if(index == -1)
        {
            CycleWeapon(true);
        }
        animationPlayer = FindObjectOfType<AnimationPlayer>();
    }
    // Update is called once per frame
    void Update()
    {
        canDoCombo = animationPlayer.GetAnimator().GetBool("CanDoCombo");
        if (Input.GetKeyDown(KeyCode.T))
        {
            twoHand = !twoHand;
        }
            SwitchToTwoHand(twoHand);
        if (Input.GetKeyDown(forwardKey))
        {
            try
            {
                CycleWeapon(true);
            }
            catch
            {
                index = 0;
                weaponObject.EquipOn(false, equipmentParent);
            }
        }
        else if (Input.GetKeyDown(backKey))
        {
            try
            {
                CycleWeapon(false);
            }
            catch
            {
                index = 0;
                weaponObject.EquipOn(false,equipmentParent);
            }
        }
        if (weaponObject != null)
        {
            AttackBehavior();
        }
    }
    void AttackBehavior()
    {
        SwitchComboCheck();
        if (Input.GetMouseButtonDown(0))
        {
           
            if (stats.currentStamina >= weaponObject.baseStaminaDrain * weaponObject.lightAttackMultiplier)
            {

                if (canDoCombo)
                {
                    comboFlag = true;
                    HandleWeaponCombo();
                    comboFlag = false;
                }
                else
                {
                    if (canDoCombo)
                        return;
                    if (animationPlayer.GetAnimator().GetBool("IsInteracting"))
                        return;
                    HandleLightAttack();
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            

            if (stats.currentStamina >= weaponObject.baseStaminaDrain * weaponObject.heavyAttackMultiplier)
            {

                if (canDoCombo)
                {
                    comboFlag = true;
                    HandleWeaponCombo();
                    comboFlag = false;
                }
                else
                {
                    if (canDoCombo)
                        return;
                    if (animationPlayer.GetAnimator().GetBool("IsInteracting"))
                        return;
                    HandleHeavyAttack();
                }
            }
        }        
    }
    public void TakeHeavyStaminaAnimationEvent()
    {
        stats.TakeStamina(weaponObject.baseStaminaDrain * weaponObject.heavyAttackMultiplier);
    }
    public void TakeLightStaminaAnimationEvent()
    {
        stats.TakeStamina(weaponObject.baseStaminaDrain * weaponObject.lightAttackMultiplier);
    }
    void SwitchComboCheck()
    {
        if (this.animator.GetCurrentAnimatorStateInfo(1).IsName(lightAttack1))
        {
            if (canDoCombo)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    animationPlayer.PlayerTargetAnim(heavyAttack1, true);
                    lastAttack = heavyAttack1;
                }
            }
        }
        else if (this.animator.GetCurrentAnimatorStateInfo(1).IsName(heavyAttack1))
        {
            if (Input.GetMouseButtonDown(0))
            {
                animationPlayer.PlayerTargetAnim(lightAttack1, true);
                lastAttack = lightAttack1;
            }
        }
        bool x = this.animator.GetCurrentAnimatorStateInfo(1).IsName(lightAttack1) || this.animator.GetCurrentAnimatorStateInfo(1).IsName(lightAttack2) || this.animator.GetCurrentAnimatorStateInfo(1).IsName(heavyAttack1) || this.animator.GetCurrentAnimatorStateInfo(1).IsName(heavyAttack2);
        if (x)
        {
            animator.speed = attackSpeed;
        }
        else
        {
            animator.speed = 1;
        }
    }
    void HandleWeaponCombo()
    {
        if (comboFlag)
        {
            animationPlayer.GetAnimator().SetBool("CanDoCombo", false);
            if (lastAttack == lightAttack1)
            {                              
                //else
                //{
                    animationPlayer.PlayerTargetAnim(lightAttack2, true);
               // }
            }
            else if (lastAttack == heavyAttack1)
            {                                
                //else
                //{
                    animationPlayer.PlayerTargetAnim(heavyAttack2, true);
                //}
            }
        }
    }
    void HandleLightAttack()
    {
        animationPlayer.PlayerTargetAnim(lightAttack1, true);
        lastAttack = lightAttack1;
    }
    void HandleHeavyAttack()
    {
        animationPlayer.PlayerTargetAnim(heavyAttack1, true);
        lastAttack = heavyAttack1;
    }
    void CycleWeapon(bool forward)
    {
        //handle bounds, to do
        if (forward)
        {
            index++;
            if (weaponObject != null)        
            {
                Unequip(weaponObject);
            }
            weaponObject = weaponsInBackPack[index];
            animator.runtimeAnimatorController = weaponObject.weaponOvveride;
            weaponObject.EquipOn(false,equipmentParent);
        }
        else // also I have to destroy previous instance, to do
        {
            index--;
            if (weaponObject != null)
            {
                Unequip(weaponObject);
            }
            weaponObject = weaponsInBackPack[index];
            animator.runtimeAnimatorController = weaponObject.weaponOvveride;
            weaponObject.EquipOn(false, equipmentParent);
        }
    }
    void SwitchToTwoHand(bool toTwoHand)
    {
        if (toTwoHand)
        {
            animator.runtimeAnimatorController = weaponObject.twoHandWeaponOvveride;
            if (weaponObject.GetInstance() != null) {
                weaponObject.HandTransformRegulator(weaponObject.GetInstance(), twoHand);
                }
        }
        else
        {
            animator.runtimeAnimatorController = weaponObject.weaponOvveride;
            if (weaponObject.GetInstance() != null)
            {
                weaponObject.HandTransformRegulator(weaponObject.GetInstance(), twoHand);
            }
        }
    }
    void Unequip(WeaponObject weaponObject)
    {
        Destroy(weaponObject.GetInstance());
    }
    private void OnTriggerStay(Collider other)
    {
        //maybe some ui here as well
        if(other.tag == "Weapon pickup")
        {
            if (Input.GetKeyDown(pickupKey))
            {
                twoHand = false;
                weaponsInBackPack.Add(other.GetComponent<BetterPickup>().Pickup());
                Destroy(other.gameObject);
            }
        }
    }
    public string GetLastAttack()
    {
        return lastAttack;
    }
}
