using UnityEngine;
using System.Collections;

public class PowerUpScript : MonoBehaviour {

    float rotateSpeed = 40.0f;

    public int powerUpType;

	void Start () {

        powerUpType = (int)Random.Range(0, 1.99f);

	}

	void Update () {

        transform.Rotate(transform.forward, 40.0f * Time.deltaTime);
	}

    public void Active(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
