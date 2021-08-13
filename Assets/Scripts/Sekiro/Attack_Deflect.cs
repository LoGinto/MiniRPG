using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Deflect : MonoBehaviour
{
   [SerializeField] int deflectLayer = 1;
    [HideInInspector]Animator animator;
    [HideInInspector]AnimationPlayer animationPlayer;
    [HideInInspector] int deflectAnimationHash = Animator.StringToHash("Deflect");
    bool deflectAnim = false;
    void Start(){
        animationPlayer = GetComponent<AnimationPlayer>();
        animator = GetComponent<Animator>();
    }
   void Update(){
       if(Input.GetMouseButton(1)){
            Debug.Log("RMB held");
            animator.SetLayerWeight(deflectLayer,1);
             //if(!animator.GetCurrentAnimatorStateInfo(1).IsName("Deflect")){           
                if(!deflectAnim){
                     animationPlayer.PlayerTargetAnim(deflectAnimationHash,false);
                    deflectAnim = true;
               // }         
             }
          }
       else{
           Debug.Log("RMB not held");         
          deflectAnim = false;
            if(!animator.GetCurrentAnimatorStateInfo(1).IsName("Deflect")){
                animator.SetLayerWeight(deflectLayer,0);
            }
       }
   }
}
