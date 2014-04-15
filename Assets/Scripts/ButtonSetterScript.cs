using UnityEngine;
using System.Collections;

public class ButtonSetterScript : MonoBehaviour 
{
    public GameObject handlingButtons;
    int currentStance;
	// Use this for initialization
    InputLogicScript inputLogic;

	void Start () 
    {
        #region Check for InputLogic and set to variable
        // Check for CameraScipt and set to variable
        if (GameObject.FindGameObjectWithTag("PlayerObject").GetComponent<InputLogicScript>())
            inputLogic = GameObject.FindGameObjectWithTag("PlayerObject").GetComponent<InputLogicScript>();
        else
            Debug.LogError("ButtonSetterScript needs InputLogicScript in PlayerObject");
        #endregion

        if (PlayerPrefs.GetInt("Handle_Buttons") == 1)
        {
            currentStance = 1;
            activateButtons();
            inputLogic.buttonInput = true;
        }
        else
        {
            currentStance = 0;
            activateButtons();
            inputLogic.buttonInput = false;
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        /*if (PlayerPrefs.GetInt("Handle_Buttons") != currentStance)
        {
            currentStance = PlayerPrefs.GetInt("Handle_Buttons");
            activateButtons();
            Debug.Log("Changed Buttons");
        }*/
	}

    void activateButtons()
    {
        if (currentStance == 1) handlingButtons.SetActive(true);
        else handlingButtons.SetActive(false);
    }
}
