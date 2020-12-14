using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Impacter : MonoBehaviour
{
    //base damage dealing class
    private float dealingDamage;
    public WeaponObject weaponObject;
    GameObject player;
    private void Start()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        player = GameObject.FindGameObjectWithTag("Player");
        CalculateDamage();
    }
    private void Update()
    {
        if (CanDamage())
        {
            CalculateDamage();
        }
    }
    public virtual void CalculateDamage()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerStats stats = player.GetComponent<PlayerStats>();
        int level = stats.level;
        dealingDamage = weaponObject.DamageCalculator(level);
    }
    public virtual bool CanDamage()
    {
        return player.GetComponent<DamageColliders>().GetWeaponCollider().enabled;
    }
    private void OnCollisionEnter(Collision collision)
    {
        //base for player
        Debug.Log("Collision with " + collision.collider.name);
        if (collision.collider.tag == "Enemy")
        {
            collision.collider.GetComponent<EnemyHealth>().TakeDamage(dealingDamage);
        }
    }
}
