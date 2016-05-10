using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreScript : MonoBehaviour {

    Vector3 bottomCorner;
    Vector3 screenMiddle;
    Vector3 offScreen;

    Vector3 targetPosition;

    Image squareChild;
    Image flashScreen;
    GameObject menuBG;
    GameObject textChild;
    GameObject retryChild;
    GameObject menuPlayChild;
    GameObject enterMenuChild;
    GameObject leaderboard;
    GameObject achievements;
    GameObject titleChild;

    Color squareColour = Color.clear;

	void Start () {

        textChild = GameObject.Find("ScoreText");
        squareChild = GameObject.Find("Fader").GetComponent<Image>();
        flashScreen = GameObject.Find("FlashScreen").GetComponent<Image>();
        retryChild = GameObject.Find("Retry");
        menuPlayChild = GameObject.Find("MenuPlay");
        enterMenuChild = GameObject.Find("EnterMenu");
        titleChild = GameObject.Find("Title");
        leaderboard = GameObject.Find("Leaderboard");
        achievements = GameObject.Find("Achievements");
        menuBG = GameObject.Find("MenuBG");

        retryChild.SetActive(false);
        enterMenuChild.SetActive(false);

        bottomCorner = textChild.transform.localPosition;
        screenMiddle = new Vector3(0, 50, 0);
        offScreen = new Vector3(-300, bottomCorner.y, 0);

        textChild.transform.localPosition = offScreen;
        targetPosition = offScreen;

        squareChild.color = new Color(0.157f, 0.157f, 0.157f);
	}

	void Update () {

        textChild.transform.localPosition = Vector3.Lerp(textChild.transform.localPosition, targetPosition, 0.08f);
        textChild.transform.localScale = Vector3.Lerp(textChild.transform.localScale, new Vector3(0.5f, 0.5f, 0.5f), 0.08f);

        squareChild.color = Color.Lerp(squareChild.color, squareColour, 0.8f);
        flashScreen.color = Color.Lerp(flashScreen.color, Color.clear, 0.08f);
	}

    public void EndGame()
    {
        targetPosition = screenMiddle;

        squareColour = new Color(0, 0, 0, 0.5f);

        retryChild.SetActive(true);
        enterMenuChild.SetActive(true);
    }

    public void StartGame()
    {
        targetPosition = bottomCorner;

        squareColour = Color.clear;

        retryChild.SetActive(false);
        enterMenuChild.SetActive(false);
        titleChild.SetActive(false);
    }

    public void Menu(bool isActive)
    {
        if (isActive)
        {
            //squareColour = new Color(0.157f, 0.157f, 0.157f);
            
            targetPosition = offScreen;
            if (textChild != null)
                textChild.transform.localPosition = targetPosition;
            if (menuPlayChild != null)
                menuPlayChild.SetActive(true);
            if (enterMenuChild != null)
                enterMenuChild.SetActive(false);
            if (titleChild != null)
                titleChild.SetActive(true);
            if (menuBG != null)
                menuBG.SetActive(true);
            if (leaderboard != null)
                leaderboard.SetActive(true);
            if (achievements != null)
                achievements.SetActive(true);
        }
        else
        {
            //squareColour = Color.clear;
            menuBG.SetActive(false);
            targetPosition = bottomCorner;
            textChild.transform.localPosition = targetPosition;
            menuPlayChild.SetActive(false);
            leaderboard.SetActive(false);
            achievements.SetActive(false);
        }
    }

    public void BounceText()
    {
        textChild.transform.localScale = new Vector3(0.65f, 0.65f, 0.65f);
    }

    public void FlashWhite()
    {
        flashScreen.color = new Color(1, 1, 1, 0.33f);
    }

    public void FlashRed()
    {
        flashScreen.color = new Color(0.67f, 0, 0, 0.25f);
    }
}
