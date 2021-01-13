using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbushingEnemy : Enemy
{
    public bool hasAmbushed = false;
    public bool playerDetected = false;
    public float minAngle = -50f;
    public float maxAngle = 50f;
    public float detectionRadius = 3f;
    public LayerMask detectionLayer;
    public override void TickBehavior()
    {
        if (hasAmbushed)
        {
            base.TickBehavior();
        }
        else
        {
            if (!playerDetected)
            {
                EnemyTargetAnim("Sitting", false);
                DetectPlayerFromAmbush();
            }
            else
            {
                Ambush();
            }
        }
    }
    private void Ambush()
    {
        EnemyTargetAnim("StandUp", true);
        //hasAmbushed = true;       
    }
    public void SetHasAmbushedAnim()
    {
        hasAmbushed = true;
    }
    private void DetectPlayerFromAmbush()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            PlayerStats playerStats = colliders[i].transform.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                Vector3 targetDir = playerStats.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDir, transform.forward);
                if (viewableAngle > minAngle && viewableAngle < maxAngle)
                {
                    playerDetected = true;
                }
            }
        }
    }
}
