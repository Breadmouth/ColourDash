using UnityEngine;
using System.Collections;

public class PowerUpScript : MonoBehaviour {

    float rotateSpeed = 120.0f;

    public int powerUpType;

    bool pause = false;

	void Start () {

        powerUpType = (int)Random.Range(0, 1.99f);

	}

	void Update () {

        if (!pause)
            transform.Rotate(transform.forward, rotateSpeed * Time.deltaTime);
	}

    public void Active(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void Pause(bool isPaused)
    {
        pause = isPaused;
    }
}
