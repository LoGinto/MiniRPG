using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageColliders : MonoBehaviour
{
    Collider weaponCollider = null;
    private void Awake()
    {
        AssignWeaponCollider();
    }
    private void AssignWeaponCollider()
    {
        if (weaponCollider == null)
        {
            if (gameObject.GetComponent<BetterFighter>().weaponObject != null)
            {
                weaponCollider = gameObject.GetComponent<BetterFighter>().weaponObject.GetInstance().GetComponent<Collider>();
            }
        }
    }
    private void Update()
    {
        AssignWeaponCollider();
    }
    public void TurnOnColliderAnim()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
            //Debug.Log("Weapon enabled");
        }
    }
    public void TurnOffColliderAnim()
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
