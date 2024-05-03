using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class nodeData : MonoBehaviour
{
    public List<nodeData> connectedNodes;
    public List<edgeData> connectedEdges;
    public bool isVisited = false;
    public NodeGUI connectedGUIElement;
    public float shortestPathValue = 10000; //set to super large number so that this works with dijkstras aka the distance between this node and the starting node

    public void Start()
    {
        this.GetComponent<Renderer>().enabled = false;
        this.GetComponent<Collider>().enabled = false;
    }
    //Either append connected nodes or connected edges based on isNode flag
    public void appendStuff(GameObject[] stuff, bool isNode = true)
    {
        foreach (GameObject thing in stuff){
            if (isNode && (this.gameObject != thing))
            {
                if (!connectedNodes.Contains(thing.GetComponent<nodeData>()))
                {
                    nodeData temp = thing.GetComponent<nodeData>();
                    connectedNodes.Add(temp);
                }
            }
            else if (this.gameObject != thing)
            {
                if (!connectedEdges.Contains(thing.GetComponent<edgeData>()))
                {
                    edgeData temp = thing.GetComponent<edgeData>();
                    connectedEdges.Add(temp);
                }
            }
        }
    }

    //sort the list based off of the edge weights
    private void sortConnectedEdges()
    {
        connectedEdges.OrderBy(x => x.edgeWeight);
    }

    //given two nodes, determine which node is the node we are not at
    public nodeData getOutGoingNode(GameObject nodeOne, GameObject nodeTwo)
    {
        if (nodeOne == this.gameObject)
        {
            return nodeTwo.GetComponent<nodeData>();
        }
        return nodeOne.GetComponent<nodeData>();
    }

    //get the closest node from current node, if the node needs to be unvisited, check all nodes
    //until the shortest unvisted node is found, if none is found, return null
    public nodeData getClosestNode(bool shouldBeUnvisited = false)
    {
        nodeData returnVal = null;
        this.sortConnectedEdges();
        //this controled loop gets the lowest cost neighboring node that is unvisited. This could use a method extraction TODO
        if (shouldBeUnvisited)
        {
            edgeData spoof = new edgeData();
            spoof.edgeWeight = 10000000; //stupidly high so that spoof is never chosen
            spoof.isVisited = true;
            edgeData currentShortest = spoof;
            nodeData temp;
            foreach (edgeData edge in this.connectedEdges){
                temp = getOutGoingNode(edge.StartNode, edge.EndNode);
                if (temp.isVisited == false)
                {
                    if (returnVal == null)
                    {
                        returnVal = temp;
                    }
                    else if (edge.edgeWeight <= currentShortest.edgeWeight)
                    {
                        returnVal = temp;
                        currentShortest = edge;
                    }
                }
                
            }
            return returnVal;
        }
        //this just returns the nearest node, regardless of whether or not it is visited
        else
        {
            edgeData temp = this.connectedEdges[0];
            returnVal = getOutGoingNode(temp.StartNode, temp.EndNode);
        }
        return returnVal;
    }

    public void visitNode()
    {
        isVisited = true;
        connectedGUIElement.setVisited();
    }

    public void setActive()
    {
        isVisited = true;
        connectedGUIElement.setActive();
    }

    public void setEnd()
    {
        connectedGUIElement.setEnd();
    }

    public void setStart()
    {
        connectedGUIElement.setStart();
    }

    public void setCandidate()
    {
        connectedGUIElement.setCandidate();
    }

    public edgeData getEdgeConnectedToNodeX(nodeData nodeToConnect)
    {
        foreach(edgeData edge in connectedEdges)
        {
            if (edge.StartNode == nodeToConnect.gameObject && edge.EndNode == this.gameObject)
            {
                return edge;
            }
            if (edge.StartNode == this.gameObject && edge.EndNode == nodeToConnect.gameObject)
            {
                return edge;
            }
        }
        return null;
    }
}
