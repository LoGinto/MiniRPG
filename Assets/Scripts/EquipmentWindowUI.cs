using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulItem;
namespace RPG.UI
{
    public class EquipmentWindowUI : MonoBehaviour
    {
        public bool righthandSlot01Selected;
        public bool righthandSlot02Selected;
        public bool lefthandSlot01Selected;
        public bool lefthandSlot02Selected;

        public void SelectRightHandSlot01()
        {
            righthandSlot01Selected = true;
        }
        public void SelectRightHandSlot02()
        {
            righthandSlot02Selected = true;
        }
        public void SelectLeftHandSlot01()
        {
            lefthandSlot01Selected = true;
        }
        public void SelectLeftHandSlot02()
        {
            lefthandSlot02Selected = true;
        }

        public HandEquipmentSlotUI[] handEquipmentSlotUI;

        
        public void LoadWeaponsOnEquipmentScreen(PlayerInventory playerInventory)
        {
            for (int i = 0; i < handEquipmentSlotUI.Length; i++)
            {
                if (handEquipmentSlotUI[i].rightHandSlot01)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[0]);

                }
                else if (handEquipmentSlotUI[i].rightHandSlot02)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[1]);
                }
                else if (handEquipmentSlotUI[i].leftHandSlot01)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[0]);
                }
                else
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[1]);
                }
            }
        }
    }
}