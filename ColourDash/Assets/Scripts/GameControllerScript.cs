using UnityEngine;
using System.Collections;

public class GameControllerScript : MonoBehaviour {

    //Have 5 nodes at one time

    public GameObject nodePrefab;

    float nextNodeY = 6.0f;

    GameObject[] nodeArray;

    int nextNodeID = 4;

	void Start () {

        nodeArray = new GameObject[5];

        nodeArray[2] = GameObject.Find("Node");

        GameObject newNode = (GameObject)Instantiate(nodePrefab, new Vector3(Random.Range(-1.5f, 1.5f), nextNodeY, 5.0f), transform.rotation);
        nextNodeY += 3.0f;

        nodeArray[3] = newNode;

        newNode = (GameObject)Instantiate(nodePrefab, new Vector3(Random.Range(-1.5f, 1.5f), nextNodeY, 5.0f), transform.rotation);
        nextNodeY += 3.0f;

        nodeArray[4] = newNode;

        nodeArray[2].GetComponent<NodeScript>().SetnextNode(nodeArray[3]);
        nodeArray[3].transform.LookAt(nodeArray[2].transform);
        nodeArray[3].transform.Rotate(transform.forward, 270);
        nodeArray[3].GetComponent<NodeScript>().SetnextNode(nodeArray[4]);
        nodeArray[4].transform.LookAt(nodeArray[3].transform);
        nodeArray[4].transform.Rotate(transform.forward, 270);
	}

    public void CreateNewNode()
    {
        GameObject newNode = (GameObject)Instantiate(nodePrefab, new Vector3(Random.Range(-1.5f, 1.5f), nextNodeY, 5.0f), transform.rotation);
        nextNodeY += 3.0f;

        if (nextNodeID == 4)
        {
            Destroy(nodeArray[0]);
            nodeArray[0] = newNode;

            nodeArray[4].GetComponent<NodeScript>().SetnextNode(nodeArray[0]);
            nodeArray[0].transform.LookAt(nodeArray[4].transform);
            nodeArray[0].transform.Rotate(transform.forward, 270);

            nextNodeID = 0;
        }
        else
        {
            Destroy(nodeArray[nextNodeID + 1]);
            nodeArray[nextNodeID + 1] = newNode;

            nodeArray[nextNodeID].GetComponent<NodeScript>().SetnextNode(nodeArray[nextNodeID + 1]);
            nodeArray[nextNodeID + 1].transform.LookAt(nodeArray[nextNodeID].transform);
            nodeArray[nextNodeID + 1].transform.Rotate(transform.forward, 270);

            nextNodeID++;
        }
    }

    public void Pause(bool isPaused)
    {
        for (int i = 0; i < 5; ++i)
        {
            if (nodeArray[i] != null)
                nodeArray[i].GetComponent<NodeScript>().Pause(isPaused);
        }
    }

    public void Restart()
    {
        for (int i = 4; i >= 0; --i)
        {
            Destroy(nodeArray[i]);
        }

        CreateInitialNodes();
    }

    void CreateInitialNodes()
    {
        nextNodeID = 4;
        nextNodeY = 3.0f;

        GameObject newNode = (GameObject)Instantiate(nodePrefab, new Vector3(0, nextNodeY, 5.0f), transform.rotation);
        nextNodeY += 3.0f;

        nodeArray[2] = newNode;

        newNode = (GameObject)Instantiate(nodePrefab, new Vector3(Random.Range(-1.5f, 1.5f), nextNodeY, 5.0f), transform.rotation);
        nextNodeY += 3.0f;

        nodeArray[3] = newNode;

        newNode = (GameObject)Instantiate(nodePrefab, new Vector3(Random.Range(-1.5f, 1.5f), nextNodeY, 5.0f), transform.rotation);
        nextNodeY += 3.0f;

        nodeArray[4] = newNode;

        nodeArray[2].transform.rotation = Quaternion.Euler(new Vector3(90, 180, 0));
        nodeArray[2].GetComponent<NodeScript>().SetnextNode(nodeArray[3]);
        nodeArray[3].transform.LookAt(nodeArray[2].transform);
        nodeArray[3].transform.Rotate(transform.forward, 270);
        nodeArray[3].GetComponent<NodeScript>().SetnextNode(nodeArray[4]);
        nodeArray[4].transform.LookAt(nodeArray[3].transform);
        nodeArray[4].transform.Rotate(transform.forward, 270);
    }
}
