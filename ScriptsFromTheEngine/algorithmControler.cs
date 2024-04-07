using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class algorithmControler : MonoBehaviour
{
    public bool ready = false;
    public nodeData startingNode = null;
    public nodeData endingNode = null;
    public edgeData[] edgeList;
    public nodeData[] nodeList;
    public int numOfNodes;
    //Data To Send
    public nodeData currentNode;
}
