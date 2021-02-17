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
    protected GameObject player;
    protected bool sight;
    bool comboChance;
    public Transform backStabberTransform;
    [SerializeField] float spottingDist;
    [SerializeField] bool sightBased;
    [SerializeField] protected float attackingDist;
    [SerializeField] float loseSightDist;
    [SerializeField] float runSpeed = 12;
    [SerializeField] float rotationSpeed = 35f;//angular speed in degrees
    [SerializeField] protected EnemyStat enemyStat;
    public WeaponObject weaponObject;
    public Transform parentForMelee;
    public bool isStun = false;
    [HideInInspector]protected NavMeshAgent navMeshAgent;
    [HideInInspector]protected Animator animator;    
    [HideInInspector]protected Vector3 lookAtPlayer;
    [HideInInspector]public string lastAttack;
    private float actualCoolDown = 0;
    bool isHavingattackAnim;
    public States state = States.calm;
    //private GameObject inst;
    public ActionState actionState = ActionState.calmBehavior;
    // Start is called before the first frame update
   protected virtual void Awake()
    {
        AssignWeapon();
    }
    public void SetStunEnemy()
    {
        isStun = true;
    }
    public void SetUnstunEnemy()
    {
        isStun = false;
    }
    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();                
        navMeshAgent.speed = runSpeed;
        
    }
    public void  BecomeAgressive()
    {
        if (GetComponent<Enemy>() != null)
        {
           sight = true;
           state = States.agressive;                       
        }
    }
   
    // Update is called once per frame
    protected virtual void Update()
    {
        comboChance = Random.value < enemyStat.comboChance;
        TickBehavior();
        AIMove();
        if (this.animator.GetCurrentAnimatorStateInfo(1).IsName("Zero"))
        {
            animator.SetBool("IsInteracting", false);
        }
        isHavingattackAnim =
             this.animator.GetCurrentAnimatorStateInfo(1).IsName(enemyStat.attackAnim1) || this.animator.GetCurrentAnimatorStateInfo(1).IsName(enemyStat.attackAnim2) || this.animator.GetCurrentAnimatorStateInfo(1).IsName(enemyStat.attackAnim3) || this.animator.GetCurrentAnimatorStateInfo(1).IsName(enemyStat.attackAnim4) || this.animator.GetCurrentAnimatorStateInfo(1).IsName(enemyStat.combo1) || this.animator.GetCurrentAnimatorStateInfo(1).IsName(enemyStat.combo2);
        lookAtPlayer = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
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
                    transform.LookAt(lookAtPlayer);
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
                            transform.LookAt(lookAtPlayer);
                            if (enemyStat.combo1 != "")
                            {
                                EnemyTargetAnim(enemyStat.combo1, true);
                                lastAttack = enemyStat.combo1;
                            }
                        }
                        else
                        {
                            transform.LookAt(lookAtPlayer);
                            if (enemyStat.combo2 != "")
                            {
                                EnemyTargetAnim(enemyStat.combo2, true);
                                lastAttack = enemyStat.combo2;
                            }
                        }                    
                }
            }
            actualCoolDown = enemyStat.attackCoolDown;
        }
    }
    public virtual void Chase()
    {        
        if (!isHavingattackAnim)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(player.transform.position);
        }        
    }
    public bool GetIsHavingAttackAnim()
    {
        return isHavingattackAnim;
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
        //go back to cal
        if (!sight && state == States.calm)
        {
            if (!sightBased)
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
            else
            {
                float minAngle = -50f;
                float maxAngle = 50f;
                Vector3 targetDir = player.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDir, transform.forward);
                if (viewableAngle > minAngle && viewableAngle < maxAngle)
                {
                    sight = true;
                    state = States.agressive;
                }
                else
                {
                    if (state == States.calm)
                    {
                        actionState = ActionState.calmBehavior;
                    }
                }
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
        if (animator.GetBool("IsInteracting")){ return; }
        animator.applyRootMotion = isInteracting;
        animator.SetBool("IsInteracting", isInteracting);
        animator.CrossFade(targetAnim, 0.2f);
    }
    public virtual void EnemyInterruptingAnim(string targetAnim)
    {
        if (animator.GetBool("IsInteracting"))
        {
            animator.CrossFade(targetAnim, 0.2f);
        }
    }
    public EnemyStat GetEnemyStat()
    {
        return enemyStat;
    }
    void AssignWeapon()
    {
        if(weaponObject != null)
        {           
            weaponObject.EquipOn(false,parentForMelee);            
        }
    }
}
