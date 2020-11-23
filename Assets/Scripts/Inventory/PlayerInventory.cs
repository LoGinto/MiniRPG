using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulItem
{
    public class PlayerInventory : MonoBehaviour
    {
        WeaponSlotManager weaponSlotManager;
        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;
        public WeaponItem unarmedWeapon;
        public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
        public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[1];
        bool interactibleKey;
        [HideInInspector]public int currenRightWeaponIndex = 0;
        [HideInInspector]public int currenLeftWeaponIndex = 0;
        public List<WeaponItem> weaponsInventory;
        [SerializeField]float radius;
        private void Awake()
        {
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }
        private void Start()
        {
            rightWeapon = unarmedWeapon;
            leftWeapon = unarmedWeapon;
        }
        public void ChangeWeapon(int index,WeaponItem[] weapons,WeaponItem weapon,bool isLeft)
        {
            try
            {
                index = index + 1;
                if (weapons[index] != null)
                {
                    weapon = weapons[index];
                    //weaponSlotManager.LoadWeaponOnSlot(weapon, isLeft);
                }
                else
                {
                    index = index - 1;
                    weapon = weapons[index];
                }
                if (index > weapons.Length)
                {
                    index = 0;
                    weapon = weapons[index];
                   // weaponSlotManager.LoadWeaponOnSlot(weapon, isLeft);
                }
                                
                weaponSlotManager.LoadWeaponOnSlot(weapon, isLeft);
                if (isLeft) leftWeapon = weapon;
                else rightWeapon = weapon; //explicitly
            }
            catch
            {
                Debug.Log("catch");
                
            }
            Debug.Log("I change" + index + " Weapon to" + weapon.name);
        }       
        void HandleQuickSlot()
        {
            if (Input.GetKeyDown(KeyCode.CapsLock))
            {
                
                ChangeWeapon(currenRightWeaponIndex,weaponsInRightHandSlots,rightWeapon,false);
            }
            else if(Input.GetKeyDown(KeyCode.Tab))
            {
                ChangeWeapon(currenLeftWeaponIndex, weaponsInLeftHandSlots, leftWeapon, true);
            }
        }
        private void Update()
        {
            HandleQuickSlot();
            interactibleKey = Input.GetKeyDown(KeyCode.E);
            if (interactibleKey)
            {
                CheckForInteractible();
            }
        }
        public void CheckForInteractible()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position,radius);
            foreach (var hitCollider in colliders)
            {
                if(hitCollider.tag == "Interactible")
                {
                    if (hitCollider.GetComponent<WeaponPickup>())
                    {
                        hitCollider.GetComponent<WeaponPickup>().Interact();
                    }
                }
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
    
}
