using UnityEngine;
using System.Collections;

// Public enums
public enum MovementPhase
{
    Jump,
    Slide,
    Shoot
}
public enum AnimationPhase
{
    Run,
    Jump,
    Idle,
    Slide,
    Attack,
    RushDown,
    Death
}
public enum SoundPhase
{
    Run,
    Slide,
    Jump,
    Land,
    RushDown,
    Attack,
    Shoot,
    Hitted,
    Collect
}

public class PlayerControllerScript : MonoBehaviour
{
    #region Variables
    // Public unity setter
    public float runPower = 10, jumpPower = 10, gravity = 30, superJumpPower = 15,
                 followPlayerHigh = 1, rushPower = 5, misslePower = 2000, regenarationPower = 1;
    [Range(1, 100)]
    public int maxMissleCount = 3;
    [Range(0, 1)]
    public float startRushTime = 0.2f, maxRushTime = 0.5f, maxSlideTime = 0.5f, maxAttackTime = 0.2f,
                 maxTouchBufferTime = 0.3f, maxGroundedBufferTime = 0.1f;
    public GameObject characterMesh;
    public AudioClip runSound, jumpSound, rushDownSound, landSound, startSlideSound, slideSound, closeCombatSound, throwSound,
                     hittedSound, collectSound;
    public Rigidbody missle;
    public Transform missleStartPosition;

    // Game variables
    bool play;
    public bool Play
    {
        private get { return play; }
        set
        {
            play = value;

            if (value)
            {
                // Set permanent movement
                movement.x = runPower;
                // Set RunPower to MainCamera
                cameraScript.MovementSpeed = movement.x;
            }
            else
            {
                // Set permanent movement
                movement.x = 0;
                // Set RunPower to MainCamera
                cameraScript.MovementSpeed = movement.x;
            }
        }
    }

    // CharacterController variables
    CharacterController characterController;
    public bool PlayerTopTrigger { private get; set; }
    Vector3 movement = Vector3.zero;
    Vector3 startPosition, lastCheckpointPosition;

    // Missle variables
    int currentMissleCount;

    // Camera variables
    CameraScript cameraScript;

    // Level variables
    CleanSceneScript cleanSceneScript;
    LevelGeneratorScript levelGeneratorScript;

    // Enemy variabels
    GameObject currentEnemy;

    public GameObject CurrentEnemy { get; set; }

    // Sound variables
    SoundPhase currentSoundPhase;
    AudioSource movementSounds, playerActiveSounds, playerPassivSounds;

    // Movement variables
    bool jump, isGrounded, notGrounded, slide, standUp, standUpAfterObject, attack, rush, rushDone, regenerate;
    float jumpBufferTime, groundedBufferTime, slideBufferTime, slideStartTime, attackStartTime, rushStartTime;
    #endregion

    void Awake()
    {
        #region Check for CharacterController and set to variable
        if (GetComponent<CharacterController>())
            characterController = GetComponent<CharacterController>();
        else
            Debug.LogError("PlayerControllerScript needs CharacterController in Playerobject");
        #endregion

        #region Check for CameraObject and set to variable
        // Check for CameraScipt and set to variable
        if (GameObject.FindGameObjectWithTag("CameraObject").GetComponent<CameraScript>())
            cameraScript = GameObject.FindGameObjectWithTag("CameraObject").GetComponent<CameraScript>();
        else
            Debug.LogError("PlayerControllerScript needs CameraScript in CameraObject");
        #endregion

        #region Check for CleanSceneScript and set to variable
        if (GameObject.FindGameObjectWithTag("LevelObject"))
        {
            // Check for ClearSceneScipt and set to variable
            if (GameObject.FindGameObjectWithTag("LevelObject").GetComponent<CleanSceneScript>())
                cleanSceneScript = GameObject.FindGameObjectWithTag("LevelObject").GetComponent<CleanSceneScript>();
            else
                Debug.LogError("PlayerControllerScript needs CleanSceneScript in CameraObject");
            // Check for LevelGeneratorScipt and set to variable
            if (GameObject.FindGameObjectWithTag("LevelObject").GetComponent<LevelGeneratorScript>())
                levelGeneratorScript = GameObject.FindGameObjectWithTag("LevelObject").GetComponent<LevelGeneratorScript>();
            else
                Debug.LogError("PlayerControllerScript needs LevelGeneratorScript in LevelObject");
        }
        #endregion

        #region Set sound
        movementSounds = gameObject.AddComponent<AudioSource>();
        playerActiveSounds = gameObject.AddComponent<AudioSource>();
        playerPassivSounds = gameObject.AddComponent<AudioSource>();
        #endregion
    }

    // Use this for initialization
    void Start()
    {
        // Set startposition
        startPosition = transform.position;
        SetToStartPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (Play)
        {
            #region Set player position to other scripts
            // Set player position to cleanScreneScript to delet enemies behinde player
            cleanSceneScript.PlayerPositionX = characterController.transform.position.x;

            // Set player position du levelgeneratorScript to generate level
            levelGeneratorScript.PlayerPositionX = characterController.transform.position.x;

            if (!isGrounded && !jump && transform.position.y < cameraScript.CameraPositionY)
                cameraScript.FollowPlayer(transform.position.y);
            else if ((isGrounded || jump) && Mathf.Abs(transform.position.y - cameraScript.CameraPositionY) >= 2)
                cameraScript.FollowPlayer(transform.position.y);
            #endregion

            #region Gravity
            // Touch Ground or ceiling
            if (characterController.isGrounded)
                movement.y = -10f;
            else if (characterController.collisionFlags == CollisionFlags.Above)
                movement.y = -3f;
            // Fall down
            else
                movement.y -= gravity * Time.deltaTime;
            #endregion

            #region Character Movement
            // Set all movements
            DoMovementPhase();
            // Move player by moving character controller
            characterController.Move(movement * Time.deltaTime);
            // Correct z position
            if (transform.position.z != 0)
                characterController.Move(Vector3.forward * -transform.position.z);
            #endregion

            // Fall down debugger
            if (characterController.transform.position.y <= -10
                || characterController.transform.position.y <= cameraScript.CameraPositionY - 10)
                SetToStartPosition();
        }
        else
        {
            // Set animation
            SetAnimationPhase(AnimationPhase.Idle);
            // Set sound
            SetSoundPhase(SoundPhase.Run);
        }
    }

    void OnTriggerEnter(Collider colliderObject)
    {
        switch (colliderObject.tag)
        {
            #region Get coins, ect. trigger
            case "GetCoinTrigger":
                GetCoin(colliderObject);
                break;
            case "EnemyObject":
                currentEnemy = colliderObject.gameObject;
                attack = true;
                attackStartTime = Time.time;
                currentEnemy.GetComponent<EnemyControllerScript>().SetEnemyToAttack(maxAttackTime);
                break;
            case "MissleObject":
                GetMissle(colliderObject);
                break;
            #endregion
            #region Set to position trigger
            case "CameraEndTrigger":
                //SetToLastCheckpointPosition();
                SetToStartPosition();
                break;
            case "LevelEndTrigger":
                //SetToStartPosition();
                Application.LoadLevel("MainMenu");
                /* TODO 
                 * LOAD NEXT LEVEL
                 */
                break;
            case "FallDownTrigger":
                //SetToLastCheckpointPosition();
                SetToStartPosition();
                break;
            case "CheckpointTrigger":
                lastCheckpointPosition = transform.position;
                break;
            #endregion
        }
    }

    #region Set to position methodes
    // Set Player object and camera object to last checkpoint position
    void SetToLastCheckpointPosition()
    {
        // Set positions
        transform.position = lastCheckpointPosition;
        cameraScript.CameraToPosition(lastCheckpointPosition);

        // Call reset methode
        ResetAllMovements();
    }
    // Set Player object and camera object to start position
    void SetToStartPosition()
    {
        // Set last checkpoint to start position
        lastCheckpointPosition = startPosition;

        // Set positions
        transform.position = startPosition;
        cameraScript.CameraToPosition(startPosition);

        // Reset player position x in clean scene script
        cleanSceneScript.PlayerPositionX = characterController.transform.position.x;

        // Call reset methodes
        ResetAllMovements();
        cleanSceneScript.ResetCompleteScene();
    }
    public void SetToPositionXZero()
    {
        // Set positions
        cameraScript.CameraToPosition(new Vector3(cameraScript.CameraPositionX + cameraScript.xOffsetToPlayer - transform.position.x, 0, 0));
        transform.position = new Vector3(0, transform.position.y, 0);
    }
    #endregion

    #region GetOnTriggerMethodes
    public void GetCoin(Collider colliderObject)
    {
        // Add points
        int points = PlayerPrefs.GetInt("Points") + 100;
        PlayerPrefs.SetInt("Points", points);
        // Set coin object deactive
        colliderObject.gameObject.SetActive(false);
        // Set sound
        SetSoundPhase(SoundPhase.Collect);
    }
    public void GetMissle(Collider colliderObject)
    {
        Destroy(colliderObject.gameObject);
        // Add missle
        SetCurrentMissleCount(1);
        SetSoundPhase(SoundPhase.Collect);
    }

    void SetCurrentMissleCount(int changeCount)
    {
        // Remove missle
        if (changeCount < 0 && currentMissleCount > 0)
            currentMissleCount--;
        // Add missle
        else if (changeCount > 0 && currentMissleCount < maxMissleCount)
            currentMissleCount++;
    }
    #endregion

    void ResetAllMovements()
    {
        // Stand up if sliding
        if (standUp)
            StandUp();

        // Set all movements to false
        jump = false; attack = false; rush = false; rushDone = false; regenerate = false;
        // Set all times to null
        jumpBufferTime = 0; slideBufferTime = 0; slideStartTime = 0; rushStartTime = 0;

        // Set movespeed y to default
        movement.y = 0;

        // Reset current missle count
        currentMissleCount = maxMissleCount;

        // Set points to 0
        PlayerPrefs.SetInt("Points", 0);

        // Stop game
        Play = false;
    }

    void DoMovementPhase()
    {
        #region Run
        if (!standUp && isGrounded && !attack)
        {
            // Set animation
            SetAnimationPhase(AnimationPhase.Run);

            // Set sound
            SetSoundPhase(SoundPhase.Run);
        }
        #endregion

        #region Attack
        if (attack && Time.time <= attackStartTime + maxAttackTime
            && !characterMesh.animation.IsPlaying("Jump"))
        {
            // Set animation
            SetAnimationPhase(AnimationPhase.Attack);

            // Set sound
            SetSoundPhase(SoundPhase.Attack);
        }
        else
            attack = false;
        #endregion

        #region Grounded
        // Check character is grounded or not
        if (characterController.isGrounded)
        {
            if (!isGrounded)
                SetSoundPhase(SoundPhase.Land);
            isGrounded = true;
            notGrounded = false;
        }
        // Set false after buffer time
        else if (!notGrounded && !characterController.isGrounded)
        {
            groundedBufferTime = Time.time;
            notGrounded = true;
        }
        else if (notGrounded && Time.time >= groundedBufferTime + maxGroundedBufferTime)
        {
            isGrounded = false;
            notGrounded = false;
        }
        #endregion

        #region Regeneration
        if (!regenerate && transform.position.x < cameraScript.CameraPositionX + cameraScript.XOffsetToPlayer)
        {
            movement.x += regenarationPower;
            regenerate = true;
        }
        else
        {
            movement.x = runPower;
            regenerate = false;
        }
        #endregion

        #region Slide
        if (slide)
            if (!standUp && Time.time <= maxTouchBufferTime + slideBufferTime)
            {
                // Set new standup time
                slideStartTime = Time.time;

                // Fast down to ground
                if (!isGrounded)
                {
                    movement.y = -jumpPower;
                    // Set animation
                    SetAnimationPhase(AnimationPhase.RushDown);
                    // set sound
                    SetSoundPhase(SoundPhase.RushDown);
                    // Stop rush
                    if (rush)
                        rush = false;
                    else if (rushDone)
                        rushStartTime -= maxRushTime;

                }
                else
                {
                    // Set animation
                    SetAnimationPhase(AnimationPhase.Slide);
                }

                // Set collisionbox
                characterController.height /= 2;
                characterController.center -= new Vector3(0, characterController.center.y / 2, 0);

                // Stop waiting
                slide = false;
                // Start waiting
                standUp = true;
            }
            // End cower after buffertime ends
            else if (Time.time >= slideBufferTime + maxTouchBufferTime)
                // Stop waiting
                slide = false;
        #endregion

        #region Stand up
        if (standUp)
        {
            // Stand up after time
            if (!PlayerTopTrigger && Time.time >= slideStartTime + maxSlideTime)
            {
                // Set collisionbox
                StandUp();
                standUpAfterObject = false;
            }
            // Allow stand up after object 
            else if (!standUpAfterObject && PlayerTopTrigger)
            {
                // Set under object
                standUpAfterObject = true;
            }
            // Stand up after object
            else if (standUpAfterObject && !PlayerTopTrigger)
            {
                // Set collisionbox
                StandUp();
                standUpAfterObject = false;
                slide = false;
            }

            SetSoundPhase(SoundPhase.Slide);
        }
        #endregion

        #region Jump
        if (jump)
            if (Time.time <= maxTouchBufferTime + jumpBufferTime
                && isGrounded && !PlayerTopTrigger)
            {
                // High jump
                if (standUp)
                {
                    // Set collisionbox
                    StandUp();

                    // Set movement
                    movement.y = superJumpPower;
                }
                // Normal jump
                else
                    // Set movement
                    movement.y = jumpPower;

                // Set animation
                SetAnimationPhase(AnimationPhase.Jump);

                // Set sound
                SetSoundPhase(SoundPhase.Jump);

                // Stop waiting
                jump = false;
                // Start waiting
                isGrounded = false;

                if (currentEnemy)
                    currentEnemy.GetComponent<EnemyControllerScript>().StopAttackEnemy();
            }
            // End jump after buffertime ends
            else if (Time.time >= jumpBufferTime + maxTouchBufferTime)
                // Stop waiting
                jump = false;
        #endregion
    }
    void StandUp()
    {
        // Set collisionbox
        characterController.center += new Vector3(0, characterController.center.y, 0);
        characterController.height *= 2;

        // Stop waiting
        standUp = false;
    }

    void SetAnimationPhase(AnimationPhase animationPhase)
    {
        switch (animationPhase)
        {
            case AnimationPhase.Run:
                characterMesh.animation.Play("Run");
                break;
            case AnimationPhase.Jump:
                characterMesh.animation.Play("Jump");
                break;
            case AnimationPhase.Slide:
                characterMesh.animation.Play("Slide");
                break;
            case AnimationPhase.Attack:
                if (!standUp
                    && !characterMesh.animation.IsPlaying("RushDown"))
                    characterMesh.animation.Play("Attack");
                break;
            case AnimationPhase.RushDown:
                characterMesh.animation.Play("RushDown");
                break;
            case AnimationPhase.Death:
                characterMesh.animation.Play("Death");
                break;
            default:
                characterMesh.animation.Play("Run");
                break;
        }
    }

    void SetSoundPhase(SoundPhase soundPhase)
    {
        if (PlayerPrefs.GetInt("Sound_Buttons") == 0)
        {
            // Check sound phase
            switch (soundPhase)
            {
                // Movement sounds
                case SoundPhase.Run:
                    if (currentSoundPhase == SoundPhase.Slide || !movementSounds.isPlaying)
                    {
                        movementSounds.clip = runSound;
                        movementSounds.Play();
                    }
                    break;
                case SoundPhase.Jump:
                    movementSounds.clip = jumpSound;
                    break;
                case SoundPhase.RushDown:
                    movementSounds.clip = rushDownSound;
                    break;
                case SoundPhase.Land:
                    if (currentSoundPhase != SoundPhase.Run)
                        movementSounds.clip = landSound;
                    break;
                case SoundPhase.Slide:
                    if (currentSoundPhase == SoundPhase.Run || !movementSounds.isPlaying)
                    {
                        movementSounds.clip = slideSound;
                        movementSounds.Play();
                    }
                    break;
                // Player active sounds
                case SoundPhase.Attack:
                    playerActiveSounds.clip = closeCombatSound;
                    playerActiveSounds.Play();
                    break;
                case SoundPhase.Shoot:
                    playerActiveSounds.clip = throwSound;
                    playerActiveSounds.Play();
                    break;
            }

            if (soundPhase != SoundPhase.Run && soundPhase != SoundPhase.Slide)
                movementSounds.Play();

            // Player passiv sounds
            switch (soundPhase)
            {
                case SoundPhase.Collect:
                    playerPassivSounds.clip = collectSound;
                    playerPassivSounds.Play();
                    break;
                case SoundPhase.Hitted:
                    playerPassivSounds.clip = hittedSound;
                    playerPassivSounds.Play();
                    break;
            }

            // Save current sound phase
            currentSoundPhase = soundPhase;
        }
    }

    // Methode called by InputLogigScript to set movemten in dependence to input
    public void SetMovementPhase(MovementPhase movementPhase)
    {
        switch (movementPhase)
        {
            case MovementPhase.Jump:
                // Set new buffer time and start waiting to true
                jumpBufferTime = Time.time;
                jump = true;
                break;
            case MovementPhase.Slide:
                // Set new buffer time and start waiting to true
                slideBufferTime = Time.time;
                slide = true;
                break;
            case MovementPhase.Shoot:
                if (Play && currentMissleCount > 0)
                {
                    // Remove missle
                    SetCurrentMissleCount(-1);

                    // Create new missle at missle start position and shoot
                    Rigidbody missleInstance;
                    missleInstance = Instantiate(missle, missleStartPosition.position, Quaternion.Euler(0, 0, 0)) as Rigidbody;
                    missleInstance.AddForce(missleStartPosition.right * misslePower);
                    // Set animation
                    SetAnimationPhase(AnimationPhase.Attack);
                    // Set sound
                    SetSoundPhase(SoundPhase.Shoot);
                }
                break;
        }
    }
}
