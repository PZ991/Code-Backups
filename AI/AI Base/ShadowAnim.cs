using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowAnim : MonoBehaviour
{
    public AnimatorOverrideController overrideController;
    public bool[] HumanoidUse = new bool[7];
    public Animator animator;
    void Start()
    {
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayShadowAnim(AnimationClip clip,int layer)
    {
        


            if (animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 1f && !animator.IsInTransition(1) && HumanoidUse[1] == true && layer == 1)
            {
                overrideController["Test LA1"] = clip;
                //Debug.Log(clip[0].animation.name);
                animator.CrossFade("Test LA1", 0.5f, 1, -0.5f);

                HumanoidUse[1] = false;
            }
            else if (animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 1f && !animator.IsInTransition(1) && HumanoidUse[1] == false && layer == 1)
            {
                overrideController["Test LA"] = clip;
                animator.CrossFade("Test LA", 0.5f, 1, -0.5f);
                HumanoidUse[1] = true;
            }

        
            if (animator.GetCurrentAnimatorStateInfo(2).normalizedTime > 1f && !animator.IsInTransition(2) && HumanoidUse[2] == true && layer == 2)
            {

                overrideController["Test RA1"] = clip;
                animator.CrossFade("Test RA1", 0.5f, 2, -0.5f);
                HumanoidUse[2] = false;
            }
            else if (animator.GetCurrentAnimatorStateInfo(2).normalizedTime > 1f && !animator.IsInTransition(2) && HumanoidUse[2] == false && layer == 2)
            {
                overrideController["Test RA"] = clip;
                animator.CrossFade("Test RA", 0.5f, 2, -0.5f);
                HumanoidUse[2] = true;
            }

        
    }
}
