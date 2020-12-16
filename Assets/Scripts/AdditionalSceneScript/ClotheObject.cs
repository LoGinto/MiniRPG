using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Cloth")]
public class ClotheObject : ScriptableObject
{
    public GameObject model;
    public Sprite icon;
    public Vector3 relativePosition;
    public Vector3 relativeRotation;
    public enum BodyPart
    {
        head,torso,legs,arms,empty
    }
    public BodyPart bodyPart = BodyPart.head;
    GameObject clothInstance;
    //also I can add methods which decrease speed or do smth with stats in future
    //to do: unequip from same part of body and equip another
    public void EquipCloth(Transform parent)
    {
        clothInstance = Instantiate(model, parent);
        clothInstance.transform.localPosition = relativePosition;
        clothInstance.transform.localEulerAngles = relativeRotation;
    }
    public GameObject GetClothInstance()
    {
        return clothInstance;
    }
}
