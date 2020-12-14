using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageColliders : MonoBehaviour
{
    private Collider weaponCollider = null;
    private void Awake()
    {
        AssignWeaponCollider();
    }
    public virtual void AssignWeaponCollider()
    {
        if (weaponCollider == null)
        {
            if (gameObject.GetComponent<BetterFighter>().weaponObject != null)
            {
                //get instance's collider
                weaponCollider = gameObject.GetComponent<BetterFighter>().weaponObject.GetInstance().GetComponent<Collider>();
            }
        }
    }
    private void Update()
    {
        AssignWeaponCollider();
    }
    public virtual void TurnOnColliderAnim()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
            //Debug.Log("Weapon enabled");
        }
    }
    public virtual void TurnOffColliderAnim()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
            //Debug.Log("Weapon disabled");
        }
    }
    public Collider GetWeaponCollider()
    {
        return weaponCollider;
    }
}
