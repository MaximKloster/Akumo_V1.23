using UnityEngine;
using System.Collections;

public enum ButtonSetting
{
    noSettingButton,
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

public enum ButtonHandling
{
    NoHandlingButton,
    JumpButton,
    SlideButton,
    ShootButton
}

public class GUIButtonScript : MonoBehaviour
{
    public ButtonSetting buttonSetting = ButtonSetting.noSettingButton;
    public ButtonHandling buttonHandling = ButtonHandling.NoHandlingButton;
    public int level = 1;
    [Range(1, 100)]
    public float xDistance = 5, yDistance = 5;
    [Range(0.01f, 50)]
    public float size = 1;
    public Texture2D texture;
    private float screenSize, xCoord, yCoord, xSize, ySize;

    PlayerControllerScript playerController;
    bool mouseDown;

    void Start()
    {
        //#region Check for PlayerControllerScript and set to variable
        //// Check for CameraScipt and set to variable
        //if (GameObject.FindGameObjectWithTag("PlayerObject").GetComponent<PlayerControllerScript>())
        //    playerController = GameObject.FindGameObjectWithTag("PlayerObject").GetComponent<PlayerControllerScript>();
        //#endregion

        screenSize = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height) / 100;
        //Debug.Log("MenuButton ScreenSize: " +screenSize);
        xSize = texture.width / 10 * screenSize * size;
        //Debug.Log("MenuButton xSize: " +xSize);
        ySize = texture.height / 10 * screenSize * size;
        //Debug.Log("MenuButton ySize: " +ySize);
        xCoord = xDistance * screenSize;
        //Debug.Log("MenuButton xCoord: " +xCoord);
        yCoord = yDistance * screenSize;
        //Debug.Log("MenuButton yCoord: " +yCoord);
    }

    void Update()
    {
        screenSize = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height) / 100;
        xSize = texture.width / 10 * screenSize * size;
        ySize = texture.height / 10 * screenSize * size;
        xCoord = xDistance * screenSize;
        yCoord = yDistance * screenSize;
    }

    private void OnGUI()
    {
        #region Setting button
        if (buttonSetting != ButtonSetting.noSettingButton)
            if (texture != null && GUI.Button(new Rect(xCoord, yCoord, xSize , ySize ), texture, GUIStyle.none))
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
        #endregion

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
