using System.Collections;
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
    [SerializeField] float spottingDist;
    [SerializeField] float attackingDist;
    [SerializeField] float loseSightDist;
    [SerializeField] float runSpeed = 12;
    [HideInInspector]NavMeshAgent navMeshAgent;
    [HideInInspector]Animator animator;
    
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
        TickBehavior();
        AIMove();
    }
    public virtual void TickBehavior()
    {
        StateChange();
       // AIMove();
        if(actionState == ActionState.calmBehavior)
        {
            Calm();
        }
        else if(state == States.agressive && actionState == ActionState.chasing)
        {
            Chase();
        }
        else if (state == States.agressive && actionState == ActionState.attacking)
        {
            Attack();
        }
        else if(state == States.agressive && actionState == ActionState.lostSight)
        {
            LostSight();
        }
    }
    public virtual void Attack()
    {
        Debug.Log(gameObject.name + " attacks");
    }
    public virtual void Chase()
    {
        if (IsFarTo(attackingDist))
        {
            navMeshAgent.SetDestination(player.transform.position);
        }              
    }
    public virtual void Calm()
    {
        Debug.Log(gameObject.name + " is calm");
    }
    public virtual void AIMove()
    {
        //animations and movement logic in general
        animator.SetFloat("Blend", navMeshAgent.velocity.magnitude);
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
}
