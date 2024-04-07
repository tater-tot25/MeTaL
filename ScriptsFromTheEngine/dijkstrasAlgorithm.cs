using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class dijkstrasAlgorithm : MonoBehaviour
{
    /*
    This is a demonstration of Dijkstras algorithm. It should have two stages, one where it goes through and updates every node.
    It will use the visited color for nodes that are fully updated and have been removed from the list. And it will also have a 
    color for actively updating nodes. The second stage will be reconstructing the path from the list of nodes with values. This 
    will have a seperate color to signify the correct path. I will represent this as a list of tuples with index 0 being the
    nodeData object and index 1 being the current shortest length to that node. The unvisited set is created at runtime through
    the algorithmControler and the GUI setup
    */
    public algorithmControler thisAlgorithmsControler;
    public List<nodeData> visitedNodes;
    public nodeData[] unvisitedNodes;
    public nodeData startingNode;
    public nodeData endingNode;
    public nodeData currentNode;
    public bool doneSearching = false;
    public bool done = false;
    private bool first = true;
    public List<nodeData> shortestPath;
    private bool blocked = false;

    // general Setup function before the algorithm is ran
    void setUp()
    {
        unvisitedNodes = thisAlgorithmsControler.nodeList;
        startingNode = thisAlgorithmsControler.startingNode;
        endingNode = thisAlgorithmsControler.endingNode;
        startingNode.shortestPathValue = 0; //initialize the starting node as having a pathlength of 0 since the distance of travel from n -> n is 0
        currentNode = startingNode;
        thisAlgorithmsControler.currentNode = currentNode;
        startingNode.setStart();
        endingNode.setEnd();
        Debug.Log("Set Up Complete");
        simulateMinHeapRestructure(); // not needed, but for the sake of consistency
    }

    //I don't feel like doing a whole min heap implementation to get ideal performance, hopefully this will suffice
    //Has the option to remove the visitedNode, then re-sort
    public void simulateMinHeapRestructure(bool removeCurrentNode = false)
    {
        Debug.Log("restructureHeap");
        if (removeCurrentNode)
        {
            unvisitedNodes = unvisitedNodes.Skip(1).ToArray(); //remove the first element since it should be the most recently accessed
        }
        unvisitedNodes = unvisitedNodes.OrderBy(x => x.shortestPathValue).ToArray();
    }

    //Search each non-visited neighboring node and update the path value if the path through this node is shorter.
    public IEnumerator searchNeighboringNodes(nodeData nodeToSearch)
    {
        blocked = true;
        foreach (nodeData node in nodeToSearch.connectedNodes)
        {
            if (!node.isVisited)
            {
                edgeData edgeToColor = nodeToSearch.getEdgeConnectedToNodeX(node);
                edgeToColor.setActive();
                node.setCandidate();
                float currentShortest = node.shortestPathValue;
                float candidatePath = nodeToSearch.shortestPathValue;
                candidatePath += nodeToSearch.getEdgeConnectedToNodeX(node).edgeWeight; //this is the value of the path to our neighboring node from the current node
                if (candidatePath < currentShortest)
                {
                    node.shortestPathValue = candidatePath;
                }
            }
        }
        nodeToSearch.isVisited = true;
        nodeToSearch.visitNode();
        visitedNodes.Add(nodeToSearch);
        simulateMinHeapRestructure(true);
        if (unvisitedNodes.Length == 0)
        {
            currentNode = null;
            thisAlgorithmsControler.currentNode = currentNode;
            doneSearching = true;
            blocked = false;
            yield return null;
        }
        else
        {
            if (unvisitedNodes[0].shortestPathValue != 10000) //this value is a placehold for infinity, or an unreachable node
            {
                currentNode = unvisitedNodes[0];
                currentNode.setActive();
                thisAlgorithmsControler.currentNode = currentNode;
            }
            else
            {
                //If there are no more unreachable unvisited nodes, we can terminate this portion of the algorithm
                doneSearching = true;
            }
            blocked = false;
            yield return null;
        }
    }

    public void StepOfAlgorithm()
    {
        if (!blocked)
        {
            Debug.Log("Algorithm Stepped");
            StartCoroutine(searchNeighboringNodes(currentNode));
        }
    }

    //reconstruct the shortest path based on the information gathered.
    public IEnumerator backTrackAndGetShortestPath()
    {
        blocked = true;
        Debug.Log("reconstructingPath...");
        List<nodeData> returnVal = new List<nodeData>();
        nodeData currentNode = endingNode;
        nodeData previousNode = new nodeData();
        while(currentNode != startingNode)
        {
            returnVal.Add(currentNode);
            List<nodeData> neighboringNodes = currentNode.connectedNodes;
            neighboringNodes = neighboringNodes.OrderBy(x => x.shortestPathValue).ToList();
            nodeData candidateNode = neighboringNodes[0];
            if (neighboringNodes[0] == previousNode)
            {
                candidateNode = neighboringNodes[1]; //pick the next shortestPathNode
            }
            previousNode = currentNode;
            edgeData edgeToColor = currentNode.getEdgeConnectedToNodeX(candidateNode);
            edgeToColor.visitEdge();
            currentNode = candidateNode;
            currentNode.setActive();
        }
        returnVal.Add(currentNode);
        returnVal.Reverse(); //this should now be a list of nodes
        shortestPath = returnVal;
        blocked = false;
        endingNode.setActive();
        thisAlgorithmsControler.currentNode = startingNode;
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (thisAlgorithmsControler.ready && !doneSearching)
        {
            if (first)
            {
                setUp();
                first = false;
            }
            else if (unvisitedNodes.Length != 0)
            {
                StepOfAlgorithm();
            }
            thisAlgorithmsControler.ready = false;
        }
        else if (thisAlgorithmsControler.ready && !done && !blocked && doneSearching)
        {
            StartCoroutine(backTrackAndGetShortestPath());
            done = true;
            thisAlgorithmsControler.ready = false;
        }
    }
}
