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
    //NOTICE: this version of DFS does not randomly pick the next node, instead it picks the shortest path
    public algorithmControler thisAlgorithmsControler;
    private bool first = true;
    public nodeData startNode;
    public nodeData endNode;
    public bool done = false;
    //dfs specific stuff
    Stack<nodeData> dfsStack = new Stack<nodeData>();
    nodeData currentNode;
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
        startNode.setActive();
        dfsStack.Push(startNode);
        currentNode = startNode;
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
            }
            else
            {
                edgeData edgeToColor = currentNode.getEdgeConnectedToNodeX(temp);
                edgeToColor.visitEdge();
                currentNode = temp;
                currentNode.setActive();
                dfsStack.Push(temp);
            }
        }
        else
        {
            done = true;
        }
    }
}
