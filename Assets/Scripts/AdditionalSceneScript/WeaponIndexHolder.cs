using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIndexHolder : MonoBehaviour
{
    public int weaponToClickIndex;
    public int clothToClickIndex;
    [HideInInspector] BetterFighter betterFighter;
    [HideInInspector] BetterInventory betterInventory;
    private void OnEnable()
    {
        betterFighter = FindObjectOfType<BetterFighter>();
        betterInventory = FindObjectOfType<BetterInventory>();
    }
    public void EquipTheWeaponAtIndex()
    {
        if (weaponToClickIndex <= betterFighter.weaponsInBackPack.Count)
        {
            betterFighter.EquipWeaponFromInventory(weaponToClickIndex);
        }
    }
    public void EquipCloth()
    {
        if(clothToClickIndex <= betterInventory.clothes.Count)
        {
            betterInventory.EquipClothFromInventory(clothToClickIndex);
        }
    }
}
