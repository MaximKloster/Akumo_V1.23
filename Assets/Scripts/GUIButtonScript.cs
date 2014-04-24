using UnityEngine;
using System.Collections;

public enum ButtonSetting
{
    //noSettingButton,
    startLevelButton,
    nextLevelButton,
    restartLevelButton,
    continueButton,
    infinitLevelButton,
    levelChoiceButton,
    menuButton,
    changeInputButton,
    soundButton,
    creditsButton,
    exitButton
}

/*public enum ButtonHandling
{
    NoHandlingButton,
    JumpButton,
    SlideButton,
    ShootButton
}*/

public class GUIButtonScript : MonoBehaviour
{
    public ButtonSetting buttonSetting = ButtonSetting.nextLevelButton;
    //public ButtonHandling buttonHandling = ButtonHandling.NoHandlingButton;
    public int level = 1;
    [Range(1, 100)]
    public float xDistance = 5, yDistance = 5;
    [Range(0.01f, 50)]
    public float size = 1;
    public Texture2D mainTexture, activatedTexture = null;
    private float screenFactorX, screenFactorY, xCoord, yCoord, xSize, ySize;

    PlayerControllerScript playerController;
    bool mouseDown;

    void Start()
    {
        //#region Check for PlayerControllerScript and set to variable
        //// Check for CameraScipt and set to variable
        //if (GameObject.FindGameObjectWithTag("PlayerObject").GetComponent<PlayerControllerScript>())
        //    playerController = GameObject.FindGameObjectWithTag("PlayerObject").GetComponent<PlayerControllerScript>();
        //#endregion

        screenFactorX = Screen.width / 100;
        screenFactorY = Screen.height / 100;
        xSize = mainTexture.width / 10 * screenFactorX * size;
        ySize = mainTexture.height / 10 * screenFactorY * size;
        xCoord = xDistance * screenFactorX;
        yCoord = yDistance * screenFactorY;
    }

    void Update()
    {
        screenFactorX = Screen.width / 100;
        screenFactorY = Screen.height / 100;
        xSize = mainTexture.width / 10 * screenFactorX * size;
        ySize = mainTexture.height / 10 * screenFactorY * size;
        xCoord = xDistance * screenFactorX;
        yCoord = yDistance * screenFactorY;
    }

    private void OnGUI()
    {
        if (mainTexture != null && GUI.Button(new Rect(xCoord, yCoord, xSize, ySize), mainTexture, GUIStyle.none))
            switch (buttonSetting)
            {
                case ButtonSetting.exitButton:
                    Application.Quit();
                    break;
                case ButtonSetting.menuButton:
                    LoadingScript.loadingScreenOn = true;
                    PlayerPrefs.SetString("Load_Level", "MainMenu");
                    break;
                case ButtonSetting.infinitLevelButton:
                    LoadingScript.loadingScreenOn = true;
                    PlayerPrefs.SetString("Load_Level", "Level_Infinit");
                    break;
                case ButtonSetting.changeInputButton:
                    if (PlayerPrefs.GetInt("Handle_Buttons") == 0)
                        PlayerPrefs.SetInt("Handle_Buttons", 1);
                    else
                        PlayerPrefs.SetInt("Handle_Buttons", 0);
                    break;
                case ButtonSetting.soundButton:
                    if (PlayerPrefs.GetInt("Sound_Buttons") == 0)
                        PlayerPrefs.SetInt("Sound_Buttons", 1);
                    else
                        PlayerPrefs.SetInt("Sound_Buttons", 0);
                    break;
                case ButtonSetting.startLevelButton:
                default:
                    LoadingScript.loadingScreenOn = true;
                    PlayerPrefs.SetString("Load_Level", "Level_" + level);
                    break;
            }

        //if (buttonHandling != ButtonHandling.NoHandlingButton)
        //    if (GUI.RepeatButton(new Rect(xCoord, yCoord, xSize, ySize), texture, GUIStyle.none))
        //    {
        //        if (!mouseDown && playerController)
        //            switch (buttonHandling)
        //            {
        //                case ButtonHandling.JumpButton:
        //                    playerController.SetMovementPhase(MovementPhase.Jump);
        //                    break;
        //                case ButtonHandling.SlideButton:
        //                    playerController.SetMovementPhase(MovementPhase.Slide);
        //                    break;
        //                case ButtonHandling.ShootButton:
        //                    playerController.SetMovementPhase(MovementPhase.Shoot);
        //                    break;
        //            }
        //        mouseDown = true;
        //    }
        //    else if (Event.current.type == EventType.Repaint)
        //        if (mouseDown)
        //            mouseDown = false;
    }
}
