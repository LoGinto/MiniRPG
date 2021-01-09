using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ManageLocomotionEnemy : MonoBehaviour
{
    ManagerEnemy enemyManager;
    EnemyAnimatorManager enemyAnimatorManager;
    public PlayerStats currentTarget;
    public LayerMask detectionLayer;
    NavMeshAgent navMeshAgent;
    public Rigidbody enemyRigidBody;
    public float distanceFromTarget;
    public float speed = 3f;
    public float stoppingDistance = 0.5f;
    public float rotationSpeed = 15f;
    private void Awake()
    {
        enemyRigidBody = GetComponent<Rigidbody>();
        enemyManager = GetComponent<ManagerEnemy>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>(); 
        enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
    }
    private void Start()
    {
        navMeshAgent.enabled = false;
        enemyRigidBody.isKinematic = false;
    }
    public void HandleDetection()
    {        
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius,detectionLayer);
        for(int i = 0; i < colliders.Length; i++)
        {
            PlayerStats playerStats = colliders[i].transform.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                Vector3 targetDir = playerStats.transform.position - transform.position;
                float vieawableAngle = Vector3.Angle(targetDir, transform.forward);
                if (vieawableAngle > enemyManager.minimumDetectionAngle && vieawableAngle<enemyManager.maximumDetectionAngle)
                {
                    currentTarget = playerStats;
                }
            }
        }
    }
    public void HandleMoveToTarget()
    {
        Vector3 targetDirection = currentTarget.transform.position - transform.position;
        distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);

        float viewableAngle = Vector3.Angle(targetDirection,transform.forward);
        if (enemyManager.isPerformingAction)
        {
            enemyAnimatorManager.GetAnimator().SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            navMeshAgent.enabled = false;
        }
        else
        {
            if(distanceFromTarget > stoppingDistance)
            {
                enemyAnimatorManager.GetAnimator().SetFloat("Vertical", 1f, 0.1f, Time.deltaTime);
            }
            else if (distanceFromTarget<=stoppingDistance)
            {
                enemyAnimatorManager.GetAnimator().SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
            }
        }
        HandleRotationTowardsTarget();
        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;
    }
    private void HandleRotationTowardsTarget()
    {
        if (enemyManager.isPerformingAction)
        {
            //rotate manually
            Vector3 direction = currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();
            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed / Time.deltaTime);
        }
        else
        {
            //rotate with nav mesh
            Vector3 relativeDirection = transform.InverseTransformDirection(navMeshAgent.desiredVelocity);
            Vector3 targetVelocity = enemyRigidBody.velocity;
            navMeshAgent.enabled = true;
            navMeshAgent.SetDestination(currentTarget.transform.position);
            enemyRigidBody.velocity = targetVelocity;
            transform.rotation = Quaternion.Slerp(transform.rotation, navMeshAgent.transform.rotation, rotationSpeed / Time.deltaTime);
        }
    }  
}
