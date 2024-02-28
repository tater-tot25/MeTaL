using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    public bool step = false;
    public TimeManager manager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (step)
        {








            manager.AlgorithmReady = true; //This is done so that once the animation is done, 
            step = false;                  //Can renable the operation of the algorithm.
        }
    }
}
