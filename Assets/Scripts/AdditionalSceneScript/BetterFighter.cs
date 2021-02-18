using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterFighter : MonoBehaviour
{
    #region Encapsulation
    //refactored fighter
    public WeaponObject weaponObject;
    public WeaponObject leftHandWeapon;
    [SerializeField] int bulletAmt = 20;
    [HideInInspector] Animator animator;
    [HideInInspector] PlayerStats stats;
    [SerializeField] float attackSpeed = 1.5f;
    [HideInInspector] AnimationPlayer animationPlayer;
    [SerializeField] KeyCode consumableUseKey = KeyCode.Q;
    public Transform equipmentParent;
    public Transform leftHandParent;
    public Transform consumableParent;
    public Transform emptryThrowTarget;
    public Transform criticalAttackRayPoint;
    public float criticalDmg = 3000;
    [HideInInspector]private int index = -1;
    [HideInInspector] private bool twoHand = false;
    [SerializeField] KeyCode forwardKey = KeyCode.O;
    [SerializeField] KeyCode backKey = KeyCode.L;
    [SerializeField] KeyCode pickupKey = KeyCode.E;
    [SerializeField] KeyCode blockKey = KeyCode.R;
    [HideInInspector] BetterInventory inventory;
    public List<WeaponObject> weaponsInBackPack = new List<WeaponObject>();
    string lastAttack = "";
    [HideInInspector]const string lightAttack1 = "LightAttack1";
    [HideInInspector]const string lightAttack2 = "LightAttack2";
    [HideInInspector]const string heavyAttack1 = "HeavyAttack1";
    [HideInInspector]const string heavyAttack2 = "HeavyAttack2";
    [HideInInspector]bool canDoCombo;
    [HideInInspector]bool isHoldingShield;
    [HideInInspector]bool comboFlag;
    [HideInInspector] bool attackAnimPlaying;
    #endregion
    #region start and update
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
    void Update()
    {
        #region update. No need to thank me
        if (leftHandWeapon != null)
        {
            ShieldLogic();
            RifleLogic();
        }
        attackAnimPlaying = this.animator.GetCurrentAnimatorStateInfo(2).IsName(lightAttack1) || this.animator.GetCurrentAnimatorStateInfo(2).IsName(lightAttack2) || this.animator.GetCurrentAnimatorStateInfo(2).IsName(heavyAttack1) || this.animator.GetCurrentAnimatorStateInfo(2).IsName(heavyAttack2);
        if (Input.GetKeyDown(KeyCode.F))
        {
            AttemptCritical();
        }
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
        #endregion
    }
    #endregion
    #region left hands
    #region Rifle
    void RifleLogic()
    {
        //bool attackAnimPlaying = this.animator.GetCurrentAnimatorStateInfo(2).IsName(lightAttack1) || this.animator.GetCurrentAnimatorStateInfo(2).IsName(lightAttack2) || this.animator.GetCurrentAnimatorStateInfo(2).IsName(heavyAttack1) || this.animator.GetCurrentAnimatorStateInfo(2).IsName(heavyAttack2);
        try
        {
            if (leftHandWeapon != null)
            {
                if (leftHandWeapon.isRanged)
                {
                    if (Input.GetKeyDown(blockKey))
                    {
                        animator.SetLayerWeight(4, 1);
                        if (bulletAmt > 0)
                        {
                            if (!attackAnimPlaying)
                            {
                                animationPlayer.PlayerTargetAnim("GunShoot", true);
                                bulletAmt--;
                            }                                                 
                        }
                    }
                    if (this.animator.GetCurrentAnimatorStateInfo(4).IsName("GunShoot"))
                    {
                        animator.SetLayerWeight(4, 1);
                        leftHandWeapon.GetInstance().transform.localPosition = new Vector3(-0.131f, 0.032f, -0.111f);
                        leftHandWeapon.GetInstance().transform.localEulerAngles = new Vector3(-33.384f, 221.004f, -24.91f);
                    }
                    else
                    {
                        animator.SetLayerWeight(4, 0);
                        leftHandWeapon.GetInstance().transform.localPosition = leftHandWeapon.oneHandedEquipmentPosition;
                        leftHandWeapon.GetInstance().transform.localEulerAngles = leftHandWeapon.oneHandedEquipmentRotation;
                    }
                }
                else
                {
                    animator.SetLayerWeight(4, 0);
                }
            }
        }
        catch
        {
            Debug.Log("Problem with gun");
        }
    }
    public void ShootGun()
    {
        if(leftHandWeapon != null)
        {
            animator.SetFloat("Move", 0); 
            if (leftHandWeapon.isRanged)
            {
                leftHandWeapon.GetInstance().GetComponent<Shotgun>().ShootTheGun();
            }
        }
    }
    public int GetBulletAmt()
    {
        return bulletAmt; 
    }
    #endregion
    // Update is called once per frame
    #region Parry
    public void ParryAnimEvent()
    {
        if (leftHandWeapon != null) {
            if (leftHandWeapon.isAShield)
            {
                leftHandWeapon.GetInstance().GetComponentInChildren<ShieldParry>().Parry();
            }
        }
    }
    public void UnParryEvent()
    {
        if (leftHandWeapon != null)
        {
            if (leftHandWeapon.isAShield)
            {
                leftHandWeapon.GetInstance().GetComponentInChildren<ShieldParry>().BackToNormal();
            }
        }
    }
    void AttemptCriticalParry()
    {
        RaycastHit hit;
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Physics.Raycast(criticalAttackRayPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.6f))
            {
                if (leftHandWeapon != null)
                {
                    if (leftHandWeapon.isAShield)
                    {
                        if (hit.collider.GetComponent<Enemy>().isStun)
                        {
                            animationPlayer.PlayerTargetAnim("BackStab", true);
                            hit.collider.GetComponent<Enemy>().EnemyTargetAnim("IsParried", true);
                            hit.collider.GetComponent<Enemy>().EnemyInterruptingAnim("IsParried");
                        }
                        else
                        {
                            AttemptCritical(); 
                        }
                    }
                }
            }
        }
    }
    #endregion
    void ShieldLogic()
    {
        AttemptCriticalParry(); 
        Vector3 shieldOnPos = new Vector3(0.07622319f, -0.1480567f, -0.121954f);
        Vector3 shieldOnRot = new Vector3(-10.499f, -253.194f, 101.297f);
       // bool attackAnimPlaying = this.animator.GetCurrentAnimatorStateInfo(2).IsName(lightAttack1) || this.animator.GetCurrentAnimatorStateInfo(2).IsName(lightAttack2) || this.animator.GetCurrentAnimatorStateInfo(2).IsName(heavyAttack1) || this.animator.GetCurrentAnimatorStateInfo(2).IsName(heavyAttack2);
        if (!twoHand)
        {
            if (Input.GetKeyDown(blockKey) && !attackAnimPlaying)
            {
                #region Shield up/down
                if (leftHandWeapon.isAShield)
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
                    if (isHoldingShield&&leftHandWeapon.isAShield)
                    {
                        animator.SetLayerWeight(3, 1);
                    }
                    else
                    {
                        animator.SetLayerWeight(3, 0);
                    }
                }
                #endregion               
            }
            //else
            //{
            //    animator.SetLayerWeight(3, 0);
            //}
            if (attackAnimPlaying)
            {
                isHoldingShield = false;
            }
        }
    }
    
    #endregion
    #region attacks
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
    #endregion
    #region WeaponSelection and pickup
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
    public bool GetIsHoldingShield()
    {
        return isHoldingShield;
    }
    #endregion
    #region consume and critical attacks
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
    void AttemptCritical()
    {
        RaycastHit hit;
        if(Physics.Raycast(criticalAttackRayPoint.position,transform.TransformDirection(Vector3.forward),out hit,0.6f))
        {
            if (hit.collider.GetComponent<Enemy>())
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                Vector3 view = hit.collider.transform.forward;
                view.Normalize(); 
                Vector3 br = transform.position - hit.collider.transform.position;
                br.Normalize();
                if (DotProduct(view, br) < -0.5)
                {
                    Debug.Log("BackStab");
                    transform.position = enemy.backStabberTransform.position;
                    Vector3 rotationDir = transform.eulerAngles;
                    rotationDir = hit.transform.position - transform.position;
                    rotationDir.y = 0;
                    rotationDir.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDir);
                    Quaternion targetRot = Quaternion.Slerp(transform.rotation, tr, 500 * Time.deltaTime);
                    transform.rotation = targetRot;
                    animationPlayer.PlayerTargetAnim("BackStab", true);
                    enemy.EnemyTargetAnim("BackStabbed", true);
                    //enemy.EnemyInterruptingAnim("BackStabbed"); 
                    lastAttack = "BackStab";
                    criticalDmg = enemy.GetComponent<EnemyHealth>().GetEnemyHealth() / 2;
                    enemy.GetComponent<EnemyHealth>().TakeDamage(criticalDmg);
                    //enemy.SetSight(true);
                }
            }
        }    
    }
    private float DotProduct(Vector3 a, Vector3 b )
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }
    #endregion
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
