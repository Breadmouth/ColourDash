using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

    bool canMove = true;
    Vector3 targetPosition = Vector3.zero;

    CameraScript mainCamera;

    GameControllerScript gameController;

    float rotateSpeed = 100.0f;

    bool touchNode = false;

    float rotationDir = 1.0f;

    Text scoreText;

    int score;

    GameObject currentNode;

    ScoreScript scoreScript;

	void Start () {

        mainCamera = GameObject.Find("MainCamera").GetComponent<CameraScript>();
        gameController = GameObject.Find("MainCamera").GetComponent<GameControllerScript>();

        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();

        scoreScript = GameObject.Find("Overlay").GetComponent<ScoreScript>();
	}

	void Update () {

        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began && canMove)
            {
                //move in direction faced x distance
                targetPosition = transform.position + transform.up * 5.0f;

                mainCamera.SetFollow(false);

                canMove = false;
            }
        }

        //old computer controls

        if (Input.GetMouseButtonDown(0) && canMove)
        {
            //move in direction faced x distance
            targetPosition = transform.position + transform.up * 5.0f;

            mainCamera.SetFollow(false);

            canMove = false;
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.12f);

        if ((transform.position - targetPosition).magnitude < 0.01f && canMove == false)
        {
            if (!touchNode)
                Restart();

            ReachNode();
            UpdateScore();
        }

        if (canMove)
        {
            if (transform.rotation.eulerAngles.z > 90.0f && transform.rotation.eulerAngles.z < 270.0f)
            {
                rotationDir *= -1;
                if (transform.rotation.eulerAngles.z < 180.0f)
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90.0f));
                else
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270.0f));
            }

            //rotate player
            transform.Rotate(transform.forward, rotateSpeed * Time.deltaTime * rotationDir);
        }

	}

    void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Node")
        {
            targetPosition = other.transform.position - new Vector3(0, 0, 5);

            touchNode = true;

            currentNode = other.gameObject;
        }
        else if (other.tag == "Enemy")
        {
            Restart();
        }
    }

    void Restart()
    {
        //scoreScript.EndGame();
        Application.LoadLevel(Application.loadedLevel);
    }

    void UpdateScore()
    {
            score++;

            scoreText.text = score.ToString();
    }

    void ReachNode()
    {
        touchNode = false;

        transform.position = targetPosition;

        canMove = true;
        mainCamera.SetFollow(true);

        currentNode.GetComponent<NodeScript>().ActivateNode();

        gameController.CreateNewNode();

        rotateSpeed += 2.0f;
    }
}
