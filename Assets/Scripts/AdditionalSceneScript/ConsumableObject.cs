using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Consumable")]
public class ConsumableObject : ScriptableObject
{
    public Sprite icon;
    public bool isThrowable;
    public bool isBoostable;
    public float increase;
    public GameObject model;
    public Vector3 consumableSpawnPos;
    public Vector3 consumableSpawnRot;
    public string consumingAnimationName;
    GameObject instance;
    public void Consume(float variableToIncrease)
    {
        if (!isThrowable && isBoostable)
        {
            variableToIncrease += increase;
        }
    }
    public void Consume(int variableToIncrease)
    {
        if (!isThrowable&&isBoostable)
        {
            int myVar = (int)increase;
            variableToIncrease += myVar;
        }
    }
    public void SpawnConsumableAt(Transform parent)
    {
        instance = Instantiate(model, parent);
        instance.transform.localPosition = consumableSpawnPos;
        instance.transform.localEulerAngles = consumableSpawnRot; 
    }
    public GameObject GetConsumableInstance()
    {
        return instance;
    }
}
