using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulItem;

namespace RPG.UI { 
public class UIManager : MonoBehaviour
{
        public GameObject selectWindow;
        public GameObject hudUI;
        public GameObject weaponSlotPrefab;
        public GameObject weaponInventoryWindow;
        bool inventoryOpen = false;
        public Transform weaponSlotParent;
        WeaponInventorySlot[] weaponSlots;
        public PlayerInventory inventoryScript;

        private void Start()
        {
            weaponSlots = weaponSlotParent.GetComponentsInChildren<WeaponInventorySlot>();
        }
        private void Update()
        {
            HandleInventoryInput();
        }
        void CloseAllInventoryWindows()
        {
            weaponInventoryWindow.SetActive(false);
        }
        public void UpdateUI()
        {
            for(int i = 0; i < weaponSlots.Length; i++)
            {
                if (i < inventoryScript.weaponsInventory.Count)
                {
                    if(weaponSlots.Length < inventoryScript.weaponsInventory.Count)
                    {
                        Instantiate(weaponSlotPrefab,weaponSlotParent);
                        weaponSlots = weaponSlotParent.GetComponentsInChildren<WeaponInventorySlot>();
                    }
                    weaponSlots[i].AddItem(inventoryScript.weaponsInventory[i]);
                }
                else
                {
                    weaponSlots[i].ClearInventorySlot();
                }
            }
        }
        void HandleInventoryInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                inventoryOpen = !inventoryOpen;
            }
            if (inventoryOpen == true)
            {
                UpdateUI();
                hudUI.SetActive(false);
            }
            else
            {
                CloseAllInventoryWindows();
                hudUI.SetActive(true);
            }
            selectWindow.SetActive(inventoryOpen);
        }
}
}