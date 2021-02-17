using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIK : MonoBehaviour
{
    [HideInInspector]Animator animator;
    [SerializeField] Vector3 footIkOffset;
    [SerializeField] Vector3 footIkOffset2;
    [SerializeField] Transform mesh;
    float ikWeight;
  
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame 
    void Update()
    {
        bool turnOffAnim = this.animator.GetCurrentAnimatorStateInfo(3).IsName("BlockIdle") || this.animator.GetCurrentAnimatorStateInfo(2).IsName("Roll")|| this.animator.GetCurrentAnimatorStateInfo(2).IsName("Roll")||animator.GetFloat("Move")>0.5f|| this.animator.GetCurrentAnimatorStateInfo(2).IsName("BackStab")|| animator.GetFloat("Move") > 0.5f || this.animator.GetCurrentAnimatorStateInfo(2).IsName("ParryShield")|| this.animator.GetCurrentAnimatorStateInfo(3).IsName("PutOnShield")|| this.animator.GetCurrentAnimatorStateInfo(4).IsName("GunShoot");
        if (turnOffAnim == true)
        {
            ikWeight = 0;
        }
        else
        {
            ikWeight = 1;
        }
    }
    private void OnAnimatorIK(int layerIndex)
    {
        Vector3 p_leftFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot).position;
        Vector3 p_rightFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot).position;
        p_leftFoot = GetHitPoint(p_leftFoot + Vector3.up, p_leftFoot - Vector3.up * 5) + footIkOffset;
        p_rightFoot = GetHitPoint(p_rightFoot + Vector3.up, p_rightFoot - Vector3.up * 5)+footIkOffset2;
        mesh.transform.localPosition = new Vector3(0, -Mathf.Abs((p_leftFoot.y - p_rightFoot.y) / 2), 0);   
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, ikWeight);
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, ikWeight);
        animator.SetIKPosition(AvatarIKGoal.LeftFoot,p_leftFoot);
        animator.SetIKPosition(AvatarIKGoal.RightFoot, p_rightFoot);

    }
    private Vector3 GetHitPoint(Vector3 start, Vector3 end)
    {
        RaycastHit hit;
        if (Physics.Linecast(start, end, out hit))
        {
            return hit.point;
        }
        return end;
    } 
}
