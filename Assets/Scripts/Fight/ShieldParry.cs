using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldParry : MonoBehaviour
{
    Shield shield;
    [SerializeField] KeyCode shieldParryKeycode;
    [HideInInspector]AnimationPlayer animationPlayer;
    Collider parryCollider;
    BetterFighter betterFighter = null;
    EnemyImpacter impacter;
    private void Start()
    {
        shield = GetComponentInParent<Shield>();
        if (shield.isForPlayer)
        {
            betterFighter = FindObjectOfType<BetterFighter>();
            animationPlayer = FindObjectOfType<AnimationPlayer>();
        }
        parryCollider = GetComponent<Collider>();
        parryCollider.enabled = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(shieldParryKeycode))
        {
            if (shield.isForPlayer)
            {
                if (betterFighter.leftHandWeapon != null && betterFighter.leftHandWeapon.isAShield)
                {                    
                    animationPlayer.PlayerTargetAnim("ParryShield", true);                    
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyImpacter>())
        {
            if (shield.isForPlayer) {
                impacter = other.GetComponent<EnemyImpacter>();
                Debug.Log($"{impacter.transform.root} should be parried");
                impacter.transform.root.GetComponent<Enemy>().EnemyInterruptingAnim("Stun");
                impacter.transform.root.GetComponent<Enemy>().EnemyTargetAnim("Stun", true);
            }

        }
    }
    public EnemyImpacter GetEnemyImpacter()
    {
        return impacter;
    }
    public void Parry()
    {
        shield.shieldUp = false;
        //Debug.Log("Succesful parry anim event");
        this.parryCollider.enabled = true;
        shield.GetShieldCollider().enabled = false;
    }
    public void BackToNormal()
    {
        //Debug.Log("Succesful unparry anim event");
        parryCollider.enabled = false;
    }
}
