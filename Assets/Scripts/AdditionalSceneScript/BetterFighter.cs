using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterFighter : MonoBehaviour
{
    public WeaponObject weaponObject;
    Animator animator;
    PlayerStats playerStats;
    public Transform equipmentParent;
    private int index = -1;
    private bool twoHand = false;
    [SerializeField] KeyCode forwardKey = KeyCode.O;
    [SerializeField] KeyCode backKey = KeyCode.L;
    [SerializeField] KeyCode pickupKey = KeyCode.E;
    public List<WeaponObject> weaponsInBackPack = new List<WeaponObject>();
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        //handle first weapon as well
        if(index == -1)
        {
            CycleWeapon(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
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
            twoHand = false;
            weaponsInBackPack.Add(other.GetComponent<BetterPickup>().Pickup());
            Destroy(other.gameObject);
        }
    }
}
