using UnityEngine;
using System.Collections;

public class EnemyControllerScript : MonoBehaviour
{
    // Public unity getter
    [Range(0, 7)]
    public float runPower = 0.5f;
    public GameObject enemyMesh;
    
    // Enemy variables
    CleanSceneScript cleanSceneScript;
    CharacterController characterController;
    Vector3 startPosition;
    float gravity, maxAttackTime, attackStartTime;
    bool attack, attackAnimation;


    void Awake()
    {
        #region Check for CharacterController and set to variable
        if (GetComponent<CharacterController>())
            characterController = GetComponent<CharacterController>();
        else
            Debug.LogError("PlayerControllerScript needs CharacterController in Playerobject");
        #endregion

        // Save startposition
        startPosition = characterController.transform.position;
    }

    void Start()
    {
        #region Check for CameraObject, ClearScene and set to variable
        // Check for ClearSceneScipt and set to variable
        if (GameObject.FindGameObjectWithTag("LevelObject").GetComponent<CleanSceneScript>())
            cleanSceneScript = GameObject.FindGameObjectWithTag("LevelObject").GetComponent<CleanSceneScript>();
        else
            Debug.LogError("EnemyControllerScript needs CleanSceneScript in LevelObject");

        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        if (characterController.collisionFlags == CollisionFlags.Sides) //|| 
            //characterController.collisionFlags != CollisionFlags.Below)
            runPower = -runPower;

        #region Gravity
        // Touch Ground or ceiling
        if (characterController.isGrounded)
            gravity = -10f;
        // Fall down
        else
            gravity -= 15 * Time.deltaTime;
        #endregion

        // Move player by moving character controller
        characterController.Move(new Vector3(-runPower, gravity, 0) * Time.deltaTime);

        // Set enemy to attack and set deactiv after time
        if(!attackAnimation)
            enemyMesh.animation.Play("Run");
        else
            enemyMesh.animation.Play("Attack");

        if (attack && Time.time >= attackStartTime + maxAttackTime)
        {
            attack = false;

            // Delet enemy from list for showing in front of character
            cleanSceneScript.DeletEnemyFromList(gameObject);
            // Delet missle and set activ of destroyable object to false
            gameObject.SetActive(false);
        }
    }

    public void SetEnemyToStartPosition()
    {
        characterController.transform.position = startPosition;
        runPower = Mathf.Abs(runPower);
        attackAnimation = false;
        attack = false;
    }

    public void SetEnemyToAttack(float maxAttackTime)
    {
        this.maxAttackTime = maxAttackTime;
        attackStartTime = Time.time;
        attack = true;
        attackAnimation = true;
    }

    public void StopAttackEnemy()
    {
        attack = false;
        attackAnimation = false;
    }
}
