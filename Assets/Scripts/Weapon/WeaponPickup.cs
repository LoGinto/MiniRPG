using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace SoulItem {
    public class WeaponPickup : Interactible
    {
        public WeaponItem weapon;
        GameObject player;
        PlayerInventory inventory;
        InteractibleUI uiParent;
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            inventory = player.GetComponent<PlayerInventory>();
            uiParent = FindObjectOfType<InteractibleUI>();
        }
        public override void Interact()
        {
            PickUpItem();
        }
        void PickUpItem()
        {
                      
            inventory.weaponsInventory.Add(weapon);
            inventory.itemInteractibleUIGameObject.GetComponentInChildren<Text>().text = weapon.itemName;
            uiParent.SetItemImage(weapon.itemIcon);
            inventory.itemInteractibleUIGameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
