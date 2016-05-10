using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;

public class PlayerScript : MonoBehaviour {

    int firstDeath;
    int powerUpCounter = 0;

    Vector3 targetPosition = Vector3.zero;

    CameraScript mainCamera;

    GameControllerScript gameController;
    BGControllerScript bgController;

    GameObject currentNode;
    GameObject aimer;

    ScoreScript scoreScript;

    Text scoreText;
    Text bestScoreText;
    Text powerUpText;

    bool touchNode = false;
    bool canMove = true;
    bool paused = false;
    bool pointMultiplier = false;

    float rotationDir = 1.0f;
    float powerUpTime = 10.0f;
    float multiplierCountdown = 0.0f;
    float aimerCountdown = 0.0f;
    float rotateSpeed = 180.0f;

    int score = 0;
    int bestScore = 0;

	void Start () {

        mainCamera = GameObject.Find("MainCamera").GetComponent<CameraScript>();
        gameController = GameObject.Find("MainCamera").GetComponent<GameControllerScript>();
        bgController = GameObject.Find("BGController").GetComponent<BGControllerScript>();

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

        if (PlayerPrefs.HasKey("firstDeath"))
        {
            firstDeath = PlayerPrefs.GetInt("FirstDeath");
        }

        scoreScript.Menu(true);

        Pause(true);

        // authenticate user:
        Social.localUser.Authenticate((bool success) => {
            // handle success or failure
            if (success)
            {}
        });
	}

	void Update () {

        if (paused)
            return;

        if (multiplierCountdown > 0)
            multiplierCountdown -= Time.deltaTime;
        else if (multiplierCountdown < 0)
        {
            multiplierCountdown = 0;
            pointMultiplier = false;
            if (aimerCountdown <= 0)
                powerUpText.text = "";
        }
        if (aimerCountdown > 0)
            aimerCountdown -= Time.deltaTime;
        else if (aimerCountdown < 0)
        {
            aimerCountdown = 0;
            aimer.SetActive(false);
            if (multiplierCountdown <= 0)
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

            powerUpCounter++;

            scoreScript.FlashWhite();
        }
    }

    public void Restart()
    {
        if (firstDeath == 0)
        {
            Social.ReportProgress("CgkIlMWJ8uoBEAIQAQ", 100.0f, (bool success) =>
            {
                // handle success or failure
            });

            firstDeath = 1;
            PlayerPrefs.SetInt("firstDeath", 1);
        }

        scoreScript.FlashRed();

        Pause(true);

        powerUpText.text = "";

        scoreScript.EndGame();
    }

    void UpdateScore()
    {
        score++;

        if (pointMultiplier)
            score++;

        scoreText.text = score.ToString();

        scoreScript.BounceText();
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

        //mainCamera.ScreenBounce();
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
        bgController.Reset();
        scoreScript.StartGame();

        transform.position = Vector3.zero;
        targetPosition = Vector3.zero;

        if (score > bestScore)
        {
            bestScore = score;
            bestScoreText.text = bestScore.ToString();

            PlayerPrefs.SetInt("bestScore", bestScore);

            Social.ReportScore(bestScore, "CgkIlMWJ8uoBEAIQBg", (bool success) =>
            {
                // handle success or failure
            });

            if (score >= 10)
            {
                Social.ReportProgress("CgkIlMWJ8uoBEAIQAg", 100.0f, (bool success) =>
                {
                    // handle success or failure
                });
            }
            if (score >= 50)
            {
                Social.ReportProgress("CgkIlMWJ8uoBEAIQAw", 100.0f, (bool success) =>
                {
                    // handle success or failure
                });
            }
            if (score >= 100)
            {
                Social.ReportProgress("CgkIlMWJ8uoBEAIQBA", 100.0f, (bool success) =>
                {
                    // handle success or failure
                });
            }
        }

        if (powerUpCounter >= 3)
        {
            Social.ReportProgress("CgkIlMWJ8uoBEAIQBQ", 100.0f, (bool success) =>
            {
                // handle success or failure
            });
        }
        if (powerUpCounter >= 5)
        {
            Social.ReportProgress("CgkIlMWJ8uoBEAIQCA", 100.0f, (bool success) =>
            {
                // handle success or failure
            });
        }

        powerUpCounter = 0;
        canMove = true;
        rotateSpeed = 120.0f;
        touchNode = false;
        rotationDir = 1.0f;
        score = 0;
        scoreText.text = score.ToString();
        mainCamera.Restart();

        aimer.SetActive(false);
        pointMultiplier = false;
        aimerCountdown = 0;
        multiplierCountdown = 0;
    }

    void EnableAimer()
    {
        aimer.SetActive(true);

        aimerCountdown = powerUpTime;

        powerUpText.text = "AIM ASSIST";
    }

    void EnablePointMulitplier()
    {
        pointMultiplier = true;

        multiplierCountdown = powerUpTime;

        powerUpText.text = "POINT MULTIPLIER";
    }

    public void ShowLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }

    public void ShowAchievements()
    {
        Social.ShowAchievementsUI();
    }

    public void Login()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            // handle success or failure
            if (success)
            {}
        });
    }
}
