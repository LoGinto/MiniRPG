using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.UI;
namespace SoulItem
{
    public class WeaponSlotManager : MonoBehaviour
    {
        WeaponHolderSlot leftHandSlot;
        WeaponHolderSlot rightHandSlot;
        Animator animator;
        QuickSlotUI quickSlotUI;
        WeaponItem attackingWeapon;        
        private void Awake()
        {
            animator = GetComponent<Animator>();
            quickSlotUI = FindObjectOfType<QuickSlotUI>();
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach(WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.isLeftSlot)
                {
                    leftHandSlot = weaponSlot;
                }
                else if(weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
            }
        }
        public void LoadWeaponOnSlot(WeaponItem weaponItem,bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.LoadWeaponModel(weaponItem);
                quickSlotUI.UpdateWeaponQuickSlotsUI(true,weaponItem);
                if(weaponItem != null)
                {
                    animator.CrossFade(weaponItem.left_hand_idle, 0.2f);
                }
                else
                {
                    animator.CrossFade("Left Arm Empty", 0.2f);
                }
            }
            else
            {
                rightHandSlot.LoadWeaponModel(weaponItem);
                quickSlotUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                if (weaponItem != null)
                {
                    animator.CrossFade(weaponItem.right_hand_idle, 0.2f);
                }
                else
                {
                    animator.CrossFade("Right Arm Empty", 0.2f);
                }
            }
        }
       
    }
}