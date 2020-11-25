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
    }
}