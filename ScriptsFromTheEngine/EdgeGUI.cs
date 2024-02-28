using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeGUI : MonoBehaviour
{
    public GameObject startNode;
    public GameObject endNode;
    public LineRenderer draw;
    public float edgeWeight; //might change to an int


    //Should be called once at setup by the layer that is in charge of setting up the graph
    //visualization
    public void setUpLine()
    {
        draw = this.gameObject.GetComponent<LineRenderer>();
        draw.positionCount = 2;
        GameObject[] temp = { startNode, endNode };
        draw.SetPosition(0, startNode.transform.position);
        draw.SetPosition(1, startNode.transform.position);
        this.setInactive();
    }

    public void setActive()
    {
        draw.startColor = Color.green;
        draw.endColor = Color.green;
    }

    public void setInactive()
    {
        draw.startColor = Color.red;
        draw.endColor = Color.red;
    }

}
