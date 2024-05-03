using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class buildingDetector : MonoBehaviour
{
    public GameObject[] buildingList;
    private bool first = true;
    private bool loopControl = true;
    private TMP_Text textComponent;

    // Start is called before the first frame update
    public string getNearestBuilding(Vector3 playerPosition)
    {
        loopControl = false;
        float currentClosest = Vector3.Distance(playerPosition, buildingList[0].transform.position);
        string returnVal = buildingList[0].name;
        foreach (GameObject building in buildingList)
        {
            if (Vector3.Distance(building.transform.position, playerPosition) < currentClosest)
            {
                currentClosest = Vector3.Distance(building.transform.position, playerPosition);
                returnVal = building.name;
            }
        }
        loopControl = true;
        return returnVal;
    }

    IEnumerator everyOneSecondLoop()
    {
        while (true)
        {
            if (loopControl)
            {
                string GUIText = getNearestBuilding(this.transform.position);
                GUIText = ("The nearest building is " + GUIText);
                textComponent.text = GUIText;
            }
            yield return new WaitForSeconds(1);
        }
    }

    private void Start()
    {
        textComponent = this.gameObject.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(everyOneSecondLoop());
        first = false;
    }
}
