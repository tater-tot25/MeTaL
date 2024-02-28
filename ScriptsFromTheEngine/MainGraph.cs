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


    // Start is called before the first frame update
    void Start()
    {
        //Allows for the Graph Class to access the GUI portion via the dictionary after auto instantiating them
        foreach (GameObject node in Nodes)
        {
            GameObject tempNode = GameObject.Instantiate(nodePrefab);
            NodeGUINodePairs.Add(node, tempNode);
        }
        foreach (GameObject edge in Edges)
        {
            GameObject tempEdge = GameObject.Instantiate(edgePrefab);
            EdgeGUIEdgePairs.Add(edge, tempEdge);
        }
    }

    public GameObject getNodeItem(GameObject node)
    {
        return this.NodeGUINodePairs[node];
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
