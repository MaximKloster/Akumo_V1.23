using UnityEngine;
using System.Collections;

public class LoadingScript : MonoBehaviour
{
    [Range(1, 100)]
    public float logoPosX = 1, logoPosY = 1f, loadingItemPosX = 5, loadingItemPosY = 5;
    [Range(0.5f, 25)]
    public float logoSize = 1, loadingItemSize = 1;
    public int rotationSpeed = 10;

    //public GUIText countdown = null;
    public Texture2D logoTexture = null;
    public Texture2D loadingItemTexture = null;
    public static bool loadingScreenOn = false;

    // Use this for initialization
    public GameObject guiButtons, blackSreen, guiStuff;

    private float rotationAngle = 0, currentCount;
    private float screenSize, xCoordLogo, yCoordLogo, xSizeLogo, ySizeLogo, xCoordLoadingItem, yCoordLoadingItem, xSizeLoadingItem, ySizeLoadingItem;
    Rect shuriken, logo;
    // Update is called once per frame

    void Awake()
    {
        blackSreen.SetActive(false);
    }

    void Start()
    {
        //screenSize = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height) / 100;

        //xSizeLogo = logoTexture.width / 100 * screenSize * logoSize;
        //ySizeLogo = logoTexture.height / 100 * screenSize * logoSize;
        //xCoordLogo = logoPosX * screenSize - xSizeLogo / 2;
        //yCoordLogo = logoPosY * screenSize - ySizeLogo / 2;

        //xSizeLoadingItem = loadingItemTexture.width / 100 * screenSize * loadingItemSize;
        //ySizeLoadingItem = loadingItemTexture.height / 100 * screenSize * loadingItemSize;
        //xCoordLoadingItem = loadingItemPosX * screenSize;
        //yCoordLoadingItem = loadingItemPosY * screenSize;
    }

    void Update()
    {
        rotationAngle += rotationSpeed;
        if (loadingScreenOn)
        {
            //countdown.gameObject.SetActive(false);
            if(guiButtons != null) guiButtons.SetActive(false);
            blackSreen.SetActive(true);
            if(guiStuff != null) guiStuff.SetActive(false);
            screenSize = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height) / 100;

            //if (logoTexture)
            //{
            //    xSizeLogo = logoTexture.width / 100 * screenSize * logoSize;
            //    ySizeLogo = logoTexture.height / 100 * screenSize * logoSize;
            //    xCoordLogo = logoPosX * screenSize - xSizeLogo / 2;
            //    yCoordLogo = logoPosY * screenSize - ySizeLogo / 2;

            //    logo = new Rect(xCoordLogo, yCoordLogo, xSizeLogo, ySizeLogo);
            //}

            //if (loadingItemTexture != null)
            //{
            //    xSizeLoadingItem = loadingItemTexture.width / 100 * screenSize * loadingItemSize;
            //    ySizeLoadingItem = loadingItemTexture.height / 100 * screenSize * loadingItemSize;
            //    xCoordLoadingItem = loadingItemPosX * screenSize;
            //    yCoordLoadingItem = loadingItemPosY * screenSize;

            //    shuriken = new Rect(xCoordLoadingItem, yCoordLoadingItem, xSizeLoadingItem, ySizeLoadingItem);
            //}
            Application.LoadLevelAsync(PlayerPrefs.GetString("Load_Level"));
            loadingScreenOn = false;
        }
        else if (!Application.isLoadingLevel)
        {
            if (guiButtons != null) guiButtons.SetActive(true);
        }
    }

    private void OnGUI()
    {
        //if (logoTexture != null)
        //    GUI.Label(logo, logoTexture);

        //if (loadingItemTexture != null)
        //{
        //    GUIUtility.RotateAroundPivot(rotationAngle, new Vector2(shuriken.x + xSizeLoadingItem / 2, shuriken.y + ySizeLoadingItem / 2));
        //    GUI.Label(shuriken, loadingItemTexture);
        //}

    }
}
