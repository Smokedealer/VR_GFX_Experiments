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
        experimentMaganer.LogAnswer(0);
        experimentMaganer.LoadNextObject();
    }

    void SelectSecond()
    {
        Debug.Log("Second object selected");
        experimentMaganer.LogAnswer(2);
        experimentMaganer.LoadNextObject();
    }

    void SelectUndecided()
    {
        Debug.Log("Undecided selected");
        experimentMaganer.LogAnswer(1);
        experimentMaganer.LoadNextObject();
    }
}
