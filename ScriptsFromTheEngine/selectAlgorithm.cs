using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class selectAlgorithm : MonoBehaviour
{
    public string algorithmToRun;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "player")
        {
            crossSceneDataHandler.algorithmToRun = algorithmToRun;
            SceneManager.LoadScene("mainMap"); //TODO: I need to implement the fade to black for both teleporting and scene loading
        }
    }
}
