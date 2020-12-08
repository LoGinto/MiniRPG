using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public virtual void TakeDamage(float damage)
    {
        FindObjectOfType<PlayerStats>().health -= damage;
    }
}
