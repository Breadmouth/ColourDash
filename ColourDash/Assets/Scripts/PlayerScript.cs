using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    bool canMove = true;
    Vector3 targetPosition = Vector3.zero;

    CameraScript mainCamera;

    GameControllerScript gameController;

    float rotateSpeed = 100.0f;

    bool touchNode = false;

	void Start () {

        mainCamera = GameObject.Find("MainCamera").GetComponent<CameraScript>();
        gameController = GameObject.Find("MainCamera").GetComponent<GameControllerScript>();

	}

	void Update () {
	

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

            touchNode = false;

            transform.position = targetPosition;

            canMove = true;
            mainCamera.SetFollow(true);
        }

        if (canMove)
        {
            //rotate player
            transform.Rotate(transform.forward, rotateSpeed * Time.deltaTime);
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

            other.GetComponent<NodeScript>().ActivateNode();

            gameController.CreateNewNode();

            touchNode = true;
        }
        else if (other.tag == "Enemy")
        {
            Restart();
        }
    }

    void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
