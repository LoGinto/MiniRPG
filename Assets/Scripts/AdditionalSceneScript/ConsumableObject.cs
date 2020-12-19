using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Consumable")]
public class ConsumableObject : ScriptableObject
{
    #region Encapsulation
    public Sprite icon;
    public bool isThrowable;
    public bool isAWeaponBoost;
    public float increase;
    public GameObject model;
    public GameObject particle;
    public enum IncreaseType { stamina,health}
    public IncreaseType increaseType = IncreaseType.health; 
    public string consumingAnimationName;
    [Header("Consumable spawn point")]
    public Vector3 consumableSpawnPos;
    public Vector3 consumableSpawnRot;
    [Header("Consumable Effect spawn point")]
    public Vector3 consumableEffectSpawnPos;
    public Vector3 consumableEffectSpawnRot;
    GameObject instance;
    GameObject particleInstance;
    #endregion
    public void Consume(float variableToIncrease)
    {
        if (!isThrowable && !isAWeaponBoost)
        {
            variableToIncrease += increase;
        }
    }
    public void Consume(int variableToIncrease)
    {
        if (!isThrowable&&!isAWeaponBoost)
        {
            int myVar = (int)increase;
            variableToIncrease += myVar;      
        }
    }
    public void PlayConsumptionEffect(Transform parent)
    {
        if (particle != null) {
            particleInstance = Instantiate(particle, parent);
            particleInstance.transform.localPosition = consumableEffectSpawnPos;       
            particleInstance.transform.localEulerAngles = consumableEffectSpawnRot;
            if (particleInstance.GetComponent<ParticleSystem>())
            {
                particleInstance.GetComponent<ParticleSystem>().Play();
            }
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
    public GameObject GetParticleInstance()
    {
        return particleInstance;
    }
}
