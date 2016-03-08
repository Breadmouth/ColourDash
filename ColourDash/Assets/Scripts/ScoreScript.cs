using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreScript : MonoBehaviour {

    Vector3 bottomCorner;
    Vector3 screenMiddle;

    Vector3 targetPosition;

    GameObject textChild;
    Image squareChild;

    Color targetColour = Color.clear;

	void Start () {

        textChild = GameObject.Find("ScoreText");
        squareChild = GetComponentInChildren<Image>();

        bottomCorner = new Vector3(-140, -255, 0);
        screenMiddle = Vector3.zero;

        targetPosition = bottomCorner;
	}

	void Update () {

        textChild.transform.localPosition = Vector3.Lerp(textChild.transform.localPosition, targetPosition, 0.08f);

        squareChild.color = Color.Lerp(squareChild.color, targetColour, 0.8f);
	}

    public void EndGame()
    {
        targetPosition = screenMiddle;

        targetColour = new Color(0, 0, 0, 0.5f);
    }

    public void StartGame()
    {
        targetPosition = bottomCorner;

        targetColour = Color.clear;
    }
}
