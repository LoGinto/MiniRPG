using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulItem {
    public class WeaponPickup : Interactible
    {
        public WeaponItem weapon;
        GameObject player;
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        public override void Interact()
        {
            PickUpItem();
        }
        void PickUpItem()
        {
            PlayerInventory inventory;
            inventory = player.GetComponent<PlayerInventory>();
            inventory.weaponsInventory.Add(weapon);
            Destroy(gameObject);
        }
    }
}
