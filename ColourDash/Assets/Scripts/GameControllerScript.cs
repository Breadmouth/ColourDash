using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class GameControllerScript : MonoBehaviour {

    //Have 5 nodes at one time

    public GameObject nodePrefab;
    GameObject[] nodeArray;

    float nextNodeY = 6.0f;
    int nextNodeID = 4;

    void Awake()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            // enables saving game progress.
            .EnableSavedGames()
            // require access to a player's Google+ social graph to sign in
            .RequireGooglePlus()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();

        nodeArray = new GameObject[5];

        nodeArray[2] = GameObject.Find("Node");

        GameObject newNode = (GameObject)Instantiate(nodePrefab, new Vector3(Random.Range(-1.5f, 1.5f), nextNodeY, 5.0f), transform.rotation);
        nextNodeY += 3.0f;

        nodeArray[3] = newNode;

        newNode = (GameObject)Instantiate(nodePrefab, new Vector3(Random.Range(-1.5f, 1.5f), nextNodeY, 5.0f), transform.rotation);
        nextNodeY += 3.0f;

        nodeArray[4] = newNode;

        nodeArray[2].GetComponent<NodeScript>().SetnextNode(nodeArray[3]);
        nodeArray[3].GetComponent<NodeScript>().SetLookPos(nodeArray[2].transform.position);
        nodeArray[3].GetComponent<NodeScript>().SetnextNode(nodeArray[4]);
        nodeArray[4].GetComponent<NodeScript>().SetLookPos(nodeArray[3].transform.position);
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
            nodeArray[0].GetComponent<NodeScript>().SetLookPos(nodeArray[4].transform.position);

            nextNodeID = 0;
        }
        else
        {
            Destroy(nodeArray[nextNodeID + 1]);
            nodeArray[nextNodeID + 1] = newNode;

            nodeArray[nextNodeID].GetComponent<NodeScript>().SetnextNode(nodeArray[nextNodeID + 1]);
            nodeArray[nextNodeID + 1].GetComponent<NodeScript>().SetLookPos(nodeArray[nextNodeID].transform.position);

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
        nodeArray[3].GetComponent<NodeScript>().SetLookPos(nodeArray[2].transform.position);
        nodeArray[3].GetComponent<NodeScript>().SetnextNode(nodeArray[4]);
        nodeArray[4].GetComponent<NodeScript>().SetLookPos(nodeArray[3].transform.position);
    }
}
