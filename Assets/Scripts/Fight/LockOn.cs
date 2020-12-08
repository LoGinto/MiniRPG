using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class LockOn : MonoBehaviour
{
    public Transform lookAtTransform = null;
    bool isLocked = false;
    [SerializeField] GameObject lockCinemachinecam;
    [SerializeField] GameObject baseCam;
    [SerializeField] float lockOnMovementSpeed = 0.5f;
    [SerializeField] float sweepRadius;
    [HideInInspector]private Movement movement;
    [HideInInspector]CinemachineFreeLook freeLook;
    Vector3 targetMovement;
    // Start is called before the first frame update
    void Start()
    {
        movement = FindObjectOfType<Movement>();
        freeLook = lockCinemachinecam.GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if (baseCam != null && lockCinemachinecam !=null)
        {
            baseCam.SetActive(!isLocked);
            lockCinemachinecam.SetActive(isLocked);
        }           
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            isLocked = !isLocked;
            if (isLocked)
            {
                SearchTheObjectToLockOn();
            }
            else
            {
                lookAtTransform = null;
            }
        }
        if (isLocked)
        {
            //lock on target and alter movement
            if (lookAtTransform != null)
            {
                freeLook.m_LookAt = lookAtTransform; 
                LockBehavior();
                try
                {
                    if (Vector3.Distance(transform.position, lookAtTransform.position) > lookAtTransform.GetComponent<LockTransformGetter>().lockResetDist)
                    {
                        isLocked = false;
                        lookAtTransform = null;
                    }
                }
                catch
                {
                    if (Vector3.Distance(transform.position, lookAtTransform.position) > lookAtTransform.GetComponentInParent<LockTransformGetter>().lockResetDist)
                    {
                        isLocked = false;
                        lookAtTransform = null;
                    }
                }
            }
        }
        else
        {
            freeLook.m_LookAt = null;
        }
        
    }
    void SearchTheObjectToLockOn()
    {
        float bestDistance = 999f;
        float distance;
        Collider bestValue = null;
        Collider[] colliders = Physics.OverlapSphere(transform.position, sweepRadius);
        foreach (var obj in colliders)
        {
            if (obj.GetComponent<LockTransformGetter>())
            {
                distance = Vector3.Distance(transform.position, obj.transform.position);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestValue = obj;
                }
            }
        }
        if(bestValue != null)
        {
            lookAtTransform = bestValue.GetComponent<LockTransformGetter>().GetLockToTransform();
        }
        else
        {
            isLocked = false;
        }
    }
    void LockBehavior()
    {
        transform.LookAt(lookAtTransform);
        targetMovement  = transform.forward * Input.GetAxis("Vertical") *lockOnMovementSpeed;
        targetMovement += transform.right * Input.GetAxis("Horizontal") * lockOnMovementSpeed ;
        targetMovement.y -= movement.GetGravity();
        //movement.MovementBlendTree(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));//this line is temp
        gameObject.GetComponent<CharacterController>().Move(targetMovement);
    }
    public bool GetLockState()
    {
        return isLocked;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sweepRadius);
    }
}
