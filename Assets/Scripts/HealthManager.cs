using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public virtual void TakeDamage(float damage)
    {
        FindObjectOfType<PlayerStats>().health -= damage;
    }
    public virtual void TakeDamageFromThrowable(float damage,string hurtAnim)
    {
        FindObjectOfType<PlayerStats>().health -= damage;
        FindObjectOfType<AnimationPlayer>().PlayerTargetAnim(hurtAnim,true);
    }
}
