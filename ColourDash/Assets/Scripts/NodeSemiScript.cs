﻿using UnityEngine;
using System.Collections;

public class NodeSemiScript : MonoBehaviour {

    Color targetColour;

    SpriteRenderer myRenderer;

    public bool rotateRight;

    bool moving = false;
    bool paused = false;

	void Start () {

	}

    void Awake()
    {
        myRenderer = transform.GetComponent<SpriteRenderer>();

        targetColour = myRenderer.color;
    }

	void Update () {

        if (paused)
            return;

        if (moving)
        {
            if (rotateRight)
            {
                transform.Rotate(transform.forward, 25.0f * Time.deltaTime);
            }
            else
            {
                transform.Rotate(transform.forward, -25.0f * Time.deltaTime);
            }
        }

        myRenderer.color = Color.Lerp(myRenderer.color, targetColour, 0.05f);

	}

    public void Die()
    {
        transform.tag = "Player";

        targetColour = Color.clear;
    }

    public void BeginSpin()
    {
        moving = true;
    }

    public void SetColor(Color newColor)
    {
        targetColour = newColor;
        myRenderer.color = newColor;
    }

    public void Pause(bool isPaused)
    {
        paused = isPaused;
    }
}
