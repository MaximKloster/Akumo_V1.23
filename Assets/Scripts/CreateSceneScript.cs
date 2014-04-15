using UnityEngine;
using System.Collections;

public class CreateSceneScript : MonoBehaviour
{
    // Public object variables
    public GameObject playerObject, cameraObject;
    public Transform playerStartPosition;

    // Use this for initialization
    void Awake()
    {
        // Create objects
        Instantiate(cameraObject, playerStartPosition.position, Quaternion.Euler(0, 0, 0));
        Instantiate(playerObject, playerStartPosition.position, Quaternion.Euler(0, 0, 0));
    }
}
