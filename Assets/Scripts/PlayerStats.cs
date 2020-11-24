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
    public Slider staminaSlider;
    private float staminaRecoveryValue;
    private void Start()
    {
        staminaRecoveryValue = SetStaminaRecoveryValue();
        maxStamina = SetMax();
        currentStamina = maxStamina;
        
    }
    float SetMax()
    {
        maxStamina = staminaLevel * 10;
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
        staminaSlider.value = currentStamina / maxStamina;       
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

