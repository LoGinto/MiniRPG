using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Enemy
{
    [SerializeField] float farAttack;
    [SerializeField] float closeAttack;
    [SerializeField] float farAttackCD = 3f;
    [SerializeField] GameObject throwingObj;
    [SerializeField] GameObject missles; 
    [SerializeField] Transform instantiateTransform;
    float actualfarAttackCD;
    Vector3 playerVector;
    public enum FarVariance {variance1,variance2}
    public FarVariance farVariance = FarVariance.variance1;
    public override void Chase()
    {
        base.Chase();
    }
    public override void Attack()
    {
        if (IsFarTo(closeAttack)&&IsCloseTo(closeAttack))
        {
            FarAttack();
        }
        else if (IsCloseTo(closeAttack) && IsCloseTo(farAttack))
        {
            CloseAttack();
        }
    }
    void FarAttack()
    {
        if (actualfarAttackCD>0)
        {
            actualfarAttackCD -= 1 * Time.deltaTime;
        }
        else 
        {
            //do smth
            int oneRandom = Random.Range(0, 3);
            if(oneRandom == 1)
            {
                farVariance = FarVariance.variance1;
            }
            else if(oneRandom == 2)
            {
                farVariance = FarVariance.variance2; 
            }
            if(farVariance == FarVariance.variance1)
            {
                EnemyTargetAnim("ThrowBall", true);
            }
            else if(farVariance == FarVariance.variance2)
            {
                EnemyTargetAnim("CastProjectiles", true);
            }
            actualfarAttackCD = farAttackCD;
        }
    }
    void CloseAttack()
    {
        base.Attack();
    }
    public void FarAttackThrowANim()
    {
        Instantiate(throwingObj, instantiateTransform.position, Quaternion.identity);
    }
    public void FarAttackMultipleAnims()
    {
        Instantiate(missles, instantiateTransform.position, Quaternion.identity);
    }
}
