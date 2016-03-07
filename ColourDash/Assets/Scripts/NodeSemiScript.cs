using UnityEngine;
using System.Collections;

public class NodeSemiScript : MonoBehaviour {

    public bool rotateRight;

    bool moving = false;

    Color targetColour;

    SpriteRenderer myRenderer;

	void Start () {

	}

    void Awake()
    {
        myRenderer = transform.GetComponent<SpriteRenderer>();

        targetColour = myRenderer.color;
    }

	void Update () {
	
        if (moving)
        {
            if (rotateRight)
            {
                transform.Rotate(transform.forward, 10.0f * Time.deltaTime);
            }
            else
            {
                transform.Rotate(transform.forward, -10.0f * Time.deltaTime);
            }
        }

        myRenderer.color = Color.Lerp(myRenderer.color, targetColour, 0.05f);

	}

    public void Die()
    {
        GetComponent<Collider2D>().enabled = false;

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
}
