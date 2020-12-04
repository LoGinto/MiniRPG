using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterPickup : MonoBehaviour
{
    [SerializeField] WeaponObject weaponObject;

    public WeaponObject Pickup()
    {
        return weaponObject;
    }
}
