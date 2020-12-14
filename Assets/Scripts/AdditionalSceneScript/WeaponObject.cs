using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/WeaponObject")]
public class WeaponObject : ScriptableObject
{
    [Header("Icon")]
    public Sprite icon;
    [Header("Anims")]
    public AnimatorOverrideController weaponOvveride;
    public AnimatorOverrideController twoHandWeaponOvveride;
    [Header("Stamina drain")]
    public float baseStaminaDrain;
    public float lightAttackMultiplier;
    public float heavyAttackMultiplier;
    [Header("Damage")]
    public float baseDamage;
    [Header("Rotations and positions")]
    public Vector3 oneHandedEquipmentPosition;//local
    public Vector3 twoHandedEquipmentPosition;//local
    [Space(4)]
    public Vector3 oneHandedEquipmentRotation;//local
    public Vector3 twoHandedEquipmentRotation;//local 
    [Header("Prefab model")]
    public GameObject model;
    [Header("enemy impact anims")]
    public string lightAttackImpactAnimName;
    public string lightAttack2ImpactAnimName;
    public string heavyAttackImpactAnimName;
    public string heavyAttack2ImpactAnimName;
    GameObject instance;
    public float DamageCalculator(int level)
    {
        return level + baseDamage; 
    }
    public void EquipOn(bool twoHanded,Transform parent)
    {        
        if (!twoHanded)
        {
            instance = Instantiate(model, parent);
            HandTransformRegulator(instance, false);
        }
        else
        {
            instance = Instantiate(model, parent);
            HandTransformRegulator(instance, true);
        }
    }
    public void HandTransformRegulator(GameObject gameObject,bool isTwoHanded)
    {
        if (!isTwoHanded)
        {
            gameObject.transform.localPosition = oneHandedEquipmentPosition;
            gameObject.transform.localEulerAngles = oneHandedEquipmentRotation;
        }
        else
        {
            gameObject.transform.localPosition = twoHandedEquipmentPosition;
            gameObject.transform.localEulerAngles = twoHandedEquipmentRotation;
        }
    }
    public GameObject GetInstance()
    {
        return instance;
    }
}
