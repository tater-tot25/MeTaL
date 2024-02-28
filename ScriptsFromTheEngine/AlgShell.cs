using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgShell : MonoBehaviour
{
    public bool step;
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
            manager.GUIReady = true;
            step = false;
        }   
    }
}
