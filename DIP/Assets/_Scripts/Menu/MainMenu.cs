using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class MainMenu : MonoBehaviour {

    public TMP_Dropdown dropDown;

    void Start()
    {
        dropDown.onValueChanged.AddListener(ChangeExperimentPart);
    }

    public void StartExperiment(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    public void quitApplication()
    {
        //TODO Exit app
        Debug.Log("Application exit");
    }

    public void ChangeExperimentPart(int index)
    {
        string selectedValue = dropDown.options[dropDown.value].text;
        Debug.Log(selectedValue);

        string effectTranslated = ExperimentEffects.getInstance().GetValue(selectedValue);
        Debug.Log(effectTranslated);

        ExperimentRunParameters.experimentPart = effectTranslated;
    }

}
