using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterFighter : MonoBehaviour
{
    public WeaponObject weaponObject;
    Animator animator;
    PlayerStats playerStats;
    public Transform equipmentParent;
    int index = 0;
    public List<WeaponObject> weaponsInBackPack = new List<WeaponObject>();
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        //handle first weapon as well
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CycleWeapon(bool forward)
    {
        //handle bounds, to do
        if (forward)
        {
            weaponObject = weaponsInBackPack[index + 1];
            animator.runtimeAnimatorController = weaponObject.weaponOvveride;
            weaponObject.EquipOn(false,equipmentParent);
        }
        else // also I have to destroy previous instance, to do
        {
            weaponObject = weaponsInBackPack[index - 1];
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
                weaponObject.HandTransformRegulator(weaponObject.GetInstance(), true);
                }
        }
        else
        {
            animator.runtimeAnimatorController = weaponObject.weaponOvveride;
            if (weaponObject.GetInstance() != null)
            {
                weaponObject.HandTransformRegulator(weaponObject.GetInstance(), false);
            }
        }
    }
}
