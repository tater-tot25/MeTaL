using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmHandler : MonoBehaviour
{
    public GameObject algToRun; //for now, we will assign the algorithm in the editor
    public algorithmControler algControl;
    public nodeData startNode;
    public nodeData endNode;
    // Start is called before the first frame update
    void Start()
    {
        algControl = algToRun.GetComponent<algorithmControler>();
        algControl.startingNode = startNode;
        algControl.endingNode = endNode;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!algControl.ready == true)
            {
                algControl.ready = true;
            }
        }
    }
}
