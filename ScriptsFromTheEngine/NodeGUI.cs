using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGUI : MonoBehaviour
{
    public Material inactive;
    public Material active;
    public Material visited;
    public Material endNode;
    public Material startNode;
    public Material candidateNode;
    // Start is called before the first frame update
    void Start()
    {
        this.setInactive();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setActive()
    {
        this.gameObject.GetComponent<Renderer>().material = active;
    }

    public void setInactive()
    {
        this.gameObject.GetComponent<Renderer>().material = inactive;
    }

    public void setVisited()
    {
        this.gameObject.GetComponent<Renderer>().material = visited;
    }

    public void setEnd()
    {
        this.gameObject.GetComponent<Renderer>().material = endNode;
    }

    public void setCandidate()
    {
        this.gameObject.GetComponent<Renderer>().material = candidateNode;
    }

    public void setStart()
    {
        this.gameObject.GetComponent<Renderer>().material = startNode;
    }
}
