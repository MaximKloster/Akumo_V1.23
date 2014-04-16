using UnityEngine;
using System.Collections;

public class MissleControllerScript : MonoBehaviour
{
    // Missle variables
    [Range(0, 15)]
    public float destroyMissleTime = 5;

    // Camera variables
    CleanSceneScript cleanSceneScript;

    void Awake()
    {
        // Check for ClearSceneScipt and set to variable
        if (GameObject.FindGameObjectWithTag("LevelObject").GetComponent<CleanSceneScript>())
        {
            cleanSceneScript = GameObject.FindGameObjectWithTag("LevelObject").GetComponent<CleanSceneScript>();
        }
        else
            Debug.LogError("PlayerControllerScript needs CleanSceneScript in LevelObject");
    }

    // Use this for initialization
    void Start()
    {
        // Destroy object after time when created
        Destroy(gameObject, destroyMissleTime);

    }

    void OnCollisionEnter(Collision colliderObject)
    {
        switch (colliderObject.gameObject.tag)
        {
            case "EnemyObject":
                // Delet enemy from list for showing in front of character
                cleanSceneScript.DeletEnemyFromList(colliderObject.gameObject);
                // Delet missle and set activ of destroyable object to false 
                colliderObject.gameObject.SetActive(false);
                break;
            case "DestroyableObject":
                // Delet missle and set activ of destroyable object to false 
                colliderObject.gameObject.SetActive(false);
                break;
            case "PlayerObject":
                // do nothing
                break;
            case "MissleObject":
            default:
                // set object to sleep
                GetComponent<Rigidbody>().Sleep();
                GetComponent<SphereCollider>().isTrigger = true;
                break;
        }
    }

    void OnTriggerEnter(Collider colliderObject)
    {
        switch (colliderObject.tag)
        {
            case "GetCoinTrigger":
                // Add points
                int points = PlayerPrefs.GetInt("Points") + 100;
                PlayerPrefs.SetInt("Points", points);
                // Set coin object deactive
                colliderObject.gameObject.SetActive(false);
                break;
        }
    }
}
