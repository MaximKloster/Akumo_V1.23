using UnityEngine;
using System.Collections;

public class PointsScript : MonoBehaviour
{
    public int countSpeed = 1;
    [Range(1, 100)]
    public float countPosX = 5, countPosY = 5, continueButtonX, continueButtonY, restartButtonX, restartButtonY;
    [Range(0.01f, 10)]
    public float countSize = 10, continueButtonSize = 5, restartButtonSize = 5;
    public Texture2D continueButtonTexture = null, restartButtonTexture = null;
    float currentCount;
    Rect continueButton, restartButton;
    int continueLevel = 1, endCount;

    GUIStyle textFieldStyle = GUIStyle.none;
    FontStyle fontStyle;
    public Font textFieldFont;
    public int textFieldTextSize = 10;
    public Color textFieldTextColor = Color.white;

    private float screenSize, xCoordsCount, yCoordsCount, xCoordContinue, yCoordContinue, xCoordsReset, yCoordsReset, xSizeContinue, ySizeContinue, xSizeReset, ySizeReset;

    void Start()
    {
        endCount = PlayerPrefs.GetInt("Points");

        screenSize = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height) / 100;

        xCoordsCount = countPosX * screenSize;
        yCoordsCount = countPosY * screenSize;

        xSizeContinue = continueButtonTexture.width / 100 * screenSize * continueButtonSize;
        ySizeContinue = continueButtonTexture.height / 100 * screenSize * continueButtonSize;
        
        xCoordContinue = continueButtonX * screenSize;
        yCoordContinue = continueButtonY * screenSize;
        
        xSizeReset = restartButtonTexture.width / 100 * screenSize * restartButtonSize;
        ySizeReset = restartButtonTexture.height / 100 * screenSize * restartButtonSize;
        
        xCoordsReset = restartButtonX * screenSize;
        yCoordsReset = restartButtonY * screenSize;

        // Set text field style
        textFieldStyle.font = textFieldFont;
        textFieldStyle.fontSize = textFieldTextSize;
        textFieldStyle.normal.textColor = textFieldTextColor;
    }

    // Update is called once per frame
    void Update()
    {
        currentCount = Mathf.Lerp(currentCount, endCount + 1, Time.deltaTime * countSpeed);
        screenSize = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height) / 100;
        xCoordsCount = countPosX * screenSize;
        yCoordsCount = countPosY * screenSize;
        xSizeContinue = continueButtonTexture.width / 100 * screenSize * continueButtonSize;
        ySizeContinue = continueButtonTexture.height / 100 * screenSize * continueButtonSize;
        xCoordContinue = continueButtonX * screenSize;
        yCoordContinue = continueButtonY * screenSize;
        xSizeReset = restartButtonTexture.width / 100 * screenSize * restartButtonSize;
        ySizeReset = restartButtonTexture.height / 100 * screenSize * restartButtonSize;
        xCoordsReset = restartButtonX * screenSize;
        yCoordsReset = restartButtonY * screenSize;
    }

    private void OnGUI()
    {
        if (continueButtonTexture != null) 
            continueButton = new Rect(xCoordContinue, yCoordContinue, xSizeContinue, ySizeContinue);
        else
            continueButton = new Rect(xCoordContinue, yCoordContinue, continueButtonSize, continueButtonSize);

        if (restartButtonTexture != null)
            restartButton = new Rect(xCoordsReset, yCoordsReset, xSizeReset, ySizeReset);
        else
            restartButton = new Rect(xCoordsReset, yCoordsReset, restartButtonSize, restartButtonSize);

        GUI.TextField(new Rect(xCoordsCount, yCoordsCount, 20 * countSize, 2 * countSize), "Points: " + (int)currentCount, GUIStyle.none);
        if (continueButtonTexture != null && GUI.Button(continueButton, continueButtonTexture, GUIStyle.none))
            Application.LoadLevel("Level_" + continueLevel);
        else if (continueButtonTexture == null && GUI.Button(continueButton, "Continue", GUIStyle.none))
            Application.LoadLevel("Level_" + continueLevel);
        
        if (restartButtonTexture != null && GUI.Button(restartButton, restartButtonTexture, GUIStyle.none))
            Application.LoadLevel("MainMenu");
        else if (restartButtonTexture == null && GUI.Button(restartButton, "Restart", GUIStyle.none))
            Application.LoadLevel("MainMenu");
    }
}
