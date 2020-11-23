using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStats : MonoBehaviour
{
    public float maxStamina;
    public float currentStamina;
    public int staminaLevel = 10;
    public float rollDrain = 20f;
    float staminaRecoveryValue;
    public Slider staminaSlider;
    private void Start()
    {
        staminaRecoveryValue = SetStaminaRecoveryValue();
        maxStamina = SetMax();
        currentStamina = maxStamina;
    }
    float SetMax()
    {
        maxStamina = staminaLevel *10;
        return maxStamina;
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
        staminaSlider.value = currentStamina/maxStamina;        
    }
    private void FixedUpdate()
    {
        if (!gameObject.GetComponent<Animator>().GetBool("IsInteracting")&&currentStamina<maxStamina)
            currentStamina += staminaRecoveryValue * Time.fixedDeltaTime;
    }
}
