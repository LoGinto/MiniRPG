using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyImpacter : MonoBehaviour
{
    public WeaponObject enemyWeapon;
    public float defaultWeaponDamageforError = 35f;
    [SerializeField] EnemyStat enemyStat = null;
    float dealingDamage;
    // Start is called before the first frame update
    void Start()
    {
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
        if (other.tag == "Player")
        {
            Debug.Log("Knight gives " + dealingDamage + "to " + other.name);
            other.GetComponent<HealthManager>().TakeDamage(dealingDamage);
        }
    }
}
