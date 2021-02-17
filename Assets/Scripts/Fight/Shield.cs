using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public bool shieldUp;
    public bool isForPlayer;
    BetterFighter betterFighter = null;
    BoxCollider shieldCollider;
    private void Start()
    {
        if (isForPlayer)
        {
            betterFighter = GameObject.FindGameObjectWithTag("Player").GetComponent<BetterFighter>();            
        }
        shieldCollider = gameObject.GetComponent<BoxCollider>();
    }
    private void Update()
    {
        if (isForPlayer)
        {
            shieldUp = betterFighter.GetIsHoldingShield(); 
            if (betterFighter.GetComponent<Animator>().GetCurrentAnimatorStateInfo(2).IsName("ParryShield"))
            {
                shieldUp = false;
            }
        }
        shieldCollider.enabled = shieldUp;
    }
    private void OnTriggerEnter(Collider other)
    {        
        if (other.GetComponent<EnemyImpacter>())
        {
            if (isForPlayer)
            {
                betterFighter.GetComponent<PlayerStats>().TakeStamina(other.GetComponent<EnemyImpacter>().staminaDepletionPerHit);
            }
        }
    }
    public BoxCollider GetShieldCollider()
    {
        return shieldCollider;
    }
 }
