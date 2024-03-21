using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class edgeData : MonoBehaviour
{
    public GameObject StartNode;
    public GameObject EndNode;
    public float edgeWeight = 0;
    public EdgeGUI connectedGUIElement;
    public bool isVisited = false;

    public void Start()
    {
        //edgeWeight is automatically calculated assuming the map of Whitman provided is exactly to scale
        //Otherwise, exact sidewalk measurements would be needed, which would be a pain in the ass
        edgeWeight = Vector3.Distance(StartNode.transform.position, EndNode.transform.position);
        this.GetComponent<Renderer>().enabled = false;
        this.GetComponent<Collider>().enabled = false;
    }

    public void visitEdge()
    {
        isVisited = true;
        connectedGUIElement.setVisited();
    }

    public void setActive()
    {
        isVisited = true;
        connectedGUIElement.setActive();
    }
}
