using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    GameObject player;
    Vector3 relativeToPlayer;
    Camera myCamera;

    Quaternion targetRotation;

    float targetOrtho = 5.0f;

    bool canFollow;

	void Start () {

        targetRotation = Quaternion.Euler(Vector3.zero);

        //object that we are following
        player = GameObject.Find("Player");

        //position relative to the object we are following
        relativeToPlayer = new Vector3(0.0f, 3.2f, -10.0f);

        myCamera = GetComponent<Camera>();
	}

	void Update () {
	
	}

    void LateUpdate()
    {

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.1f);

        if (canFollow)
        {
            relativeToPlayer = (transform.up * 3.2f) + new Vector3(0, 0, -10);
            //lerp towards the player position
            transform.position = Vector3.Lerp(transform.position, player.transform.position + relativeToPlayer, 0.15f);
        }

        myCamera.orthographicSize = Mathf.Lerp(myCamera.orthographicSize, targetOrtho, 0.25f);
    }

    public void SetFollow(bool i)
    {
        canFollow = i;
    }

    public void Restart()
    {
        canFollow = true;
        transform.position = player.transform.position + relativeToPlayer;
    }

    public void ScreenBounce()
    {
        myCamera.orthographicSize = 4.5f;
    }

    public void LookTowards(GameObject other)
    {
        float deltaY = -other.transform.position.x + player.transform.position.x;
        float deltaX = other.transform.position.y - player.transform.position.y;

        float angleDeg = Mathf.Atan2(deltaY, deltaX) * 180.0f / Mathf.PI;

        Quaternion oldRot = transform.rotation;

        transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.Rotate(transform.forward, angleDeg / 3.0f);

        targetRotation = transform.rotation;
        transform.rotation = oldRot;
    }
}
