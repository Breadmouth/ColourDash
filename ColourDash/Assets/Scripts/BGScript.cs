using UnityEngine;
using System.Collections;

public class BGScript : MonoBehaviour {

    public GameObject player;

    Vector3 originalPos;

	// Use this for initialization
	void Start () {

        originalPos = transform.position;

	}
	
	// Update is called once per frame
	void Update () {
	
        if (player.transform.position.y - transform.position.y > 30.0f)
        {
            transform.position += new Vector3(0, 21.5f * 3, 0);
        }

	}

    public void Reset()
    {
        transform.position = originalPos;
    }
}
