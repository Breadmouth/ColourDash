using UnityEngine;
using System.Collections;

public class NodeScript : MonoBehaviour {

    Vector3 targetScale;

    GameObject nextNode;

    NodeSemiScript child1;
    NodeSemiScript child2;

    Color randomColour;

    int rgbDecider;

    bool paused = false;

	void Start () {

        targetScale = new Vector3(0.8f, 0.8f, 0.8f);

        child1 = GetComponentsInChildren<NodeSemiScript>()[0];
        child2 = GetComponentsInChildren<NodeSemiScript>()[1];

        rgbDecider = (int)Random.Range(0, 2.99f);

        if (rgbDecider == 0)
            randomColour = new Color(Random.Range(0.8f, 1), Random.Range(0.4f, 0.8f), Random.Range(0.4f, 0.8f));
        else if (rgbDecider == 1)
            randomColour = new Color(Random.Range(0.4f, 0.8f), Random.Range(0.8f, 1), Random.Range(0.4f, 0.8f));
        else
            randomColour = new Color(Random.Range(0.4f, 0.8f), Random.Range(0.4f, 0.8f), Random.Range(0.8f, 1));

        child1.SetColor(randomColour);
        child2.SetColor(randomColour);

        GetComponent<Renderer>().material.color = randomColour * 0.35f;
	}

	void Update () {

        if (paused)
            return;

        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 0.05f);

	}

    public void ActivateNode()
    {
        child1.Die();
        child2.Die();

        targetScale = new Vector3(0.3f, 0.3f, 0.3f);

        GetComponent<Collider2D>().enabled = false;

        nextNode.GetComponent<NodeScript>().BeginSpin();
        //change node colour?
    }

    public void SetnextNode(GameObject node)
    {
        nextNode = node;
    }

    public void BeginSpin()
    {
        child1.BeginSpin();
        child2.BeginSpin();
    }

    public void Pause(bool isPaused)
    {
        paused = isPaused;

        child1.Pause(paused);
        child2.Pause(paused);
    }
}
