using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public bool shieldUp;
    public bool isForPlayer;
    BetterFighter betterFighter = null;
    private void Start()
    {
        if (isForPlayer)
        {
            betterFighter = GameObject.FindGameObjectWithTag("Player").GetComponent<BetterFighter>();            
        }
    }
    private void Update()
    {
        if (isForPlayer)
        {
            shieldUp = betterFighter.GetIsHoldingShield();
        }
        gameObject.GetComponent<BoxCollider>().enabled = shieldUp;
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
}
