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
        WeaponHolderSlot backSlot;
        
        Animator animator;
        Fighter fighter;
        QuickSlotUI quickSlotUI;
        WeaponItem attackingWeapon;        
        private void Awake()
        {
            animator = GetComponent<Animator>();
            quickSlotUI = FindObjectOfType<QuickSlotUI>();
            fighter = FindObjectOfType<Fighter>();
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
                else if (weaponSlot.isBackSlot)
                {
                    backSlot = weaponSlot;
                }
            }
        }
        public void LoadWeaponOnSlot(WeaponItem weaponItem,bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeapon = weaponItem;
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
                //animator.CrossFade("BothArmsEmpty",0.2f);
                if (fighter.GetHandlingWithTwoHandValue() == true)
                {

                    backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                    leftHandSlot.UnloadAndDestroy();
                    animator.SetBool("TH", true);
                }
                else
                {
                    animator.SetBool("TH", false);
                    backSlot.UnloadAndDestroy();
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
            rightHandSlot.currentWeapon = weaponItem;
            rightHandSlot.LoadWeaponModel(weaponItem);
            quickSlotUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
        }
        public void SetAttackingWeaponItem(WeaponItem newweaponItem)
        {
            attackingWeapon = newweaponItem;
        }
       
    }
}