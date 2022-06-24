using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleChest : MonoBehaviour
{

    //The chest object must have a collider set to “isTrigger” in the inspector


    //reference to the player script to check the player lockPickSkill.
    //PlayerControllerThatLevelsUp must have a int variable called ”currentLockPickSkill”

    [SerializeField] private PlayerControllerThatLevelsUp playerControllerThatLevelsUp;

    private int DiceRollResult = 0;
    public int ChestDifficulty = 6;

    private bool inRange;

    //variable referencing the canvas displaying the UI for the chest.
    [SerializeField] private ChestUI chestUI;

    //the coin that will be spawned when the chest is opened, setting a prefab for instantiation later
    public Transform Coin;
    private void Awake()
    {
        inRange = false;
        chestUI = GetComponentInChildren<ChestUI>();
    }

    private void Start()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent<PlayerControllerThatLevelsUp>(out playerControllerThatLevelsUp);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (inRange == true)
            {
                //PlayerControllerThatLevelsUp must have a int variable called ”currentLockPickSkill”
                CheckIfLockPickIsSuccessful();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (other.TryGetComponent<PlayerControllerThatLevelsUp>(out playerControllerThatLevelsUp))
        {
            inRange = true;
            chestUI.ToggleText(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerControllerThatLevelsUp>(out playerControllerThatLevelsUp))
        {
            inRange = false;
            chestUI.ToggleText(false);
        }
    }


    //PlayerControllerThatLevelsUp must have a int variable called ”currentLockPickSkill”
    private void CheckIfLockPickIsSuccessful()
    {
        DiceRollResult = Random.Range(0, 6);

        if (DiceRollResult + playerControllerThatLevelsUp.currentLockPickSkill >= ChestDifficulty)
        {
            StartCoroutine(SuccessfulCheck());            
        }
        else if (playerControllerThatLevelsUp.currentLockPickSkill + 5 < ChestDifficulty)
        {
            chestUI.LevelTooLow(ChestDifficulty, playerControllerThatLevelsUp.currentLockPickSkill);
        }
        else
        {
            chestUI.FailedToOpen(ChestDifficulty, DiceRollResult);
        }
    }

    IEnumerator SuccessfulCheck()
    {
        //call the method on the canvas which will dispaly a UI message to the player
        chestUI.Success();
        //wait a moment
        yield return new WaitForSeconds(1.5f);
        //instantiate the coin then destroy the chest
        Instantiate(Coin, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation);
        Destroy(gameObject);
    }

}
