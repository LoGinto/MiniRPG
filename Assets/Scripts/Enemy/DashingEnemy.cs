using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingEnemy : Enemy
{
    public float dashDistance;
    public float dashTime = 2f;
    public float dashChance = 0.5f;
    public float stopNear = 0.5f;
    public float dashSpeed;
    Vector3 playerPos;
    bool isDashing;
    CharacterController controller;
    Vector2 gravity = new Vector2(0, -20);
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        controller = GetComponent<CharacterController>();
        controller.detectCollisions = false;
    }
    protected override void Update()
    {
        base.Update();       
    }
    public override void Chase()
    {
        bool dashChanceBool = Random.value < dashChance;
        if (!dashChanceBool)
        {
            base.Chase();
        }
        else if (dashChanceBool)
        {            
            StartCoroutine(Dash());
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
            Vector3 dashTo = playerPos-transform.position * dashSpeed * Time.deltaTime;
            //dashTo.y = gravity.y;
            controller.Move(dashTo);
            if (Vector3.Distance(transform.position,player.transform.position)<=stopNear)
            {
                break; 
            }
            yield return null;
        }
        isDashing = false;  
        //controller.enabled = false;
    }
}
