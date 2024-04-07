using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dfsAlgorithm : MonoBehaviour
{
    /*
    The general idea is that this algorithm runs one step at a time in the StepOfAlgorithm() function
    after each step, it tells the controller to unready, which pauses the algorithm. It will then wait
    for a keypress or button press if in vr to execute another step. It will go until it reaches the end node
    Should give a generally good idea of how to implement future algorithms
    */
    public bool isLinear = true; //IsLinear is necissary to declare in all algorithm functions, as it tells the handler 
                                //whether or not it should teleport the player to the current node
    //NOTICE: this version of DFS does not randomly pick the next node, instead it picks the shortest path
    public algorithmControler thisAlgorithmsControler;
    private bool first = true;
    public nodeData startNode;
    public nodeData endNode;
    public bool done = false;
    //dfs specific stuff
    Stack<nodeData> dfsStack = new Stack<nodeData>();
    nodeData currentNode; //Also is needed in every algorithm function as the AlgorithmHandler needs to know the current node to teleport the player to it
    // Update is called once per frame
    void Update()
    {
        if (thisAlgorithmsControler.ready && !done)
        {
            if (first)
            {
                this.SetUp();
                first = false;
            }
            else
            {
                StepOfAlgorithm();
            }
            this.thisAlgorithmsControler.ready = false;
        }
    }

    public void SetUp()
    {
        startNode = thisAlgorithmsControler.startingNode;
        endNode = thisAlgorithmsControler.endingNode;
        startNode.setStart();
        endNode.setEnd();
        dfsStack.Push(startNode);
        currentNode = startNode;
        thisAlgorithmsControler.currentNode = currentNode;
        Debug.Log("Algorithm Started");
    }

    public void StepOfAlgorithm()
    {
        Debug.Log("Algorithm Stepped");
        if (currentNode != endNode)
        {
            currentNode.visitNode();
            nodeData temp = currentNode.getClosestNode(true);
            if (temp == null)
            {
                dfsStack.Pop();
                currentNode = dfsStack.Peek();
                currentNode.setActive();
                thisAlgorithmsControler.currentNode = currentNode;
            }
            else
            {
                edgeData edgeToColor = currentNode.getEdgeConnectedToNodeX(temp);
                edgeToColor.visitEdge();
                currentNode = temp;
                currentNode.setActive();
                thisAlgorithmsControler.currentNode = currentNode;
                dfsStack.Push(temp);
            }
        }
        else
        {
            done = true;
        }
        startNode.setStart(); //just incase start gets overwritten on a backtrack
    }
}
