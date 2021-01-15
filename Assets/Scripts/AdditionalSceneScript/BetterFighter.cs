using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterFighter : MonoBehaviour
{
    #region Encapsulation
    //refactored fighter
    public WeaponObject weaponObject;
    public WeaponObject leftHandWeapon;
    [HideInInspector] Animator animator;
    [HideInInspector] PlayerStats stats;
    [SerializeField] float attackSpeed = 1.5f;
    [HideInInspector] AnimationPlayer animationPlayer;
    [SerializeField] KeyCode consumableUseKey = KeyCode.Q;
    public Transform equipmentParent;
    public Transform leftHandParent;
    public Transform consumableParent;
    public Transform emptryThrowTarget;
    private int index = -1;
    [HideInInspector] private bool twoHand = false;
    [SerializeField] KeyCode forwardKey = KeyCode.O;
    [SerializeField] KeyCode backKey = KeyCode.L;
    [SerializeField] KeyCode pickupKey = KeyCode.E;
    [SerializeField] KeyCode blockKey = KeyCode.R;
    [HideInInspector] BetterInventory inventory;
    public List<WeaponObject> weaponsInBackPack = new List<WeaponObject>();
    string lastAttack = "";
    const string lightAttack1 = "LightAttack1";
    const string lightAttack2 = "LightAttack2";
    const string heavyAttack1 = "HeavyAttack1";
    const string heavyAttack2 = "HeavyAttack2";
    bool canDoCombo;
    bool isHoldingShield;
    bool comboFlag;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        isHoldingShield = false;
        animator = GetComponent<Animator>();
        stats = GetComponent<PlayerStats>();
        inventory = GetComponent<BetterInventory>();
        //handle first weapon as well
        if (index == -1)
        {
            CycleWeapon(true);
        }
        animationPlayer = FindObjectOfType<AnimationPlayer>();
    }
    // Update is called once per frame
    void Update()
    {
        ShieldLogic();
        canDoCombo = animationPlayer.GetAnimator().GetBool("CanDoCombo");
        ConsumeOrThrow(stats);
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
                weaponObject.EquipOn(false, equipmentParent);
            }
        }
        if (weaponObject != null)
        {
            if (!gameObject.GetComponent<BetterInventory>().WeaponsInventoryIsOpen() && gameObject.GetComponent<BetterInventory>().InventoryIsOpen() == false && gameObject.GetComponent<BetterInventory>().ClothInventoryIsOpen() == false && !gameObject.GetComponent<BetterInventory>().ConsumableInventoryIsOpen())
            {
                AttackBehavior();
            }
        }
    }
    void ShieldLogic()
    {
        Vector3 shieldOnPos = new Vector3(0.07622319f, -0.1480567f, -0.121954f);
        Vector3 shieldOnRot = new Vector3(-10.499f, -253.194f, 101.297f);   
        bool attackAnimPlaying = this.animator.GetCurrentAnimatorStateInfo(2).IsName(lightAttack1) || this.animator.GetCurrentAnimatorStateInfo(2).IsName(lightAttack2) || this.animator.GetCurrentAnimatorStateInfo(2).IsName(heavyAttack1) || this.animator.GetCurrentAnimatorStateInfo(2).IsName(heavyAttack2);
        if (!twoHand)
        {
            if (Input.GetKeyDown(blockKey) && !attackAnimPlaying)
            {
                isHoldingShield = !isHoldingShield;
                if (isHoldingShield)
                {
                    animationPlayer.PlayerTargetAnim("PutOnShield", false);
                    if (leftHandWeapon != null)
                    {
                        leftHandWeapon.GetInstance().transform.localEulerAngles = shieldOnRot;
                        leftHandWeapon.GetInstance().transform.localPosition = shieldOnPos;
                    }
                }
                else
                {
                    if (leftHandWeapon != null)
                    {
                        leftHandWeapon.GetInstance().transform.localEulerAngles = leftHandWeapon.oneHandedEquipmentRotation;
                        leftHandWeapon.GetInstance().transform.localPosition = leftHandWeapon.oneHandedEquipmentPosition;
                    }
                }
            }
            if (leftHandWeapon != null)
            {
                if (isHoldingShield)
                {
                    animator.SetLayerWeight(3, 1);
                }
                else
                {
                    animator.SetLayerWeight(3, 0);
                }
            }
        }
        else
        {
            animator.SetLayerWeight(3, 0);
        }
        if (attackAnimPlaying)
        {
            isHoldingShield = false;
        }
    }
    void AttackBehavior()
    {
        SwitchComboCheck();
        if (Input.GetMouseButtonDown(0))
        {
            isHoldingShield = false;
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
            isHoldingShield = false;
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
        if (this.animator.GetCurrentAnimatorStateInfo(2).IsName(lightAttack1))
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
        else if (this.animator.GetCurrentAnimatorStateInfo(2).IsName(heavyAttack1))
        {
            if (Input.GetMouseButtonDown(0))
            {
                animationPlayer.PlayerTargetAnim(lightAttack1, true);
                lastAttack = lightAttack1;
            }
        }
        bool x = this.animator.GetCurrentAnimatorStateInfo(2).IsName(lightAttack1) || this.animator.GetCurrentAnimatorStateInfo(2).IsName(lightAttack2) || this.animator.GetCurrentAnimatorStateInfo(2).IsName(heavyAttack1) || this.animator.GetCurrentAnimatorStateInfo(2).IsName(heavyAttack2);
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
            weaponObject.EquipOn(false, equipmentParent);
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
        if (other.tag == "Weapon pickup")
        {
            if (Input.GetKeyDown(pickupKey))
            {
                twoHand = false;
                if (other.GetComponent<BetterPickup>().IsPickedUp() == false)
                {
                    weaponsInBackPack.Add(other.GetComponent<BetterPickup>().Pickup());
                }
                Destroy(other.gameObject);
            }
        }
    }
    public string GetLastAttack()
    {
        return lastAttack;
    }
    public void EquipWeaponFromInventory(int indexToEquip)
    {
        if (weaponObject != null)
        {
            Unequip(weaponObject);
        }
        weaponObject = weaponsInBackPack[indexToEquip];
        animator.runtimeAnimatorController = weaponObject.weaponOvveride;
        weaponObject.EquipOn(false, equipmentParent);
    }
    public void LeftHandEquipment(int indexToEquip)
    {
        if(leftHandWeapon != null && leftHandWeapon.isLeftHandWeapon)
        {
            Unequip(leftHandWeapon);
        }
        leftHandWeapon = weaponsInBackPack[indexToEquip];
        leftHandWeapon.EquipOn(false, leftHandParent);
    }
    void ConsumeOrThrow(PlayerStats stats)
    {
        //to do 
        if (Input.GetKeyDown(consumableUseKey))
        {
            if (animator.GetBool("IsInteracting"))
            {
                return;
            }
            if (inventory.GetCurrentConsumable() != null)
            {
                if (!inventory.GetCurrentConsumable().isThrowable && !inventory.GetCurrentConsumable().isAWeaponBoost)
                {
                    animationPlayer.PlayerTargetAnim(inventory.GetCurrentConsumable().consumingAnimationName, true);
                }
                else if(inventory.GetCurrentConsumable().isThrowable && !inventory.GetCurrentConsumable().isAWeaponBoost)
                {
                    //throw anim and calculations
                    try
                    {
                        animationPlayer.PlayerTargetAnim(inventory.GetCurrentConsumable().consumingAnimationName, true);
                    }
                    catch
                    {
                        animationPlayer.PlayerTargetAnim("Throw", true);
                    }
                    if(inventory.GetCurrentConsumable().GetConsumableInstance() != null)
                    {
                        //fly towards
                        if (gameObject.GetComponent<LockOn>().GetLockState() == true)
                        {
                            if (gameObject.GetComponent<LockOn>().lookAtTransform != null)
                            {
                                Transform assignedTarget = gameObject.GetComponent<LockOn>().lookAtTransform;
                                inventory.GetCurrentConsumable().GetConsumableInstance().GetComponent<FlyingProjectileScript>().target = assignedTarget;
                                
                                inventory.GetCurrentConsumable().GetConsumableInstance().GetComponent<FlyingProjectileScript>().LaunchProjectile();
                            }
                        }
                    }
                }
            }
        }
    }
    #region ConsumeAnimEvents
    public void DrinkConsumableSpawnBottleEvent()
    {
        if (inventory.GetCurrentConsumable() != null)
        {
            inventory.GetCurrentConsumable().SpawnConsumableAt(consumableParent);
        }
    }
    public void SpawnGlowConsumableEvent()
    {
        if (inventory.GetCurrentConsumable() != null)
        {
            inventory.GetCurrentConsumable().PlayConsumptionEffect(this.transform);
        }
    }
    public void IncreaseStatByConsumableAnimEvent()
    {
        if (inventory.GetCurrentConsumable() != null)
        {
            if(inventory.GetCurrentConsumable().increaseType == ConsumableObject.IncreaseType.health)
            {
                stats.AddHealth(inventory.GetCurrentConsumable().increase);
            }
            if (inventory.GetCurrentConsumable().increaseType == ConsumableObject.IncreaseType.stamina)
            {
                stats.AddStamina(inventory.GetCurrentConsumable().increase);
            }

            Destroy(inventory.GetCurrentConsumable().GetConsumableInstance());
            Destroy(inventory.GetCurrentConsumable().GetParticleInstance());
        }
    }
    public void AnimLaunchProjectileFromFighter(int phase)
    {
        if (phase == 1)
        {
            if (inventory.GetCurrentConsumable().GetConsumableInstance() == null)
            {
                inventory.GetCurrentConsumable().SpawnConsumableAt(consumableParent);
            }
        }
        else if(phase == 2)
        {
            inventory.GetCurrentConsumable().GetConsumableInstance().transform.parent = null;
        }

    }
    #endregion
}
