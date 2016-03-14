using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreScript : MonoBehaviour {

    Vector3 bottomCorner;
    Vector3 screenMiddle;
    Vector3 offScreen;

    Vector3 targetPosition;

    Image squareChild;
    GameObject textChild;
    GameObject retryChild;
    GameObject menuPlayChild;
    GameObject enterMenuChild;

    Color squareColour = Color.clear;

	void Start () {

        textChild = GameObject.Find("ScoreText");
        squareChild = GetComponentInChildren<Image>();
        retryChild = GameObject.Find("Retry");
        menuPlayChild = GameObject.Find("MenuPlay");
        enterMenuChild = GameObject.Find("EnterMenu");

        retryChild.SetActive(false);
        enterMenuChild.SetActive(false);

        bottomCorner = textChild.transform.localPosition;
        screenMiddle = new Vector3(0, 200, 0);
        offScreen = new Vector3(-300, bottomCorner.y, 0);

        textChild.transform.localPosition = offScreen;
        targetPosition = offScreen;

        squareChild.color = new Color(0.157f, 0.157f, 0.157f);
	}

	void Update () {

        textChild.transform.localPosition = Vector3.Lerp(textChild.transform.localPosition, targetPosition, 0.08f);

        squareChild.color = Color.Lerp(squareChild.color, squareColour, 0.8f);
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
    }

    public void Menu(bool isActive)
    {
        if (isActive)
        {
            squareColour = new Color(0.157f, 0.157f, 0.157f);
            targetPosition = offScreen;
            if (textChild != null)
                textChild.transform.localPosition = targetPosition;
            if (menuPlayChild != null)
                menuPlayChild.SetActive(true);
            if (enterMenuChild != null)
                enterMenuChild.SetActive(false);
        }
        else
        {
            squareColour = Color.clear;
            targetPosition = bottomCorner;
            textChild.transform.localPosition = targetPosition;
            menuPlayChild.SetActive(false);
        }
    }
}
