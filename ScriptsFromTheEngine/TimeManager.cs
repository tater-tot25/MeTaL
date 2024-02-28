using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public bool GUIReady;
    public bool AlgorithmReady;
    public GUIManager manager; 
    public AlgShell algorithm; //To be implemented
    void Update()
    {
        if (GUIReady)
        {
            freeGUI();
        }
        else if (AlgorithmReady)
        {
            freeAlgorithm();
        }
    }
    public void freeAlgorithm()
    {
        algorithm.step = true;
        AlgorithmReady = false;
    }
    public void freeGUI()
    {
        manager.step = true;
        GUIReady = false;
    }
}
