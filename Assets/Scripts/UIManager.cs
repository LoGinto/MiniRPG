using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulItem;

namespace RPG.UI { 
public class UIManager : MonoBehaviour
{
        [Header("Windows")]
        public GameObject selectWindow;
        public GameObject hudUI;
        public GameObject weaponSlotPrefab;
        public GameObject weaponInventoryWindow;
        public GameObject equipmentScreenWindow;
        [Space(2)]
        bool inventoryOpen = false;
        public Transform weaponSlotParent;
        WeaponInventorySlot[] weaponSlots;
        public PlayerInventory inventoryScript;
        EquipmentWindowUI equipmentWindowUI;
        [Header("Selected slots")]
        public bool rightHandSlot01Selected;
        public bool rightHandSlot02Selected;
        public bool leftHandSlot01Selected;
        public bool leftHandSlot02Selected;
        private void Awake()
        {
            equipmentWindowUI = FindObjectOfType<EquipmentWindowUI>();           
        }
        private void Start()
        {
            weaponSlots = weaponSlotParent.GetComponentsInChildren<WeaponInventorySlot>();
            equipmentWindowUI.LoadWeaponsOnEquipmentScreen(inventoryScript);
        }
        private void Update()
        {
            HandleInventoryInput();
        }
        void CloseAllInventoryWindows()
        {
            ResetAllSelectedSlots();
            weaponInventoryWindow.SetActive(false);
            equipmentScreenWindow.SetActive(false);
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
        public void ResetAllSelectedSlots()
        {
            rightHandSlot02Selected = false;
            rightHandSlot01Selected = false;
            leftHandSlot01Selected = false;
            leftHandSlot02Selected = false;
         }
        public EquipmentWindowUI GetEquipmentWindowUI()
        {
            return equipmentWindowUI;
        }
}
}