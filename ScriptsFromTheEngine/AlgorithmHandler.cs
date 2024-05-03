using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
public class AlgorithmHandler : MonoBehaviour
{
    public GameObject algToRun; //for now, we will assign the algorithm in the editor
    public algorithmControler algControl;
    public nodeData startNode;
    public nodeData endNode;
    public MainGraph graphData;
    public edgeData[] edges;
    public nodeData[] nodes;
    private nodeData lastUpdatedNode = null;
    public GameObject playerObject;
    public float distanceThreshold;
    public XRController rightHand;
    public InputHelpers.Button button;
    public InputHelpers.Button menuButton;
    public XRController leftHand;
    public InputHelpers.Button skipButton;
    private bool waitBool = false;
    public bool skip = false;
    public bool randomized = true;
    public nodeData forcedStartingNode;
    public nodeData forcedEndNode;
    // Start is called before the first frame update
    void Start()
    {
        System.Random rand = new System.Random();
        if (crossSceneDataHandler.algorithmToRun == null) //if there was no algorithm selected run dijkstras, else pick the one the user selected
        {
            algToRun = GameObject.Find("basicDijkstras");
        }
        else
        {
            algToRun = GameObject.Find(crossSceneDataHandler.algorithmToRun);
        }
        algControl = algToRun.GetComponent<algorithmControler>();
        int edgeLength = graphData.Edges.Length;
        int nodeLength = graphData.Nodes.Length;
        edges = new edgeData[edgeLength];
        nodes = new nodeData[nodeLength];
        for (int i = 0; i < edgeLength; i++)
        {
            edges[i] = graphData.Edges[i].GetComponent<edgeData>();
        }
        for (int i = 0; i < nodeLength; i++)
        {
            nodes[i] = graphData.Nodes[i].GetComponent<nodeData>();
        }
        algControl.nodeList = nodes;
        algControl.edgeList = edges;
        algControl.numOfNodes = nodeLength;
        //select start and end nodes here
        int nodeOneSelect = rand.Next(nodeLength);
        algControl.startingNode = nodes[nodeOneSelect];
        nodeOneSelect = rand.Next(nodeLength);
        algControl.endingNode = nodes[nodeOneSelect];
        float distanceBetweenNodes = Vector3.Distance(algControl.endingNode.transform.position,
                                                      algControl.startingNode.transform.position);
        while (distanceBetweenNodes < distanceThreshold && randomized) //ensure that the starting node cannot equal the ending node
        {                                               //and that the nodes are adequately spaced
            nodeOneSelect = rand.Next(nodeLength);
            algControl.endingNode = nodes[nodeOneSelect];
            distanceBetweenNodes = Vector3.Distance(algControl.endingNode.transform.position,
                                                      algControl.startingNode.transform.position);
        }
        if (!randomized)
        {
            algControl.startingNode = forcedStartingNode;
            algControl.endingNode = forcedEndNode;
        }
        //end of node selection
    }

    public IEnumerator wait()
    {
        yield return new WaitForSeconds(0.5f);
        waitBool = false;
    }

    // Update is called once per frame
    void Update() //Update controls all player teleportation and button inputs!!!
    {
        if (algControl.currentNode != null && algControl.currentNode != lastUpdatedNode) //Teleport the player to the current node
        {
            Vector3 teleNode = algControl.currentNode.transform.position;
            Vector3 newPlayerLocation = new Vector3(teleNode.x, -0.809f, teleNode.z); //split this so if we need to add an offset, its easier
            playerObject.transform.position = newPlayerLocation;
            lastUpdatedNode = algControl.currentNode;
        }
        bool pressed;
        bool menuPressed;
        bool skipPressed;
        leftHand.inputDevice.IsPressed(skipButton, out skipPressed);
        rightHand.inputDevice.IsPressed(button, out pressed);
        rightHand.inputDevice.IsPressed(menuButton, out menuPressed);
        if (skipPressed)
        {
            skip = true;
        }
        else
        {
            skip = false; //make sure this is false before build
        }
        if ((pressed || Input.GetKeyDown(KeyCode.Space)) && !waitBool)
        {
            waitBool = true;
            StartCoroutine(wait());
            if (!algControl.ready == true)
            {
                algControl.ready = true;              
            }
        }
        if (skip)
        {
            if (!algControl.ready == true)
            {
                algControl.ready = true;
            }
        }
        if (menuPressed)
        {
            SceneManager.LoadScene("optionScreen");
        }
    }
}
