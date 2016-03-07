using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    GameObject player;
    Vector3 relativeToPlayer;

    bool canFollow;

	void Start () {

        //object that we are following
        player = GameObject.Find("Player");

        //position relative to the object we are following
        relativeToPlayer = new Vector3(0.0f, 2.5f, -10.0f);
	}

	void Update () {
	
	}

    void LateUpdate()
    {
        if (canFollow)
        {
            //lerp towards the player position
            transform.position = Vector3.Lerp(transform.position, player.transform.position + relativeToPlayer, 0.15f);
        }
    }

    public void SetFollow(bool i)
    {
        canFollow = i;
    }
}
