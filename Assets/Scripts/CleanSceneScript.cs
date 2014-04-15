using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CleanSceneScript : MonoBehaviour
{
    // List variables
    GameObject[] coinObjects, destroyableObjects, enemyObjects;
    List<GameObject> enemyInfrontOfCharacterObjects = new List<GameObject>();

    // Character variables
    float characterActiveZone = 25;
    public float PlayerPositionX { private get; set; }

    // Use this for initialization
    void Awake()
    {
        coinObjects = GameObject.FindGameObjectsWithTag("GetCoinTrigger");
        destroyableObjects = GameObject.FindGameObjectsWithTag("DestroyableObject");
        enemyObjects = GameObject.FindGameObjectsWithTag("EnemyObject");
    }

    void Update()
    {
        // Check all nessessary enemies
        CheckEnemies(PlayerPositionX);
    }

    // Call all reset methodes
    public void ResetCompleteScene()
    {
        ResetCoinObjects();
        ResetDestroyableObjects();
        ResetEnemies();
        DeletAllMissles();
    }

    #region Reset methodes
    // Reshow all coin objects
    public void ResetCoinObjects()
    {
        foreach (GameObject coinObject in coinObjects)
            coinObject.SetActive(true);
    }
    // Reshow all destroyable objects
    public void ResetDestroyableObjects()
    {
        foreach (GameObject destroyableObject in destroyableObjects)
            destroyableObject.SetActive(true);
    }
    // Dont show enemies and set them to in front of character list
    public void ResetEnemies()
    {
        enemyInfrontOfCharacterObjects = enemyObjects.ToList<GameObject>();

        // Set all enemys to startposition
        foreach (var enemy in enemyInfrontOfCharacterObjects)
        {
            // Set active for set to startposition
            enemy.SetActive(true);
            enemy.GetComponent<EnemyControllerScript>().SetEnemyToStartPosition();
            enemy.SetActive(false);
        }
    } 
    #endregion

    #region Delet methodes
    public void DeletAllMissles()
    {
        foreach (GameObject missle in GameObject.FindGameObjectsWithTag("MissleObject"))
            Destroy(missle);
    }
    public void DeletEnemyFromList(GameObject enemy)
    {
        enemyInfrontOfCharacterObjects.Remove(enemy);
    } 
    #endregion

    // Call object methodes
    public void CheckEnemies(float PlayerPositionX)
    {
        if (enemyInfrontOfCharacterObjects.Count > 0)
        {
            // buffer variable
            var bufferdPlayerPositionX = PlayerPositionX;

            // check view area to show eniemy objects 
            foreach (var enemy in enemyInfrontOfCharacterObjects.Where(e => e.transform.position.x <= bufferdPlayerPositionX - 10))
                enemy.SetActive(false);
            foreach (var enemy in enemyInfrontOfCharacterObjects.Where(e => e.transform.position.x > bufferdPlayerPositionX && e.transform.position.x <= bufferdPlayerPositionX + characterActiveZone))
                enemy.SetActive(true);

            // delet enemy objects behind player from list
            enemyInfrontOfCharacterObjects.RemoveAll(e => e.transform.position.x <= bufferdPlayerPositionX - 10);
        }
    }
}
