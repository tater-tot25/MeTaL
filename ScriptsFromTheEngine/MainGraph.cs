using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGraph : MonoBehaviour
{
    public GameObject[] Edges;
    public GameObject[] Nodes;
    public GameObject nodePrefab;
    public GameObject edgePrefab;
    Dictionary<GameObject, GameObject> NodeGUINodePairs = new Dictionary<GameObject, GameObject>();
    Dictionary<GameObject, GameObject> EdgeGUIEdgePairs = new Dictionary<GameObject, GameObject>();


    //calls the node with a list of edges and nodes to check if they are already contained in the nodeData object
    public void updateNodeDataHelper(GameObject node, GameObject[] edgesToUpdate, GameObject[] nodesToUpdate)
    {
        node.GetComponent<nodeData>().appendStuff(nodesToUpdate);
        node.GetComponent<nodeData>().appendStuff(edgesToUpdate, false);
    }
    //called during edge iteration in Start() so that the node can get a list of connected edges and nodes
    public void updateNodeData(edgeData edge)
    {
        GameObject nodeOne = edge.StartNode;
        GameObject nodeTwo = edge.EndNode;
        GameObject[] nodesToUpdate = { nodeOne, nodeTwo };
        GameObject[] edgesToUpdate = { edge.gameObject };
        updateNodeDataHelper(nodeOne, edgesToUpdate, nodesToUpdate);
        updateNodeDataHelper(nodeTwo, edgesToUpdate, nodesToUpdate);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Allows for the Graph Class to access the GUI portion via the dictionary after auto instantiating them
        foreach (GameObject node in Nodes)
        {
            Vector3 tempPosition = node.transform.position;
            GameObject tempNode = GameObject.Instantiate(nodePrefab);
            tempNode.transform.position = tempPosition;
            node.GetComponent<nodeData>().connectedGUIElement = tempNode.GetComponent<NodeGUI>();
            NodeGUINodePairs.Add(node, tempNode);
        }
        foreach (GameObject edge in Edges)
        {
            GameObject tempEdge = GameObject.Instantiate(edgePrefab);
            tempEdge.transform.position = edge.transform.position;
            EdgeGUI tempGUI = tempEdge.GetComponent<EdgeGUI>();
            updateNodeData(edge.GetComponent<edgeData>()); //for every edge in the graph, upon start of program, check to see if the connected nodes contain eachother
            tempGUI.startNode = edge.GetComponent<edgeData>().StartNode;
            tempGUI.endNode = edge.GetComponent<edgeData>().EndNode;
            edge.GetComponent<edgeData>().connectedGUIElement = tempGUI;
            tempGUI.setUpLine();
            EdgeGUIEdgePairs.Add(edge, tempEdge);
        }
    }

    public GameObject getNodeItem(GameObject node)
    {
        return this.NodeGUINodePairs[node];
    }

    public NodeGUI getNodeGUIItem(GameObject node)
    {
        return this.NodeGUINodePairs[node].GetComponent<NodeGUI>();
    }

    public EdgeGUI getEdgeGUIItem(GameObject edge)
    {
        return this.EdgeGUIEdgePairs[edge].GetComponent<EdgeGUI>();
    }
    
    public GameObject getEdgeItem(GameObject edge)
    {
        return this.NodeGUINodePairs[edge];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
