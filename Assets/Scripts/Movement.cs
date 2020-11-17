using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    [SerializeField] float gravity = 20f;
    AnimationPlayer animationPlayer;
    Animator animator;
    bool rollFlag;
    bool isInteracting;
    Camera myCamera;
    CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        myCamera = GameObject.FindObjectOfType<Camera>();
        animator = GetComponent<Animator>();
        animationPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<AnimationPlayer>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isInteracting = animator.GetBool("IsInteracting");
        HandleRollInput();
        Move();
        rollFlag = false;
    }
    
    void HandleRollInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rollFlag = true;
        }
    }
    
    void Move()
    {
        if (animator.GetBool("IsInteracting"))
        {
            return;
        }
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");
        Vector3 cameraForward = Vector3.Scale(myCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 udpatedVector = verticalAxis * cameraForward + horizontalAxis * myCamera.transform.right;
        transform.LookAt(udpatedVector + transform.position);
        Vector3 actualMovement = udpatedVector * speed * Time.fixedDeltaTime;
        //animator.SetBool("Walk", true);
        actualMovement.y -= gravity;
        characterController.Move(actualMovement);
        MovementBlendTree(verticalAxis, horizontalAxis);
        if (rollFlag)
        {
            if (verticalAxis != 0 || horizontalAxis != 0)
            {               
                animationPlayer.PlayerTargetAnim("Roll",true);
                actualMovement.y = 0;
                Quaternion rollRot = Quaternion.LookRotation(actualMovement);
                transform.rotation = rollRot;
            }
            else
            {
                animationPlayer.PlayerTargetAnim("Dodge", true);
            }
        }        
    }

    private void MovementBlendTree(float verticalAxis, float horizontalAxis)
    {
        if (verticalAxis != 0)
        {
            animator.SetFloat("Move", Mathf.Abs(verticalAxis));
        }
        else if (horizontalAxis != 0 && verticalAxis == 0)
        {
            animator.SetFloat("Move", Mathf.Abs(horizontalAxis));
        }
        else if (horizontalAxis == 0 && verticalAxis == 0)
        {
            float goToZero = Mathf.Lerp(animator.GetFloat("Move"), 0, Time.deltaTime);
            animator.SetFloat("Move", goToZero);
        }
    }
    public bool GetRollFlag()
    {
        return rollFlag;
    }
}




