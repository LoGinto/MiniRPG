using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerEnemy : MonoBehaviour
{
    public bool isPerformingAction;
    ManageLocomotionEnemy enemyLocomotionManager;
    [Header("AI settings ")]
    public float detectionRadius = 10f;
    public float minimumDetectionAngle = -50;
    public float maximumDetectionAngle = 50;
    private void Awake()
    {
        enemyLocomotionManager = GetComponent<ManageLocomotionEnemy>();
    }
    private void FixedUpdate()
    {
        HandleCurrentAction();
    }
    void HandleCurrentAction()
    {
        if(enemyLocomotionManager.currentTarget == null)
        {
            enemyLocomotionManager.HandleDetection();
        }
        else
        {
            enemyLocomotionManager.HandleMoveToTarget();
        }
    }
}
