using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingEnemy : Enemy
{
    public float dashDistance;
    public float dodashAtthisDistance = 3f;
    public float dashTime = 2f;
    public float dashChance = 0.5f;
    public float dashSpeed;
    public float dashCooldown = 4f;
    private float actualdashCooldown = 0;
    Vector3 playerPos;
    bool isDashing;
    CharacterController controller;
    Vector2 gravity = new Vector2(0, -20);
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        controller = this.GetComponent<CharacterController>();
        controller.detectCollisions = false;
    }
    protected override void Update()
    {
        base.Update();       
    }
    public override void Attack()
    {
        base.Attack();
    }
    public override void Chase()
    {
        bool dashChanceBool = Random.value < dashChance;
        if (!dashChanceBool)
        {
            controller.enabled = false;
            base.Chase();          
        }
        else if (dashChanceBool)
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
                    StartCoroutine(Dash());
                }
                actualdashCooldown = dashCooldown;
            }
        }
    }
    private IEnumerator Dash()
    {
        float startTime = Time.time;
        //controller.enabled = true;
        playerPos = player.transform.position;
        dashDistance = Vector3.Distance(transform.position, player.transform.position);
        while (Time.time < startTime + dashTime)
        {
            isDashing = true;
            navMeshAgent.isStopped = true;
            //dashTo.y = gravity.y;         
            Vector3 dashTo;
            if (Vector3.Distance(transform.position,player.transform.position)>attackingDist)
            {
                transform.LookAt(lookAtPlayer);
                dashTo = ((player.transform.position-transform.position).normalized * dashDistance)* dashSpeed * Time.deltaTime;
                controller.Move(dashTo);
            }
            else if (Vector3.Distance(transform.position, player.transform.position) <= attackingDist)
            {               
                break;
            }
            yield return null;
        }
        isDashing = false;
        navMeshAgent.isStopped = false;
        //controller.enabled = false;
    }
}
