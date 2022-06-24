using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignCanvas : MonoBehaviour

{
    void Update()
    {
        //rotate the canvas to always face the main camera
        transform.forward = Camera.main.transform.forward;
    }
}
