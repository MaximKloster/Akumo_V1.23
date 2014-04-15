using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum DifficultyLevel
{
    Easy,
    Normal,
    Hard
}

public class LevelTile
{
    List<GameObject> levelTileObjects = new List<GameObject>();

    public LevelTile() { }

    public LevelTile(GameObject tileObject)
    {
        AddObject(tileObject);
    }

    public LevelTile(params GameObject[] tileObject)
    {
        AddObject(tileObject);
    }

    public void AddObject(GameObject tileObject)
    {
        levelTileObjects.Add(tileObject);
    }

    public void AddObject(params GameObject[] tileObjects)
    {
        foreach (GameObject tileObject in tileObjects)
            AddObject(tileObject);
    }

    public void AddObject(List<GameObject> tileObjects)
    {
        foreach (GameObject tileObject in tileObjects)
            AddObject(tileObject);
    }

    public GameObject GetTileObject(int index)
    {
        return levelTileObjects[index];
    }

    public GameObject[] getTileObjects()
    {
        return levelTileObjects.ToArray();
    }
}

public class LevelGeneratorScript : MonoBehaviour
{
    // Public unity getter
    public bool generate = false;
    public DifficultyLevel difficultyLevel = DifficultyLevel.Easy;
    public int prefabWidth = 15;
    [Range(0, 100)]
    public int CoinPercent = 10;
    public GameObject coinObject;
    public GameObject[] easyLevelPrefabs, easyObjectPrefabs,
                        normalLevelPrefabs, normalObjectPrefabs,
                        hardLevelPrefabs, hardObjectPrefabs,
                        embellishPrefabs;

    // Player variables
    public float PlayerPositionX { private get; set; }
    PlayerControllerScript playerControllerScript;

    // Level variables
    LevelTile[] currentLevelTiles = new LevelTile[7];
    LevelTile[] currentLevelInstance = new LevelTile[7];
    bool[] generateTiles = new bool[3];

    // Use this for initialization
    void Awake()
    {
        if (generate)
        {
            // Create level
            for (int i = 0; i < currentLevelTiles.Length - 3; i++)
                currentLevelTiles[i] = GenerateRandomLevelTile();
            currentLevelTiles[4] = currentLevelTiles[0];
            currentLevelTiles[5] = currentLevelTiles[1];
            currentLevelTiles[6] = currentLevelTiles[2];

            for (int i = 0; i < currentLevelTiles.Length; i++)
            {
                currentLevelInstance[i] = new LevelTile();
                InstantiateLevelTile(i);
            }
        }
    }

    void Start()
    {
        #region Check for CharacterController and set to variable
        if (GameObject.FindGameObjectWithTag("PlayerObject").GetComponent<PlayerControllerScript>())
            playerControllerScript = GameObject.FindGameObjectWithTag("PlayerObject").GetComponent<PlayerControllerScript>();
        else
            Debug.LogError("LevelGeneratorScript needs PlayerControllerScript in Playerobject");
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        if (generate)
            if (PlayerPositionX >= prefabWidth * (currentLevelTiles.Length - 3))
            {
                ChangeTile(3);
                SetPlayerToXZero();
            }
            else if (!generateTiles[0] && PlayerPositionX >= prefabWidth)
            {
                ChangeTiles(0, 4);
                generateTiles[0] = true;
            }
            else if (!generateTiles[1] && PlayerPositionX >= prefabWidth * 2)
            {
                ChangeTiles(1, 5);
                generateTiles[1] = true;
            }
            else if (!generateTiles[2] && PlayerPositionX >= prefabWidth * 3)
            {
                ChangeTiles(2, 6);
                generateTiles[2] = true;
            }
    }

    // Change Tiles
    void ChangeTiles(int newTileIndex, int doubleToIndex)
    {
        ChangeTile(newTileIndex);

        DestroyLevelTile(doubleToIndex);
        currentLevelTiles[doubleToIndex] = currentLevelTiles[newTileIndex];
        InstantiateLevelTile(doubleToIndex);
    }

    void ChangeTile(int newTileIndex)
    {
        DestroyLevelTile(newTileIndex);
        currentLevelTiles[newTileIndex] = GenerateRandomLevelTile();
        InstantiateLevelTile(newTileIndex);
    }

    LevelTile GenerateRandomLevelTile()
    {
        LevelTile currentLevelTile = new LevelTile();

        // Random tile
        int currentTile = Random.Range(0, easyLevelPrefabs.Length);
        currentLevelTile.AddObject(easyLevelPrefabs[currentTile]);

        // Random coins
        currentLevelTile.AddObject(GenerateRandomCoinPositions(currentTile, easyLevelPrefabs));

        // Add coins to current level tile
        //foreach (Transform randomCoinPosition in randomCoinPositionList)
        //{
        //    coinObject.transform.position = randomCoinPosition.position;
        //    currentLevelTile.AddObject(coinObject);
        //}


        if (easyLevelPrefabs[currentTile].transform.FindChild("Level Object Position") != null)
        {
            List<Transform> objectPositionsList = new List<Transform>();
            for (int i = 0; i < easyLevelPrefabs[currentTile].transform.childCount; i++)
                if (easyLevelPrefabs[currentTile].transform.GetChild(i).name == ("Level Object Position"))
                    objectPositionsList.Add(easyLevelPrefabs[currentTile].transform.GetChild(i));

            Transform objectPosition = objectPositionsList[Random.Range(0, objectPositionsList.Count)];

            // Random object
            int currentObject = Random.Range(0, easyObjectPrefabs.Length);
            GameObject currentTileObject = easyObjectPrefabs[currentObject];
            currentTileObject.transform.position = objectPosition.transform.position;
            currentLevelTile.AddObject(currentTileObject);
        }

        return currentLevelTile;
    }

    GameObject[] GenerateRandomCoinPositions(int currentPrefab, GameObject[] prefab)
    {
        if (prefab[currentPrefab].transform.FindChild("Coin Object Position") != null)
        {
            List<GameObject> coinPositionsList = new List<GameObject>();
            for (int i = 0; i < prefab[currentPrefab].transform.childCount; i++)
                if (prefab[currentPrefab].transform.GetChild(i).name == ("Coin Object Position")
                    && Random.Range(0, 100) < CoinPercent)
                    coinPositionsList.Add(prefab[currentPrefab].transform.GetChild(i).gameObject); 

            GameObject[] coinObjectsList = new GameObject[coinPositionsList.Count];
            for (int i = 0; i < coinObjectsList.Length; i++)
            {
                coinObjectsList[i] = coinObject;
                coinObjectsList[i].transform.position = coinPositionsList[i].transform.position;
            }

            return coinObjectsList;
        }
        else
            return null;
    }

    void InstantiateLevelTile(int currentLevelTileIndex)
    {
        for (int i = 0; i < currentLevelTiles[currentLevelTileIndex].getTileObjects().Length; i++)
            currentLevelInstance[currentLevelTileIndex].AddObject(Instantiate(currentLevelTiles[currentLevelTileIndex].GetTileObject(i),
                                   new Vector3(currentLevelTileIndex * prefabWidth - prefabWidth / 2 + currentLevelTiles[currentLevelTileIndex].GetTileObject(i).transform.position.x,
                                               currentLevelTiles[currentLevelTileIndex].GetTileObject(i).transform.position.y,
                                               currentLevelTiles[currentLevelTileIndex].GetTileObject(i).transform.position.z),
                                   Quaternion.Euler(0, 0, 0)) as GameObject);
    }

    void DestroyLevelTile(int currentLevelTileIndex)
    {
        for (int i = 0; i < currentLevelInstance[currentLevelTileIndex].getTileObjects().Length; i++)
            Destroy(currentLevelInstance[currentLevelTileIndex].GetTileObject(i));
    }

    // Set Player to start after lose
    void SetPlayerToXZero()
    {
        playerControllerScript.SetToPositionXZero();

        for (int i = 0; i < generateTiles.Length; i++)
            generateTiles[i] = false;
    }
}
