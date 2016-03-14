using UnityEngine;
using System.Collections;

public class NodeScript : MonoBehaviour {

    Vector3 targetScale;

    GameObject nextNode;

    NodeSemiScript child1;
    NodeSemiScript child2;

    PowerUpScript powerUpChild;

    Color randomColour;

    CameraScript mainCamera;

    int rgbDecider;

    bool paused = false;

    void Awake()
    {
        targetScale = new Vector3(0.9f, 0.9f, 0.9f);

        child1 = GetComponentsInChildren<NodeSemiScript>()[0];
        child2 = GetComponentsInChildren<NodeSemiScript>()[1];

        powerUpChild = GetComponentInChildren<PowerUpScript>();

        mainCamera = GameObject.Find("MainCamera").GetComponent<CameraScript>();

        float childActive = Random.Range(0, 13.0f);
        if (childActive > 1.0f)
        {
            powerUpChild.Active(false);
        }

        rgbDecider = (int)Random.Range(0, 2.99f);

        if (rgbDecider == 0)
            randomColour = new Color(Random.Range(0.8f, 1), Random.Range(0.4f, 0.8f), Random.Range(0.4f, 0.8f));
        else if (rgbDecider == 1)
            randomColour = new Color(Random.Range(0.4f, 0.8f), Random.Range(0.8f, 1), Random.Range(0.4f, 0.8f));
        else
            randomColour = new Color(Random.Range(0.4f, 0.8f), Random.Range(0.4f, 0.8f), Random.Range(0.8f, 1));

        GetComponentInChildren<SpriteRenderer>().color = randomColour * 0.45f;
    }

	void Start () {

        child1.SetColor(randomColour);
        child2.SetColor(randomColour);
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

        if (powerUpChild != null && powerUpChild.enabled)
        {
            GameObject.Destroy(powerUpChild.gameObject);
        }

        targetScale = new Vector3(0.4f, 0.4f, 0.4f);

        nextNode.GetComponent<NodeScript>().BeginSpin();

        mainCamera.LookTowards(nextNode);
    }

    public void SetnextNode(GameObject node)
    {
        nextNode = node;
    }

    public void SetLookPos(Vector3 pos)
    {
        transform.LookAt(pos);
        transform.Rotate(Vector3.forward, 270);
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

        if (powerUpChild != null)
            powerUpChild.Pause(paused);
    }
}
