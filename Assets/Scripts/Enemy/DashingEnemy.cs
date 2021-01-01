using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingEnemy : Enemy
{
    public float dashDistance;
    public float dodashAtthisDistance = 3f;
    //public float dashTime = 2f;
    public float dashChance = 0.5f;
    public float dashSpeed;
    public float dashCooldown = 4f;
    private float actualdashCooldown = 0;
    Vector3 playerPos;
    bool isDashing =false;
    CharacterController controller;
    Vector2 gravity = new Vector2(0, -20);
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        controller = this.GetComponent<CharacterController>();
        controller.detectCollisions = false;
    }
    public override void AIMove()
    {
        if (!isDashing)
        {
            base.AIMove();
        }
    }
    protected override void Update()
    {
        base.Update();
        if (this.animator.GetCurrentAnimatorStateInfo(1).IsName("Zero")|| this.animator.GetCurrentAnimatorStateInfo(1).IsName("Running"))
        {
            animator.SetBool("IsInteracting",false);
        }       
    }
    public override void EnemyTargetAnim(string targetAnim, bool isInteracting)
    {
        base.EnemyTargetAnim(targetAnim, isInteracting);
    }
    public override void Attack()
    {
        controller.enabled = false;
        if (!isDashing)
        {
            base.Attack();
        }
        else if(isDashing)
        {
            EnemyTargetAnim("SpinAttack", true);
            isDashing = false;
        }
    }
    public override void Chase()
    {
        bool dashChanceBool = Random.value < dashChance;
        if (!dashChanceBool)
        {
            controller.enabled = false;
            base.Chase();
        }
        else if (dashChanceBool && !isDashing)
        {
            if (actualdashCooldown > 0)
            {
                actualdashCooldown -= 1 * Time.deltaTime;
            }
            else
            {
                if (Vector3.Distance(transform.position, player.transform.position) >= dodashAtthisDistance)
                {
                    controller.enabled = true;
                    if (!isDashing)
                    {
                        isDashing = true;
                        StartCoroutine(Dash());
                    }
                    //animator.SetInteger("DashStage", 3);
                }
                actualdashCooldown = dashCooldown;
            }
        }
    }
    private IEnumerator Dash()
    {
        navMeshAgent.isStopped = true;
        EnemyTargetAnim("Sword_Crouch", true);
        yield return new WaitForSeconds(1.5f);       
        Dashing();
        //controller.enabled = false;
    }
    void Dashing()
    {
        playerPos = player.transform.position;
        dashDistance = Vector3.Distance(transform.position, player.transform.position);
        if (Vector3.Distance(transform.position, player.transform.position) > attackingDist)
        {           
            navMeshAgent.isStopped = true;
            Vector3 dashTo;
            transform.LookAt(lookAtPlayer);            
            dashTo = ((player.transform.position - transform.position).normalized * dashDistance) * dashSpeed * Time.deltaTime;
            controller.Move(dashTo);
            transform.position = Vector3.Lerp(transform.position, (player.transform.position - transform.position).normalized * dashDistance, dashSpeed * Time.deltaTime);
        }       
    }
}

