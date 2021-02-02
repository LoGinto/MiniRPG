using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyImpacter : MonoBehaviour
{
    public WeaponObject enemyWeapon;
    public float defaultWeaponDamageforError = 35f;
    public string interuptionAnimName = "attack_interrupt";
    [SerializeField] EnemyStat enemyStat = null;
    float dealingDamage;
    bool blocked = false;
    public float staminaDepletionPerHit = 20f;
    GameObject player;
    Shield shield = null;
    bool shieldUP;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }
    void OnEnable()
    {
        gameObject.GetComponent<Collider>().isTrigger = true;
    }
    // Update is called once per frame
    void Update()
    {        
        try
        {
            if (CanDamage())
            {
                CalculateDamage();
            }
        }
        catch
        {
            CalculateDamage();
        }
        if(shield != null)
        {
            shieldUP = shield.shieldUp;
        }
    }
    void CalculateDamage()
    {
        //f this method
        try
        {
            dealingDamage = enemyWeapon.DamageCalculator(transform.root.GetComponentInParent<Enemy>().GetEnemyStat().level);
        }
        catch
        {
            try
            {
                dealingDamage = enemyWeapon.DamageCalculator(transform.root.GetComponentInParent<EnemyHealth>().GetEnemyStat().level);
            }
            catch
            {
                try
                {
                    if (enemyStat != null)
                    {
                        try
                        {
                            dealingDamage = enemyStat.level + enemyWeapon.baseDamage;
                        }
                        catch
                        {
                            dealingDamage = enemyWeapon.DamageCalculator(enemyStat.level);
                        }
                    }
                }
                catch
                {
                    dealingDamage = defaultWeaponDamageforError;
                }
            }
        }
    }
    bool CanDamage()
    {
        return GetComponentInParent<EnemyDamageCollider>().GetCollider().enabled;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Shield>())
        {            
            shield = other.GetComponent<Shield>();
            //interupt attack
            if (shieldUP == true)
            {
                if (transform.root.GetComponent<Enemy>().GetIsHavingAttackAnim())
                {
                    Debug.Log($"{transform.root} attack should interrupt");
                    transform.root.GetComponent<Enemy>().EnemyInterruptingAnim(interuptionAnimName);
                    transform.root.GetComponent<Animator>().CrossFade(interuptionAnimName, 0.1f);
                }
            }
            else if (shieldUP == false)
            {
                player.GetComponent<HealthManager>().TakeDamage(dealingDamage);
            }
        }
        else 
        {
            if (shield == null|| shieldUP == false)
            {
                player.GetComponent<HealthManager>().TakeDamage(dealingDamage);
            }
        }
    }
   
}
