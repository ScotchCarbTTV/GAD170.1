using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject jumpHint;
    [SerializeField] GameObject moveHint;
    [SerializeField] GameObject camHint;
    [SerializeField] GameObject interactHint;

    private float hintTime = 10;

        private void Start()
    {
        jumpHint.SetActive(true);
        moveHint.SetActive(true);
        camHint.SetActive(true);
        interactHint.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Vertical") || Input.GetButtonDown("Horizontal"))
        {
            moveHint.SetActive(false);
        }
        
        if (Input.GetButtonDown("Jump"))
        {
            jumpHint.SetActive(false);
        }
        if (Input.GetButtonDown("Interact"))
        {
            interactHint.SetActive(false);
        }
        if(Input.GetButtonDown("HorizontalLook") || Input.GetButtonDown("VerticalLook"))
        {
            camHint.SetActive(false);
        }

        hintTime -= 1 * Time.deltaTime;
        if(hintTime <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
