using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SoulItem;
namespace RPG.UI
{
    public class QuickSlotUI : MonoBehaviour
    {
        public Image leftWeaponIcon;
        public Image rightWeaponIcon;

        public void UpdateWeaponQuickSlotsUI(bool isLeft,WeaponItem weaponItem)
        {
           
            if (!isLeft)
            {
                AssignSprite(weaponItem,rightWeaponIcon);

            }
            else
            {
                AssignSprite(weaponItem, leftWeaponIcon);                
            }
        }

        private void AssignSprite(WeaponItem weaponItem,Image icon)
        {
            if (weaponItem.itemIcon != null)
            {
                icon.sprite = weaponItem.itemIcon;
                icon.enabled = true;
            }
            else
            {
                icon.sprite = null;
                icon.enabled = false;
            }
        }
    }
}