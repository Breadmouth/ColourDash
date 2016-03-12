using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class PlayerScript : MonoBehaviour {

    bool canMove = true;
    Vector3 targetPosition = Vector3.zero;

    CameraScript mainCamera;

    GameControllerScript gameController;

    float rotateSpeed = 100.0f;

    bool touchNode = false;

    float rotationDir = 1.0f;

    Text scoreText;
    Text bestScoreText;
    Text powerUpText;

    int score = 0;
    int bestScore = 0;

    GameObject currentNode;

    ScoreScript scoreScript;

    bool paused = false;

    bool pointMultiplier = false;

    float powerUpTime = 10.0f;
    float powerUpCountdown = 0.0f;

    GameObject aimer;

	void Start () {

        mainCamera = GameObject.Find("MainCamera").GetComponent<CameraScript>();
        gameController = GameObject.Find("MainCamera").GetComponent<GameControllerScript>();

        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        bestScoreText = GameObject.Find("BestScore").GetComponent<Text>();
        powerUpText = GameObject.Find("PowerUpText").GetComponent<Text>();

        scoreScript = GameObject.Find("Overlay").GetComponent<ScoreScript>();

        aimer = GameObject.Find("Aimer");

        aimer.SetActive(false);

        if (PlayerPrefs.HasKey("bestScore"))
        {
            bestScore = PlayerPrefs.GetInt("bestScore");
            bestScoreText.text = bestScore.ToString();
        }

        scoreScript.Menu(true);

        Pause(true);

        // authenticate user:
        Social.localUser.Authenticate((bool success) => {
            // handle success or failure
            if (success)
            {
                Debug.Log("success");
            }
            else
            {
                Debug.Log("fail");
            }
        });
	}

	void Update () {

        if (paused)
            return;

        if (powerUpCountdown > 0)
            powerUpCountdown -= Time.deltaTime;
        else if (powerUpCountdown < 0)
        {
            powerUpCountdown = 0;
            aimer.SetActive(false);
            pointMultiplier = false;
            powerUpText.text = "";
        }

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

        ///////////////////////old computer controls

        if (Input.GetMouseButtonDown(0) && canMove)
        {
            //move in direction faced x distance
            targetPosition = transform.position + transform.up * 5.0f;

            mainCamera.SetFollow(false);

            canMove = false;
        }

        ///////////////////////

        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.12f);

        if ((transform.position - targetPosition).magnitude < 0.015f && canMove == false)
        {
            if (touchNode)
            {
                ReachNode();
                UpdateScore();
            }
            else
                Restart();
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
        if (paused)
            return;

        if (other.tag == "Node")
        {
            targetPosition = other.transform.position - new Vector3(0, 0, 5);

            touchNode = true;

            currentNode = other.gameObject;
        }
        else if (other.tag == "Enemy")
        {
            Restart();
            return;
        }
        else if (other.tag == "PowerUp")
        {
            if (other.GetComponent<PowerUpScript>().powerUpType == 0)
            {
                EnableAimer();
            }
            else
            {
                EnablePointMulitplier();
            }

            Destroy(other.gameObject);
        }
    }

    void Restart()
    {
        //need proper restart rather than loadscene
        //SceneManager.LoadScene(0, LoadSceneMode.Single);

        Pause(true);

        scoreScript.EndGame();
    }

    void UpdateScore()
    {
            score++;

            if (pointMultiplier)
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

    void Pause(bool isPaused)
    {
        paused = isPaused;

        gameController.Pause(isPaused);
    }

    public void EnterMenu()
    {
        scoreScript.Menu(true);
    }

    public void Reset()
    {
        Pause(false);
        scoreScript.Menu(false);

        gameController.Restart();
        scoreScript.StartGame();

        transform.position = Vector3.zero;
        targetPosition = Vector3.zero;

        if (score > bestScore)
        {
            bestScore = score;
            bestScoreText.text = bestScore.ToString();

            PlayerPrefs.SetInt("bestScore", bestScore);
        }

        canMove = true;
        rotateSpeed = 100.0f;
        touchNode = false;
        rotationDir = 1.0f;
        score = 0;
        scoreText.text = score.ToString();
        mainCamera.Restart();

        aimer.SetActive(false);
        pointMultiplier = false;
        powerUpCountdown = 0;
        powerUpText.text = "";
    }

    void EnableAimer()
    {
        aimer.SetActive(true);

        powerUpCountdown = powerUpTime;

        powerUpText.text = "AIM ASSIST";
    }

    void EnablePointMulitplier()
    {
        pointMultiplier = true;

        powerUpCountdown = powerUpTime;

        powerUpText.text = "POINT MULTIPLIER";
    }
}
