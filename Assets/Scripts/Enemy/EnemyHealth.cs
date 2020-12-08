using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHealth : HealthManager
{
    [SerializeField]EnemyStat enemyStat;
    private float health;
    bool isHit = false;
    private float maxhealth;
    public Slider healthSlider;
    // Start is called before the first frame update
    void Start()
    {      
        maxhealth = (float)enemyStat.level * 10;
        health = maxhealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.gameObject.SetActive(isHit);
        healthSlider.value = health / maxhealth;
    }
    public override void TakeDamage(float damage)
    {
        isHit = true;
        health -= damage;
    }
}
