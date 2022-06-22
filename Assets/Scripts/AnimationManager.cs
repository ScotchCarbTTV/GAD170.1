using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator animator;
    public bool fall { get; private set; }

    private void Awake()
    {
        TryGetComponent<Animator>(out animator);        
    }
    void Start()
    {
        animator.SetBool("dead", false);
        ToggleFall(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {            
            StopJump();                       
        }        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Ground" && fall == true)
        {
            StopJump();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ground" && fall == false)
        {
            JumpAnim();
        }
    }

    public void JumpAnim()
    {            
        ToggleFall(true);
    }

    //player flinching when getting hit
    public void Ouchie()
    {
        //set the animator state to 'flinching'
        animator.SetBool("flinching", true);        

        //toggle falling to true
        if (!fall)
        {
            ToggleFall(true);
        }
    }

    public void StopOuchie()
    {
        //set the animator state to 'not flinching'
        animator.SetBool("flinching", false);
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

    public void Death()
    {
        animator.SetBool("dead", true);
    }

}
