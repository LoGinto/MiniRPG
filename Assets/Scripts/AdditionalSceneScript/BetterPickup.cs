using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterPickup : MonoBehaviour
{
    [SerializeField] WeaponObject weaponObject;
    bool isPickedUp = false;
    public WeaponObject Pickup()
    {
        isPickedUp = true;
        return weaponObject;
    }
    public bool IsPickedUp()
    {
        return isPickedUp;
    }
}
