using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SoulItem;
namespace RPG.UI
{
    public class HandEquipmentSlotUI : MonoBehaviour
    {
        UIManager uiManager;
        public Image icon;
        WeaponItem weaponItem;
        public bool rightHandSlot01;
        public bool rightHandSlot02;
        public bool leftHandSlot01;
        public bool leftHandSlot02;

        private void Awake()
        {
            uiManager = FindObjectOfType<UIManager>();
        }
        public void AddItem(WeaponItem newWeapon)
        {
            weaponItem = newWeapon;
            icon.sprite = weaponItem.itemIcon;
            icon.enabled = true;
            icon.gameObject.SetActive(true);
            gameObject.SetActive(true); 
        }
        public void ClearItem(WeaponItem newWeapon)
        {
            weaponItem = null;
            icon.sprite = null;
            icon.enabled = false;
            icon.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
        public void SelectThisSlot()
        {
            if (rightHandSlot01)
            {
                uiManager.rightHandSlot01Selected = true;
            }
            else if(rightHandSlot02)
            {
                uiManager.rightHandSlot02Selected = true;
            }
            else if (leftHandSlot01)
            {
                uiManager.leftHandSlot01Selected = true;
            }
            else
            {
                uiManager.leftHandSlot02Selected = true;
            }
        }
    }
}