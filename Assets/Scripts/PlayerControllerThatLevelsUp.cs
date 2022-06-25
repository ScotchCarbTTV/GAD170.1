using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;

/*
 
       Player Controller that moves and turns faster as it gains XP;
        Manages moving and rotating the player via the arrow keys.
        Up/down arrow keys to move forward and backward.
        Left/right arrow keys to rotate left and right.
*/

/*
   Modified by Ian Bell 23/6/22
 */

public class PlayerControllerThatLevelsUp : MonoBehaviour
{
    //references to the camera, cinemachine brain and cinemachine freelook component
    [SerializeField] private Transform cam;
    [SerializeField] private CinemachineBrain brain;
    [SerializeField] CinemachineFreeLook cinemachine;

    //variables referencing the audio source and audio prefab for footstep and jump
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip footClip;
    [SerializeField] AudioClip jumpClip;
    [SerializeField] AudioClip oofClip;
    [SerializeField] AudioClip crunchClip;
    

    //variable referencing the StatManagerUI script to update the UI
    [SerializeField] StatManagerUI statManager;

    //references to the skinned mesh rendered (to 'turn off' the player model when they die and are replaced by the ragdoll)
    [SerializeField] private SkinnedMeshRenderer body;
    [SerializeField] private SkinnedMeshRenderer eyes;

    //variable for the XPGainUI manager
    [SerializeField] private XPGainUI xpGainUI;

    //reference to the ragdoll prefab of the player model
    [SerializeField] private GameObject ragDoll;

    //reference to the death screen overlay
    [SerializeField] private GameObject deathScreen;

    //variable for changing parameters on the rigidbody
    private Rigidbody rbody;

    //variable for adjusting the rate at which the character falls
    public float gravityModifier = 2.5f;
    public float lowJumpMultipier = 2f;

    //The base move speed of the character and turn speed of the camera
    public float moveSpeed = 1f;
    public float turnSpeed = 45f;
    public float jumpHeight = 2f;

    //varaibes for handling the character turning relative to the direction the camera is pointing
    public float turnSmoothTime = 1f;
    private float turnSmoothVelocity;

    //The base lock picking skill which will determine how likely the player is to open a chest
    public int lockPickSkill;

    //The move speed, turn speed and jump height with the buffs you have from leveling up.   
    public float currentMoveSpeed;
    public float currentTurnSpeed;
    public float currentJumpHeight;

    //the lockpicking skill with the buff from levelling up
    public int currentLockPickSkill;


    public float xp = 0;    // Amount of XP the player has
    public float xpForNextLevel = 10;   //Xp needed to level up, the higher the level, the harder it gets. 
    public int level = 0;   // Level of the player

    //position which the player will be teleported to when respawning
    private Vector3 spawn;

    //reference to the animation manager script
    private AnimationManager animManager;

    //variables for handling the movement of the player
    private Vector3 motion;
    private Vector2 input;

    private void Awake()
    {
        //making sure the meshrenderers of the character model are enabled & the death screen is disabled
        body.enabled = true;
        eyes.enabled = true;
        deathScreen.SetActive(false);

        //set the cam variable
        if (cam == null)
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }
    }

    private void Start()
    {
        //set the spawn point of the player to wherever the character model is placed in the Unity inspector
        SetRespawn();

        //initialize all the current stats of the player based on the base stats
        SetXpForNextLevel();
        SetCurrentMoveSpeed();
        SetCurrentTurnSpeed();
        SetCurrentJumpHeight();
        SetCurrentLockPickSkill();
        statManager.UpdateTexts(level, currentLockPickSkill, currentMoveSpeed, currentTurnSpeed, currentJumpHeight, xp, xpForNextLevel);

        //set the animation manager and cinemachine brain        
        if (!TryGetComponent<AnimationManager>(out animManager))
        {
            Debug.LogError("You need an AnimationManager component on the game object.");
        }

        if (!cam.TryGetComponent<CinemachineBrain>(out brain))
        {
            Debug.LogError("You need a Cinemachine brain component on the main camera!");
        }
        //set the rigidbody
        if (!TryGetComponent<Rigidbody>(out rbody))
        {
            Debug.LogError("You need a Rigidbody component on this game object!");
        }

    }

    //methods for levelling and updating stats
    #region
    // To level up you need to collect an amount of xp;
    // This starts at 10 xp
    // Each level you gain the xp required gets higher exponentially
    // The exponential growth is slowed by scaling it by 10%

    void SetXpForNextLevel()
    {
        xpForNextLevel = (10f + (level * level * 0.1f));
        //Debug.Log("xpForNextLevel " + xpForNextLevel);
    }

    // For each level, the player adds 10% to the move speed 
    void SetCurrentMoveSpeed()
    {
        currentMoveSpeed = this.moveSpeed + (this.moveSpeed * 0.1f * level);
        //Debug.Log("currentMoveSpeed = " + currentMoveSpeed);
    }

    //For each level, the player will gain an additional 10% jump height. Very cool!
    void SetCurrentJumpHeight()
    {
        currentJumpHeight = this.jumpHeight + (this.jumpHeight * 0.1f * level);
        lowJumpMultipier = currentJumpHeight / 2;
        //Debug.Log("currentJumpHeigh = " + currentJumpHeight);
    }

    // For each level, the player adds 10% to the turn speed 
    void SetCurrentTurnSpeed()
    {
        currentTurnSpeed = this.turnSpeed + (this.turnSpeed * (level * 0.1f));
        cinemachine.m_XAxis.m_MaxSpeed = currentTurnSpeed;
        //Debug.Log("currentTurnSpeed = " + currentTurnSpeed);
    }

    //For each level the player adds their current level to their initial lockPickSkill
    void SetCurrentLockPickSkill()
    {
        currentLockPickSkill = lockPickSkill + level;
    }

    void LevelUp()
    {
        xp = 0f;
        level++;
        //Debug.Log("level" + level);
        SetXpForNextLevel();
        SetCurrentMoveSpeed();
        SetCurrentTurnSpeed();
        SetCurrentJumpHeight();
        //call the SetCurrentLockPickSkill method
        SetCurrentLockPickSkill();

        //update the UI with all the information
    }

    //a function to make the player gain the amount of Xp the you tell it. 
    public void GainXP(int xpToGain)
    {
        xp += xpToGain;
        statManager.UpdateTexts(level, currentLockPickSkill, currentMoveSpeed, currentTurnSpeed, currentJumpHeight, xp, xpForNextLevel);

        //Debug.Log("Gained " + xpToGain + " XP, Current Xp = " + xp + ", XP needed to reach next Level = " + xpForNextLevel);
    }
    #endregion


    void Update()
    {

        //Test the GainXp function by pressing the x button. 
        //if (Input.GetKeyDown(KeyCode.X) == true) { GainXP(10); }

        //levelling up code
        #region
        //LevelUp when the appropriate conditions are met.
        if (xp >= xpForNextLevel)
        {
            LevelUp();
            xpGainUI.LevelUp();
            statManager.UpdateTexts(level, currentLockPickSkill, currentMoveSpeed, currentTurnSpeed, currentJumpHeight, xp, xpForNextLevel);

        }
        #endregion


        //movement: jumping, running, turning and falling code.
        #region
        // Check spacebar to trigger jumping. Checks if vertical velocity (eg velocity.y) is near to zero.        
        if (Input.GetButtonDown("Jump") == true && !animManager.fall)
        {
            Jump(1, 1);
        }


        //checks if the player is current descending and increases their fall rate by the gravityModifier
        if (rbody.velocity.y < 0 && animManager.fall == true)
        {
            rbody.velocity += Vector3.up * Physics.gravity.y * (gravityModifier - 1) * Time.deltaTime;
        }
        //checks if tthe player is still holding down jump button; if not, the effect of gravity is increased so they perform a smaller 'hop'
        else if (Mathf.Abs(rbody.velocity.y) > 0.01f && !Input.GetButton("Jump"))
        {
            rbody.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultipier - 1) * Time.deltaTime;
        }

        // Rotation and movement speed is modifed by the level (currentMoveSpeed) of the player and by the time between update frames (Time.deltaTime). 


        // Move player via horizontal / vertical inputs             
        //take the axis of the inputs and assign them to part of 'input' vector 2
        input.x = Input.GetAxis("Vertical");
        input.y = Input.GetAxis("Horizontal");
        motion = new Vector3(input.y, 0, input.x).normalized; //combine the inputs into a single vector 3

        //check if any inputs are active
        if (motion.magnitude >= 0.1)
        {
            //turn the character towards the directions dicated by the inputs (the combined average of the two directions) modified by the current direction of the third person camera
            float targetAngle = Mathf.Atan2(motion.x, motion.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); //smooth the rotation angle for the player to face in the direction they are moving
            transform.rotation = Quaternion.Euler(0f, angle, 0f); //rotate the character to face the direction they are moving relative to the direction of the camera
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; // calculate the direction to move the character

            //update the character's position based on the inputs received modified by the current movement speed
            transform.position += new Vector3(moveDir.x, 0, moveDir.z) * currentMoveSpeed * Time.deltaTime;

            //update the rigidbody's velocity according to the inputs received

            //check if the player is currently falling; if not, switch the animation from idle to running
            if (animManager.fall == false)
            {
                animManager.ToggleRun(true);
                source.clip = footClip;
                if (source.isPlaying == false)
                {
                    source.Play();
                }
            }

        }
        //if there is no input happening switch the animation to idle
        else
        {
            animManager.ToggleRun(false);
            source.clip = null;
        }
        #endregion
    }


    //self contained method for jumping which other scripts can call with applied modifiers
    public void Jump(float jumpMod, int jumpType)
    {
        rbody.velocity += Vector3.up * this.currentJumpHeight * jumpMod;
        if (jumpType == 1)
        {
            animManager.JumpAnim();
            source.clip = null;
            source.PlayOneShot(jumpClip);
        }
        else if (jumpType == 2)
        {
            animManager.Ouchie();
            source.clip = null;
            source.PlayOneShot(oofClip);
        }
    }

    //method to update the player's respawn position to their current transform position
    public void SetRespawn()
    {
        spawn = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
    }

    //method for handling different death 'types' 
    public void Death(int deathType)
    {
        //deathType 1 = falling with lives left
        //deathType 2 = falling no lives left
        //deathType 3 = died to 


        if (deathType == 1)
        {
            StartCoroutine(FallRespawn());
        }
        else if (deathType == 2)
        {
            StartCoroutine(FallDeath());
        }
        else
        {
            StartCoroutine(DamageDeath());
        }
    }

    //method for instantiating the ragdoll when the player dies
    public void RagDoll()
    {
        //disable the meshrenderers on the player character
        body.enabled = false;
        eyes.enabled = false;
        //spawn in the ragdoll prefab on the player's current position
        Instantiate(ragDoll, transform.position, transform.rotation);
    }

    //enumerator method for respawning the player when they fall, including stopping the camera from chasing them down and activating a 'death screen' overlay
    IEnumerator FallRespawn()
    {
        //decouple the camera from the player
        brain.enabled = false;

        //show the 'YOU DIED' ui element

        //wait a few seconds
        yield return new WaitForSeconds(3);

        //clear 'YOU DIED' ui element

        //move the player 
        rbody.velocity = Vector3.zero; //prevents the player clipping through the ground when they respawn
        this.transform.position = spawn;


        //recouple the camera to the player
        brain.enabled = true;
    }

    //enumerator for when the player falls to their death without any health/lives remaining
    IEnumerator FallDeath()
    {
        //disable the camera's cinemachine brain so it doesn't follow them through the fog
        brain.enabled = false;

        //show the 'YOU DIED' ui element
        deathScreen.SetActive(true);

        //wait a few seconds
        yield return new WaitForSeconds(3);

        //clear 'YOU DIED' ui element

        //recouple the camera to the player
        brain.enabled = true;

        //reload the entire scene
        SceneManager.LoadScene("GameScene");
    }

    //enumerator for when the player dies due to damage from enemies or traps
    IEnumerator DamageDeath()
    {
        //disable the camera brain so it doesn't follow the player (if they end up falling off the edge)
        brain.enabled = false;

        //spawn the ragdoll        
        RagDoll();

        //show the 'YOU DIED' ui element
        deathScreen.SetActive(true);

        //wait a few seconds
        yield return new WaitForSeconds(3);

        //clear 'YOU DIED' ui element

        //recouple the camera to the player
        brain.enabled = true;

        //reload the game scene to start over
        SceneManager.LoadScene("GameScene");
    }

    //ontriggerenter method for performing a 'goomba stomp' on the enemies.
    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy;
        //execute the 'death' method on the enemy.
        if (other.TryGetComponent<Enemy>(out enemy))
        {
            if (animManager.animator.GetBool("flinching") == false)
            {
                //call the death function on the enemy being stomped
                enemy.Death();
                source.clip = null;
                source.PlayOneShot(crunchClip);
                Jump(1.5f, 1);
            }

        }
    }

}

