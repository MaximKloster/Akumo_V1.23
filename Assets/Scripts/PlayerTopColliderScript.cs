using UnityEngine;
using System.Collections;

public class PlayerTopColliderScript : MonoBehaviour
{
    PlayerControllerScript playerControllerScript;

    // Use this for initialization
    void Start()
    {
        #region Check for PlayerObject and set to variable
        // Check for PlayerControllerScript and set to variable
        if (GameObject.FindGameObjectWithTag("PlayerObject").GetComponent<PlayerControllerScript>())
            playerControllerScript = GameObject.FindGameObjectWithTag("PlayerObject").GetComponent<PlayerControllerScript>();
        else
            Debug.LogError("PlayerTopCollisionScript needs PlayerControllerScript in PlayerObject");
        #endregion
    }

    // Triggers
    void OnTriggerStay(Collider colliderObject)
    {
        switch (colliderObject.tag)
        {
            case "Untagged":
                playerControllerScript.PlayerTopTrigger = true;
                break;
            case "GetCoinTrigger":
                playerControllerScript.GetCoin(colliderObject);
                break;
            case "MissleObject":
                playerControllerScript.GetMissle(colliderObject);
                break;
        }
    }
    void OnTriggerExit(Collider colliderObject)
    {
        if (colliderObject.tag == "Untagged")
            playerControllerScript.PlayerTopTrigger = false;
    }
}
