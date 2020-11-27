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
        [SerializeField] float spherecastDist = 0.5f;
        [SerializeField] GameObject interactibleUIGameObject;
        public GameObject itemInteractibleUIGameObject;
        InteractibleUI interactibleui; 
        Interactible interactible = null;
        private void Awake()
        {
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }
        private void Start()
        {
            rightWeapon = weaponsInRightHandSlots[0];
            leftWeapon = weaponsInLeftHandSlots[0];
            interactibleui = FindObjectOfType<InteractibleUI>();
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
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
            CheckForInteractible();
        }
        public void CheckForInteractible()
        {
            RaycastHit hit;
            if (Physics.SphereCast(transform.position,radius,transform.forward,out hit,spherecastDist))
            {
                if(hit.collider.tag == "Interactible")
                {
                    interactible = hit.collider.GetComponent<Interactible>();
                    if(interactible != null)
                    {
                        string interactableText = interactible.interactibleText;
                        interactibleui.interactibleText.text = interactableText;
                        interactibleUIGameObject.SetActive(true);
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            hit.collider.GetComponent<Interactible>().Interact();
                        }
                    }
                }
                else
                {
                    if(interactibleUIGameObject != null)
                    {
                        interactibleUIGameObject.SetActive(false);
                    }
                    if(itemInteractibleUIGameObject != null && Input.GetKeyDown(KeyCode.E))
                    {
                        itemInteractibleUIGameObject.SetActive(false);
                    }
                }
            }
            //else
            //{
            //    interactibleUIGameObject.SetActive(false);
            //    itemInteractibleUIGameObject.SetActive(false);
            //}
        }
    }

    
}
