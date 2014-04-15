using UnityEngine;
using System.Collections;

public class StartCounterScript : MonoBehaviour
{
    // public unity getter
    public GUIText startCounterObject;
    [Range(0, 5)]
    public int startCounterTime = 3;

    // start counter variables
    float startTime;
    // player variables
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

        // Set start time
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // Show seconds to start (Countdown)
        if (Time.time <= startTime + startCounterTime)
            startCounterObject.text = "" + (int)(startTime + startCounterTime + 1 - Time.time);
        else
        {
            // Set counter inactiv an start game
            startCounterObject.gameObject.SetActive(false);
            playerControllerScript.Play = true;
        }

    }
}
