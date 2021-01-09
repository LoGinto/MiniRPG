﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void PlayerTargetAnim(string targetAnim, bool isInteracting)
    {
        if (animator.GetBool("IsInteracting")) { return; }
        animator.applyRootMotion = isInteracting;
        animator.SetBool("IsInteracting", isInteracting);
        animator.CrossFade(targetAnim, 0.2f);
    }
    public void PlayerTargetAnim(Animator myAnimator, string targetAnim, bool isInteracting)
    {
        if (myAnimator.GetBool("IsInteracting")) { return; }
        myAnimator.applyRootMotion = isInteracting;
        myAnimator.SetBool("IsInteracting", isInteracting);
        myAnimator.CrossFade(targetAnim, 0.2f);
    }
    
    public Animator GetAnimator()
    {
        return animator;
    }
    public void EnableCombo()
    {
        animator.SetBool("CanDoCombo", true);         
    }
    public void DisableCombo()
    {
        animator.SetBool("CanDoCombo", false);
    }
    private void Update()
    {
        if (this.animator.GetCurrentAnimatorStateInfo(4).IsName("ZeroState"))
        {
            StartCoroutine(Wait());
        }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
        if (animator.GetBool("IsInteracting") == true)
        {
            animator.SetBool("IsInteracting", false);
        }
    }
}
