using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStats : MonoBehaviour
{
    public float maxStamina;
    public int level = 10;
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
}

