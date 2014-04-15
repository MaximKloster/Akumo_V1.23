using UnityEngine;
using System.Collections;

public enum TouchStatus
{
    MainGame,
    MainMenu
}

public class InputLogicScript : MonoBehaviour
{
    // Public unity setter
    public bool touchInput = true, keyInput = false, buttonInput = false;
    [Range(5, 100)]
    public float minSwipDist = 10.0f;
    [Range(0, 1)]
    public float maxTipeTime = 0.2f;

    // Touch variables
    TouchStatus touchStatus = TouchStatus.MainGame;
    Vector2[] touchStartPos = new Vector2[2];
    float[] touchStartTime = new float[2];
    bool swiped;

    // Player controller variables
    PlayerControllerScript playerControllerScript;

    void Start()
    {
        // Check for PlayerControllerScript and set to variable
        if (GetComponent<PlayerControllerScript>())
            playerControllerScript = GetComponent<PlayerControllerScript>();
        else
            Debug.LogError("TouchLogicScript needs PlayerControllerScript in Playerobject");
    }

    void Update()
    {
        // Check for all chosen inputs
        if (touchInput && !buttonInput)
            CheckTouchInput();
        if (keyInput && !buttonInput)
            CheckKeyInput();
        /* TODO
         * BEREINIGEN BEI FERTIGSTELLUNG!!!!
         */
    }

    void CheckTouchInput()
    {
        if (Input.touchCount > 0)
            for (int currentTouch = 0; currentTouch < Input.touchCount; currentTouch++)
            {
                Touch touch = Input.GetTouch(currentTouch);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        // Set touch start position and time 
                        touchStartPos[currentTouch] = touch.position;
                        touchStartTime[currentTouch] = Time.time;
                        break;
                    case TouchPhase.Moved:
                        // Swipe up
                        if (touch.position.y >= touchStartPos[currentTouch].y + minSwipDist)
                        {
                            touchStartPos[currentTouch] = touch.position;
                            swiped = true;
                            InputUp();
                        }
                        // Swipe down
                        else if (touch.position.y <= touchStartPos[currentTouch].y - minSwipDist)
                        {
                            touchStartPos[currentTouch] = touch.position;
                            swiped = true;
                            InputDown();
                        }
                        break;
                    case TouchPhase.Canceled:
                    case TouchPhase.Ended:
                        // Tipe short
                        if (!swiped && Time.time - touchStartTime[currentTouch] <= maxTipeTime)
                            TipeShort();
                        swiped = false;
                        break;
                }
            }
    }

    void CheckKeyInput()
    {
        if (Input.anyKey)
            // Key right
            if (Input.GetKey(KeyCode.RightArrow))
                InputRight();
            // Key left
            else if (Input.GetKey(KeyCode.LeftArrow))
                InputLeft();
            /* TODO
            * BEREINIGEN BEI FERTIGSTELLUNG!!!!
            */
            // Key up
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                InputUp();
            // Key down
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                InputDown();
            else if (Input.GetKeyDown(KeyCode.Space))
                TipeShort();
    }

    // All input methodes
    void InputRight() { }
    void InputLeft() { }
    void InputUp() 
    {
        switch (touchStatus)
        {
            case TouchStatus.MainGame:
                playerControllerScript.SetMovementPhase(MovementPhase.Jump);
                break;
            case TouchStatus.MainMenu:
                break;
        }
    }
    void InputDown() 
    {
        switch (touchStatus)
        {
            case TouchStatus.MainGame:
                playerControllerScript.SetMovementPhase(MovementPhase.Slide);
                break;
            case TouchStatus.MainMenu:
                break;
        }
    }
    // Touch input methodes
    void TipeShort() 
    {
        switch (touchStatus)
        {
            case TouchStatus.MainGame:
                playerControllerScript.SetMovementPhase(MovementPhase.Shoot);
                break;
            case TouchStatus.MainMenu:
                break;
        }
    }
}