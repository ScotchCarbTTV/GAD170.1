using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Script: MovingPlatform
    Author: Gareth Lockett
    Version: 1.0
    Description:    Simple script to be applied to a platform object. Moves the platform back and forth along a selected axis using a sine wave.
                    Make sure to set platform up with a collider set to isTrigger = true (eg there should be a second, non-trigger collider for the character to stand on)
                    When an object, with a rigidbody, enters the collider it will be parented to this object (eg moves with it)
                    When the object exits the collider it will be unparented.
*/

public class MovingPlatform : MonoBehaviour
{
    //variables containing the two points to move between
    [SerializeField] private GameObject pointA;
    [SerializeField] private GameObject pointB;

    //variable for the current target point
    private GameObject targetPoint;

    private Animator animator;

    private Vector3 direction = new Vector3();
    
    //public enum Axis { X_AXIS, Y_AXIS, Z_AXIS }

    //public Axis axis;
    //public float moveDistance;

    public float moveSpeed;

    private void Start()
    {
        targetPoint = pointA;
    }

    void Update()
    {
        #region
        /*
        Vector3 moveDirection = Vector3.zero;
        switch (this.axis)
        {
            case Axis.X_AXIS:
                moveDirection = this.transform.right;
                break;

            case Axis.Y_AXIS:
                moveDirection = this.transform.up;
                break;

            case Axis.Z_AXIS:
                moveDirection = this.transform.forward;
                break;
        }

        this.transform.position += moveDirection * Time.deltaTime * this.moveDistance * Mathf.Sin(Time.time * this.moveSpeed);
        */
        #endregion

        //check if the platform is within the stopping distance of the target position
        if(Vector3.Distance(transform.position, targetPoint.transform.position) < 0.5f)
        {
            //change the target position to the currently non-active target position
            if(targetPoint == pointA)
            {
                targetPoint = pointB;
            }
            else
            {
                targetPoint = pointA;
            }
        }

        //calculate direction to the targetPoint and normalize it
        direction = (targetPoint.transform.position - transform.position).normalized;

        //move the platform in the desired direction modified by the movespeed
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        //set the parent of the player to this object
        other.transform.parent = this.transform;
        
    }

    private void OnTriggerExit(Collider other)
    {
        //remove this object as the player object's parent
        other.transform.parent = null;
    }
}