using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulItem;

public class Fighter : MonoBehaviour
{
    AnimationPlayer animationPlayer;
   
    PlayerInventory inventory;

    private void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
    }
    private void Start()
    {
        //animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        animationPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<AnimationPlayer>();
    }
    public void HandleLightAttack(WeaponItem weapon)
    {
        animationPlayer.PlayerTargetAnim(weapon.On_Light_Attack_1, true);
    }
    public void HandleHeavyAttack(WeaponItem weapon)
    {
        animationPlayer.PlayerTargetAnim(weapon.On_Heavy_Attack_1, true);
    }
    private void Update()
    {
        if (gameObject.GetComponent<Movement>().GetRollFlag() == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleLightAttack(inventory.rightWeapon);
            }
            if (Input.GetMouseButtonDown(1))
            {
                HandleHeavyAttack(inventory.rightWeapon);
            }
        }
    }
}
