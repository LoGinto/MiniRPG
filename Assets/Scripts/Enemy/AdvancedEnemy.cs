using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedEnemy : Enemy
{
    [SerializeField] float rollChaseDistance;
    [SerializeField] float flipDistanceperSec = 0.20f;
    [SerializeField] float rollSpeed = 0.5f;
    [SerializeField] float cd = 1.5f;
    float actualcd = 0;
    EnemyHealth health;
    protected override void Start()
    {
        base.Start();
        health = GetComponent<EnemyHealth>();
        navMeshAgent.stoppingDistance = attackingDist;
    }
    protected override void Update()
    {
        base.Update();
        if (this.animator.GetCurrentAnimatorStateInfo(1).IsName("BackFlip"))
        {
            transform.Translate(-Vector3.forward * flipDistanceperSec * Time.deltaTime);
        }
        else if (this.animator.GetCurrentAnimatorStateInfo(1).IsName("EnemyRoll"))
        {
            transform.Translate(Vector3.forward * flipDistanceperSec * Time.deltaTime);
        }
    }
    public override void Attack()
    {
        //start by attacking after roll
        //one more attack
        //roll back
        //sword dash--optional
        navMeshAgent.isStopped = true;
        transform.LookAt(lookAtPlayer);
        if (actualcd > 0)
        {
            actualcd -= 1 * Time.deltaTime;
        }
        else /*if(actualcd<=0)*/
        {
            if (lastAttack == "" && animator.GetBool("IsInteracting") == false)
            {
                EnemyTargetAnim(enemyStat.attackAnim1, true);
                lastAttack = enemyStat.attackAnim1;
            }
            else if (animator.GetBool("IsInteracting") == false && lastAttack == enemyStat.attackAnim1)
            {
                EnemyTargetAnim(enemyStat.attackAnim2, true);
                lastAttack = enemyStat.attackAnim2;
            }
            else if (lastAttack == enemyStat.attackAnim2 && animator.GetBool("IsInteracting") == false)
            {
                EnemyTargetAnim("BackFlip", true);
                lastAttack = "BackFlip";
            }
            else if (lastAttack == "BackFlip" && animator.GetBool("IsInteracting") == false)
            {
                EnemyTargetAnim(enemyStat.attackAnim3, true);
                lastAttack = "";
            }
            actualcd = cd;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rollChaseDistance);
    }
    public override void Chase()
    {
        //run towards player
        //roll at certain distance
        //on small hp drink potion before running 
        navMeshAgent.isStopped = false; 
        if (Vector3.Distance(transform.position,player.transform.position)>rollChaseDistance)
        {
            Debug.Log("Base chase");
            base.Chase();
        }
        else if(Vector3.Distance(transform.position, player.transform.position) <= rollChaseDistance&& Vector3.Distance(transform.position, player.transform.position) > attackingDist)
        {            
            if(lastAttack == "BackFlip")
            {
                Debug.Log("Attack 3 before if");
                if (!animator.GetBool("IsInteracting") && !this.animator.GetCurrentAnimatorStateInfo(1).IsName("EnemyRoll") && !this.animator.GetCurrentAnimatorStateInfo(1).IsName("BackFlip"))
                {
                    Debug.Log("Attack3 in if");
                    EnemyTargetAnim(enemyStat.attackAnim3, true);
                    lastAttack = "";
                }
            }
            else 
            {
                Debug.Log($"roll to player/{lastAttack}");
                if (!this.animator.GetCurrentAnimatorStateInfo(1).IsName("EnemyRoll"))
                {
                    RollTowardsPlayer();
                }
            }
        }
    }
    void RollTowardsPlayer()
    {
        //navMeshAgent.isStopped = true;
        //navMeshAgent.velocity = Vector3.zero;
        transform.LookAt(lookAtPlayer);
        EnemyTargetAnim("EnemyRoll", true);
    }
}
