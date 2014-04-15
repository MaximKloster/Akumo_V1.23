using UnityEngine;
using System.Collections;

public class StartScreenScript : MonoBehaviour 
{
    public int waitTime = 4;

    [Range(1, 100)]
    public float loadingItemPosX = 5, loadingItemPosY = 5;
    [Range(0.5f, 50)]
    public float loadingItemSize = 1;
    public int rotationSpeed = 10;
    public Texture loadingItem = null;

    private float screenSize, xCoord, yCoord, xSize, ySize, teamTextLength;
    Rect shuriken;
    private float rotationAngle = 0, textureWidth, textureHeight, currentCount;

    float startTime;
	// Use this for initialization
	void Start ()
    {
        startTime = Time.time;
        if(loadingItem != null)
        {
            screenSize = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height) / 100;
            xSize = loadingItem.width / 10 * screenSize * loadingItemSize;
            ySize = loadingItem.height / 10 * screenSize * loadingItemSize;
            xCoord = loadingItemPosX * screenSize;
            yCoord = loadingItemPosY * screenSize;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        rotationAngle += rotationSpeed;
        
        if (loadingItem != null)
        {
            screenSize = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height) / 100;
            xSize = loadingItem.width / 10 * screenSize * loadingItemSize;
            ySize = loadingItem.height / 10 * screenSize * loadingItemSize;
            xCoord = loadingItemPosX * screenSize;
            yCoord = loadingItemPosY * screenSize;

            shuriken = new Rect(xCoord, yCoord, xSize, ySize);
        }
        if (Time.time > startTime + waitTime) Application.LoadLevel("MainMenu");
	}

    private void OnGUI()
    {
        if (loadingItem != null)
        {
            GUIUtility.RotateAroundPivot(rotationAngle, new Vector2(shuriken.x + xSize / 2, shuriken.y + ySize / 2));
            GUI.Label(shuriken, loadingItem);
        }
    }
}
