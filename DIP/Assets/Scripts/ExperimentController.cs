using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentController : MonoBehaviour {

    public ExperimentManager experimentMaganer;



	void Update () {
        if (Input.GetButtonDown("Select1"))
        {
            SelectFirst();
        }

        if (Input.GetButtonDown("Select2"))
        {
            SelectSecond();
        }

        if (Input.GetButtonDown("SelectUndecided"))
        {
            SelectUndecided();
        }
    }

    void SelectFirst()
    {
        Debug.Log("First object selected");
        //TODO log the result

        experimentMaganer.LoadNextObject();
    }

    void SelectSecond()
    {
        Debug.Log("Second object selected");
        //TODO log the result

        experimentMaganer.LoadNextObject();
    }

    void SelectUndecided()
    {
        Debug.Log("Undecided selected");
        //TODO log the result

        experimentMaganer.LoadNextObject();
    }
}
