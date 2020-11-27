using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulItem;
using UnityEngine.UI;
namespace RPG.UI
{
    public class WeaponInventorySlot : MonoBehaviour
    {
        public Image icon;
        WeaponItem item;
        PlayerInventory inventory;
        WeaponSlotManager slotManager;
        UIManager uiManager;
        private void Awake()
        {
            inventory = FindObjectOfType<PlayerInventory>();
            uiManager = FindObjectOfType<UIManager>();
            slotManager = FindObjectOfType<WeaponSlotManager>();
        }
        public void AddItem(WeaponItem newItem)
        {
            item = newItem;
            icon.sprite = item.itemIcon;
            icon.gameObject.SetActive(true);
            gameObject.SetActive(true);
        }
        public void ClearInventorySlot()
        {
            item = null;
            icon.sprite = null;
            icon.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
        public void EquipThisItem()
        {
            if (uiManager.rightHandSlot01Selected)
            {
                inventory.weaponsInventory.Add(inventory.weaponsInRightHandSlots[0]);
                inventory.weaponsInRightHandSlots[0] = item;
                inventory.weaponsInventory.Remove(item);
                inventory.rightWeapon = inventory.weaponsInRightHandSlots[inventory.currenRightWeaponIndex];
            }
            else if (uiManager.rightHandSlot02Selected)
            {
                inventory.weaponsInventory.Add(inventory.weaponsInRightHandSlots[1]);
                inventory.weaponsInRightHandSlots[1] = item;
                inventory.weaponsInventory.Remove(item);
                //inventory.rightWeapon = inventory.weaponsInRightHandSlots[inventory.currenRightWeaponIndex];
            }
            else if (uiManager.leftHandSlot01Selected)
            {
                inventory.weaponsInventory.Add(inventory.weaponsInLeftHandSlots[0]);
                inventory.weaponsInLeftHandSlots[0] = item;
                inventory.weaponsInventory.Remove(item);
                //inventory.rightWeapon = inventory.weaponsInLeftHandSlots[inventory.currenLeftWeaponIndex];
            }
            else if(uiManager.leftHandSlot02Selected)
            {
                inventory.weaponsInventory.Add(inventory.weaponsInLeftHandSlots[1]);
                inventory.weaponsInLeftHandSlots[1] = item;
                inventory.weaponsInventory.Remove(item);        
            }
            else
            {
                return;
            }
            if (inventory.currenRightWeaponIndex != -1)
            {
                inventory.rightWeapon = inventory.weaponsInRightHandSlots[inventory.currenRightWeaponIndex];
            }
            else
            {
                inventory.currenRightWeaponIndex = 0;
            }
            if (inventory.currenLeftWeaponIndex != -1)
            {
                inventory.leftWeapon = inventory.weaponsInLeftHandSlots[inventory.currenLeftWeaponIndex];
            }
            else
            {
                inventory.currenLeftWeaponIndex = 0;
            }
            slotManager.LoadWeaponOnSlot(inventory.rightWeapon, false);
            slotManager.LoadWeaponOnSlot(inventory.leftWeapon, true );
            uiManager.GetEquipmentWindowUI().LoadWeaponsOnEquipmentScreen(inventory);
            uiManager.ResetAllSelectedSlots();
        }
    }
}