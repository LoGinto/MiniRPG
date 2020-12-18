using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIndexHolder : MonoBehaviour
{
    public int weaponToClickIndex;
    public int clothToClickIndex;
    public int consumableToClickIndex;
    [HideInInspector] BetterFighter betterFighter;
    [HideInInspector] BetterInventory betterInventory;
    private void OnEnable()
    {
        betterFighter = FindObjectOfType<BetterFighter>();
        betterInventory = FindObjectOfType<BetterInventory>();
    }
    #region EquipmentRegion
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
    public void EquipConsumable()
    {
        if (consumableToClickIndex <= betterInventory.consumables.Count)
        {
            betterInventory.EquipConsumable(consumableToClickIndex);
        }
    }
    #endregion
}
