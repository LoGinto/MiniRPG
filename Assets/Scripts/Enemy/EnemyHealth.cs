using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHealth : HealthManager
{
    [SerializeField]EnemyStat enemyStat;
    public Slider healthSlider;
    private float health;
    bool showHealthBar = false;
    private float maxhealth;
    [HideInInspector]Animator animator;
    [HideInInspector]BetterFighter betterFighter; 
    // Start is called before the first frame update
    void Start()
    {
        betterFighter = FindObjectOfType<BetterFighter>();
        maxhealth = (float)enemyStat.level * 10;
        health = maxhealth;
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        healthSlider.gameObject.SetActive(showHealthBar);
        healthSlider.value = health / maxhealth;
    }
    public override void TakeDamage(float damage)
    {
        showHealthBar = true;
        health -= damage;
        try
        {
            if (betterFighter.GetLastAttack() == "LightAttack1")
            {
                animator.CrossFade(betterFighter.weaponObject.lightAttackImpactAnimName, 0.1f);
            }
            else if (betterFighter.GetLastAttack() == "LightAttack2")
            {
                animator.CrossFade(betterFighter.weaponObject.lightAttackImpactAnimName, 0.1f);
            }
            else if (betterFighter.GetLastAttack() == "HeavyAttack1")
            {
                animator.CrossFade(betterFighter.weaponObject.heavyAttackImpactAnimName, 0.1f);
            }
            else if (betterFighter.GetLastAttack() == "HeavyAttack2")
            {
                animator.CrossFade(betterFighter.weaponObject.heavyAttack2ImpactAnimName, 0.1f);
            }
        }
        catch
        {
            Debug.Log("Missing  impac animation on " + betterFighter.weaponObject);
        }
    }
}
