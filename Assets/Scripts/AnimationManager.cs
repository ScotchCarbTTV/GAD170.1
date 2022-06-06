using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator animator;
    public bool fall { get; private set; }

    void Start()
    {
        TryGetComponent<Animator>(out animator);
        ToggleFall(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {            
            StopJump();
            animator.SetTrigger("hitGround");
            
        }
    }

    public void JumpAnim()
    {
        animator.SetTrigger("jump");
        animator.SetTrigger("hitGround");
        ToggleFall(true);
    }

    public void StopJump()
    {
        ToggleFall(false);
    }

    public void ToggleRun(bool run)
    {
        if (run == true)
        {
            animator.SetBool("isRunning", true);
           
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    public void ToggleFall(bool fall_)
    {
        if(fall_ == true)
        {
            animator.SetBool("isFalling", true);
            fall = true;
        }
        else
        {
            animator.SetBool("isFalling", false);
            fall = false;
        }
    }

}
