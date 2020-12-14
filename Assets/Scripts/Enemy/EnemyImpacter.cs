using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyImpacter : MonoBehaviour
{
    public WeaponObject enemyWeapon;
    float dealingDamage;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
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
        dealingDamage = enemyWeapon.DamageCalculator(gameObject.GetComponentInParent<Enemy>().GetEnemyStat().level); 
    }
    bool CanDamage()
    {
        return GetComponentInParent<EnemyDamageCollider>().GetCollider().enabled;
    }
    private void OnCollisionEnter(Collision collision)
    {
        //do damage
        Debug.Log("Knight delivers " + dealingDamage + " to " + collision.collider.name); 
    }
}
