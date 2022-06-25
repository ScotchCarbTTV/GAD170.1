using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    //variable to reference the animator component on this 
    public Animator animator;

    //variable for setting if the player is grounded or not
    public bool fall { get; private set; }

    private void Awake()
    {
        //assign the animator variable
        TryGetComponent<Animator>(out animator);
    }
    void Start()
    {
        //set the 'dead' bool to false (can potentially set to true when player respawns)
        animator.SetBool("dead", false);
        //set falling to false
        ToggleFall(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        //when the player enters the trigger of any object tagged as ground switch falling to false and set the animation state back to idle
        if (other.gameObject.tag == "Ground")
        {
            StopJump();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //same as the on trigger enter, just makes sure that if the player has somehow entered the trigger of a ground object without switching to the grounded 'state' they'll do so on the next frame
        if (other.gameObject.tag == "Ground" && fall == true)
        {
            StopJump();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //switches the player to the jumping/falling state when they leave the trigger of a ground object
        if (other.gameObject.tag == "Ground" && fall == false)
        {
            JumpAnim();
        }
    }

    public void JumpAnim()
    {
        //set falling to true when jumping is triggered
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
        //switches falling to false
        ToggleFall(false);
    }

    public void ToggleRun(bool run)
    {
        //switches the player's running state in the animator on and off
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
        //switches the player's falling animation on and off in the animaor
        if (fall_ == true)
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
        //begins the player's death animation
        animator.SetBool("dead", true);
    }
}
