using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class KruskalsAlgorithm : MonoBehaviour
{
    /*
    This is an implementation of Kruskal's. It should run in two steps, the first will sort all the edges based on 
    the edge weights. The second step is to recursively select edges from lowest to highest in a manner that WON'T create 
    cycles until there are no unconnected nodes. TODO elaborate on implementation.
    */
    public algorithmControler thisAlgorithmsControler;
    public List<nodeData> visitedNodes;
    public nodeData[] nodes;
    public edgeData[] edges;
    public List<edgeData> minimumSpanningTree;
    public List<nodeData> minimumSpanningTreeNodes;
    public nodeData currentNode;
    public edgeData currentEdge;
    public int edgePointer = 0; //the index of the edge being currently worked on;
    private int numberOfIterations = 1; // N (number of nodes) - 1 (set to one to not trigger the base case immediately)
    private bool first = true;
    public bool done = false;
    private bool running = false;
    private bool stopAlgorithm = false;

    // general Setup function before the algorithm is ran
    void setUp()
    {
        nodes = thisAlgorithmsControler.nodeList;
        thisAlgorithmsControler.currentNode = currentNode;
        edges = thisAlgorithmsControler.edgeList;
        edges = edges.OrderBy(x => x.edgeWeight).ToArray();
        currentEdge = edges[0];
        numberOfIterations = nodes.Length - 1; //The minimum spanning tree is always N-1 in number of edges
        Debug.Log("Set Up Complete");
    }

    //helper function to determine the next unvisited node in DFS
    private nodeData getNextNodeHelper(List<nodeData> visitedNodes, nodeData startNode)
    {
        nodeData returnVal = null;
        edgeData spoof = new edgeData();
        spoof.edgeWeight = 10000000; //stupidly high so that spoof is never chosen
        spoof.isVisited = true;
        edgeData currentShortest = spoof;
        nodeData temp;
        foreach (edgeData edge in startNode.connectedEdges)
        {
            if (minimumSpanningTree.Contains(edge))
            {
                temp = startNode.getOutGoingNode(edge.StartNode, edge.EndNode);
                if (!visitedNodes.Contains(temp))
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
        }
        return returnVal;
    }
    
    //Given two nodes, return true if a cycle present, false otherwise, using DFS
    bool isReachable(nodeData startNode, nodeData endNode) 
    {
        nodeData dfsNode = startNode;
        List<nodeData> visitedNodes = new List<nodeData>();
        visitedNodes.Add(dfsNode);
        Stack<nodeData> dfsStack = new Stack<nodeData>();
        dfsStack.Push(dfsNode);
        while (dfsNode != endNode && dfsStack.Count != 0){
            //Start of DFS Step
            visitedNodes.Add(dfsNode);
            nodeData temp = getNextNodeHelper(visitedNodes, dfsNode);
            if (temp == null) //dead end
            {
                dfsStack.Pop();
                if (dfsStack.Count != 0)
                {
                    dfsNode = dfsStack.Peek();
                }
            }
            else //no dead end
            {
                dfsNode = temp;
                dfsStack.Push(temp);
            }
            //End of DFS step
        }
        if (dfsNode == endNode)
        {
            Debug.Log("Found Cycle");
            return true; //There is a cycle
        }
        Debug.Log("No Cycle Found");
        return false; //The stack emptied and we were not able to complete DFS to the desired node
    }

    //main loop of Kruskals
    IEnumerator algorithmStep() // this function needs to be run (numberOfIterations) times
    {
        running = true;
        while (true)
        {
            nodeData tempOne = edges[edgePointer].StartNode.GetComponent<nodeData>();
            nodeData tempTwo = edges[edgePointer].EndNode.GetComponent<nodeData>();
            if (!(isReachable(tempOne, tempTwo))) //would adding this edge create a cycle
            {
                // add the edge to our spanning tree and increase our edge pointer
                minimumSpanningTree.Add(edges[edgePointer]);
                if (!minimumSpanningTreeNodes.Contains(tempOne))
                {
                    minimumSpanningTreeNodes.Add(tempOne);
                }
                if (!minimumSpanningTreeNodes.Contains(tempTwo))
                {
                    minimumSpanningTreeNodes.Add(tempTwo);
                }
                currentNode = tempTwo;
                thisAlgorithmsControler.currentNode = currentNode;
                tempOne.visitNode();
                tempTwo.visitNode();
                tempOne.isVisited = true;
                tempTwo.isVisited = true;
                edges[edgePointer].visitEdge();
                edges[edgePointer].isVisited = true;
                edgePointer++;
                break;
            }
            else
            {
                edgePointer++;
            }
        }
        running = false;
        yield return null;
    }
    //color spanning tree on completion
    void finalize()
    {
        foreach (nodeData node in minimumSpanningTreeNodes)
        {
            node.setActive();
        }
        foreach (edgeData edge in minimumSpanningTree)
        {
            edge.setActive();
        }
        stopAlgorithm = true;
    }

    // Control of the algorithm
    void Update()
    {
        if (!stopAlgorithm)
        {
            if (thisAlgorithmsControler.ready && !running && !done)
            {
                if (numberOfIterations <= 0)
                {
                    done = true;
                }
                else if (first)
                {
                    setUp();
                    first = false;
                }
                else if (!done)
                {
                    StartCoroutine(algorithmStep());
                    numberOfIterations--;
                }
                thisAlgorithmsControler.ready = false;
            }
            else if (thisAlgorithmsControler.ready && done && !running)
            {
                finalize();
            }
        }      
    }
}
