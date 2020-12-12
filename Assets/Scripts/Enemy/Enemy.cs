﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using UnityEditor.Animations;
public class Enemy : MonoBehaviour,IAI
{    
    //base class
    public enum States
    {
        calm,agressive
    }
    public enum ActionState
    {
       calmBehavior,chasing, lostSight,attacking
    }
    GameObject player;
    bool sight;
    bool comboChance;
    [SerializeField] float spottingDist;
    [SerializeField] float attackingDist;
    [SerializeField] float loseSightDist;
    [SerializeField] float runSpeed = 12;
    [SerializeField]private float attackCoolDown = 2f;
    [SerializeField] EnemyStat enemyStat;
    [HideInInspector]NavMeshAgent navMeshAgent;
    [HideInInspector]Animator animator;    
    [HideInInspector]Vector3 lookAtPlayer;
    string lastAttack;
    private float actualCoolDown = 0;
    public States state = States.calm;
    public ActionState actionState = ActionState.calmBehavior;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();                
        navMeshAgent.speed = runSpeed;
       
    }
    // Update is called once per frame
    void Update()
    {
        comboChance = Random.value < 0.25f;
        TickBehavior();
        AIMove();
        if (this.animator.GetCurrentAnimatorStateInfo(1).IsName("Zero"))
        {
            animator.SetBool("IsInteracting", false);
        }
    }
    public virtual void TickBehavior()
    {
            StateChange();
            // AIMove();               
            if (actionState == ActionState.calmBehavior)
            {
                Calm();
            }
            else if (state == States.agressive && actionState == ActionState.chasing)
            {
                Chase();
            }
            else if (state == States.agressive && actionState == ActionState.attacking)
            {
                Attack();
            }
            else if (state == States.agressive && actionState == ActionState.lostSight)
            {
                LostSight();
            }
        
    }
    public virtual void Attack()
    {
        navMeshAgent.isStopped = true;
        lookAtPlayer = new Vector3(player.transform.position.x,transform.position.y,player.transform.position.z);
        //play attack anim
        if (actualCoolDown > 0)
        {
            actualCoolDown -= 1 * Time.deltaTime;
        }
        else 
        {
            if (!animator.GetBool("IsInteracting"))
            {
                if (!comboChance)
                {
                    if (lastAttack == enemyStat.attackAnim1)
                    {
                        if (enemyStat.attackAnim2 != "")
                        {
                            EnemyTargetAnim(enemyStat.attackAnim2, true);
                            lastAttack = enemyStat.attackAnim2;
                        }
                        else
                        {
                            EnemyTargetAnim(enemyStat.attackAnim1, true);
                            lastAttack = enemyStat.attackAnim1;
                        }
                    }
                    else if (lastAttack == enemyStat.attackAnim2)
                    {
                        if (enemyStat.attackAnim3 != "")
                        {
                            EnemyTargetAnim(enemyStat.attackAnim3, true);
                            lastAttack = enemyStat.attackAnim3;
                        }
                        else
                        {
                            EnemyTargetAnim(enemyStat.attackAnim1, true);
                            lastAttack = enemyStat.attackAnim1;
                        }

                    }
                    else if (lastAttack == enemyStat.attackAnim3)
                    {
                        if (enemyStat.attackAnim4 != "")
                        {
                            EnemyTargetAnim(enemyStat.attackAnim4, true);
                            lastAttack = enemyStat.attackAnim4;
                        }
                        else
                        {
                            EnemyTargetAnim(enemyStat.attackAnim1, true);
                            lastAttack = enemyStat.attackAnim1;
                        }
                    }
                    else
                    {
                        EnemyTargetAnim(enemyStat.attackAnim1, true);
                        lastAttack = enemyStat.attackAnim1;
                    }
                }
                else
                {
                    //do combo 
                    //last attack = combo  so we go back to attack anim1
                    
                        int randomValueN2 = Random.Range(1, 3);
                        if(randomValueN2 == 1)
                        {
                            if (enemyStat.combo1 != "")
                            {
                                EnemyTargetAnim(enemyStat.combo1, true);
                                lastAttack = enemyStat.combo1;
                            }
                        }
                        else
                        {
                            if (enemyStat.combo2 != "")
                            {
                                EnemyTargetAnim(enemyStat.combo2, true);
                                lastAttack = enemyStat.combo2;
                            }
                        }                    
                }
            }
            actualCoolDown = attackCoolDown;
        }
    }
    public virtual void Chase()
    {
        //if (IsFarTo(attackingDist))
        //{
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(player.transform.position);
        //}              
    }
    public virtual void Calm()
    {
        Debug.Log(gameObject.name + " is calm");
    }
    public virtual void AIMove()
    {
        if (navMeshAgent.velocity.magnitude > 0)
        {
            animator.SetBool("Run", true);
            //animator.SetLayerWeight(1, 0);
        }
        else
        { animator.SetBool("Run", false); }
        //animations and movement logic in general
        //animator.SetFloat("BlendMove", navMeshAgent.velocity.magnitude/navMeshAgent.speed);
    }
    public virtual void LostSight()
    {
        //must: change sight bool and enum state to calm
        StartCoroutine(WaitandCalmDown());
    }
    IEnumerator WaitandCalmDown()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("Calm down coroutine exectued");
        sight = false;
        state = States.calm;
    }
    public virtual void StateChange()
    {
        //if state calm---> walk or stay on place
        //if state agressive and not within attack dist ---> chase 
        //if close enough ---> attack
        //if player far enough ---> lost sight behavior
        //go back to calm
        if (!sight && state == States.calm)
        {
            if (IsCloseTo(spottingDist))
            {
                sight = true;
                state = States.agressive;
            }
            else
            {
                actionState = ActionState.calmBehavior;
            }
        }
        if (IsCloseTo(attackingDist) && state == States.agressive && sight)
        {
            actionState = ActionState.attacking;
        }
        else if (IsFarTo(attackingDist) && IsCloseTo(loseSightDist)&& state == States.agressive && sight)
        {
            actionState = ActionState.chasing;
        }
        else if (IsFarTo(loseSightDist)&&state == States.agressive&&sight)
        {
            actionState = ActionState.lostSight;           
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spottingDist);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, loseSightDist);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, attackingDist);
    }
    public bool IsFarTo(float distance)
    {
        return Vector3.Distance(transform.position, player.transform.position) >= distance;
    }
    public bool IsCloseTo(float dist)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= dist;
    }
    public virtual void EnemyTargetAnim(string targetAnim, bool isInteracting)
    {
        animator.applyRootMotion = isInteracting;
        animator.SetBool("IsInteracting", isInteracting);
        animator.CrossFade(targetAnim, 0.2f);
    }
}
