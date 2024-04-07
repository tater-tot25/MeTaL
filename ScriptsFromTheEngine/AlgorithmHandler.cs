using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            algToRun = crossSceneDataHandler.algorithmToRun;
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
        //randomize nodes here
        int nodeOneSelect = rand.Next(nodeLength);
        algControl.startingNode = nodes[nodeOneSelect];
        nodeOneSelect = rand.Next(nodeLength);
        algControl.endingNode = nodes[nodeOneSelect];
        float distanceBetweenNodes = Vector3.Distance(algControl.endingNode.transform.position,
                                                      algControl.startingNode.transform.position);
        while (algControl.endingNode == algControl.startingNode && distanceBetweenNodes < distanceThreshold) //ensure that the starting node cannot equal the ending node
        {                                                                                                    //and that the nodes are adequately spaced
            nodeOneSelect = rand.Next(nodeLength);
            algControl.endingNode = nodes[nodeOneSelect];
            distanceBetweenNodes = Vector3.Distance(algControl.endingNode.transform.position,
                                                      algControl.startingNode.transform.position);
        }
        //end of node randomization
    }

    // Update is called once per frame
    void Update()
    {
        if (algControl.currentNode != null && algControl.currentNode != lastUpdatedNode) //Teleport the player to the current node
        {
            Vector3 teleNode = algControl.currentNode.transform.position;
            Vector3 newPlayerLocation = new Vector3(teleNode.x, teleNode.y, teleNode.z); //split this so if we need to add an offset, its easier
            playerObject.transform.position = newPlayerLocation;
            lastUpdatedNode = algControl.currentNode;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!algControl.ready == true)
            {
                algControl.ready = true;              
            }
        }
    }
}
