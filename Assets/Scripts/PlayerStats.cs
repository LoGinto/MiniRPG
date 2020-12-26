using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStats : MonoBehaviour,ISaveable
{
    public float maxStamina;
    public int level = 10;
    public int soulsCount = 0;
    public float currentStamina;
    public int staminaLevel = 10;
    public float health = 0;
    float maxHealth;
    public float rollDrain = 20f;
    public Slider staminaSlider;
    public Slider healthSlider;
    private float staminaRecoveryValue;
    private void Start()
    {
        staminaRecoveryValue = SetStaminaRecoveryValue();
        maxStamina = SetMaxStamina();
        currentStamina = maxStamina;
        maxHealth = SetMaxHealth();
        health = maxHealth;
    }
    float SetMaxStamina()
    {
        maxStamina = staminaLevel * 10;
        return maxStamina;
    }
    float SetMaxHealth()
    {
        maxHealth = level * 10;
        return maxHealth;
    }
    float SetStaminaRecoveryValue()
    {
        staminaRecoveryValue = staminaLevel / 2;
        return staminaRecoveryValue;
    }
    public void TakeStamina(float damage)
    {
        currentStamina = currentStamina - damage;
    }
    public void RollDrain()
    {
        currentStamina -= rollDrain;
    }
    private void Update()
    {
        staminaSlider.value = currentStamina / maxStamina;
        healthSlider.value = health / maxHealth;
    }
    private void FixedUpdate()
    {
        if (currentStamina < maxStamina)
        {
            StartCoroutine(WaitoRefill());
        }
    }
    IEnumerator WaitoRefill()
    {
        yield return new WaitForSeconds(3.5f);
        while (currentStamina < maxStamina)
        {
            if (!gameObject.GetComponent<Animator>().GetBool("IsInteracting"))
            {
                currentStamina += staminaRecoveryValue * Time.deltaTime;
                if (currentStamina >= maxStamina)
                {
                    break;
                }
                //yield return new WaitForEndOfFrame();               
            }
            yield return null;
        }
    }
    public void AddHealth(float healthToAdd)
    {
        health += healthToAdd;
    }
    public void AddStamina(float staminaToAdd)
    {
        currentStamina += staminaToAdd;
    }

    public object CaptureState()
    {
        return new SaveData
        {
            soulsCount = soulsCount,
            staminaLevel = staminaLevel,
            level = level
        };
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;
        level = saveData.level;
        staminaLevel = saveData.staminaLevel;
        soulsCount = saveData.soulsCount; 
    }
    [Serializable]
    private struct SaveData
    {
        public int soulsCount;
        public int staminaLevel;
        public int level;
    }
}

