using UnityEngine;
using System.Collections;

public class NodeScript : MonoBehaviour {

    Vector3 targetScale;

    GameObject nextNode;

    NodeSemiScript child1;
    NodeSemiScript child2;

    Color randomColour;

	void Start () {

        targetScale = new Vector3(0.7f, 0.7f, 0.7f);

        child1 = GetComponentsInChildren<NodeSemiScript>()[0];
        child2 = GetComponentsInChildren<NodeSemiScript>()[1];

        randomColour = new Color(Random.Range(0.2f, 1), Random.Range(0.2f, 1), Random.Range(0.2f, 1));

        child1.SetColor(randomColour);
        child2.SetColor(randomColour);
	}

	void Update () {

        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 0.05f);

	}

    public void ActivateNode()
    {
        child1.Die();
        child2.Die();

        targetScale = new Vector3(0.3f, 0.3f, 0.3f);

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
}
