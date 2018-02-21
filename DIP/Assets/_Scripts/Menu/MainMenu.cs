using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class MainMenu : MonoBehaviour {


    public void StartExperiment(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    public void quitApplication()
    {
        Application.Quit();
        Debug.Log("Application exit");
    }

    public void StartPostProcessingExperiment(string effectName)
    {
        int ppeSceneNumber = 2;
        
        ExperimentSettings settings = new ExperimentSettings();
        settings.experimentEffect = effectName;
        settings.sceneNumber = ppeSceneNumber;

        ExperimentRunParameters.settings = settings;
        
        StartExperiment(ppeSceneNumber);
    }


    public void LoadExperimentDetails()
    {
        var file = new ExperimentSettings();
        file.sceneNumber = 1;
        file.Save("experiment.xml");
        
        var settings = ExperimentSettings.Load("experiment.xml");

        if (settings == null) Debug.LogError("File not found.");

        //TODO: Zkontrolovat důležité údaje

        ExperimentRunParameters.settings = settings;
    }

    public void SaveAndLoadTest(string filename)
    {
        var result = new ExperimentResult();
        var question = new Question();
        question.questionText = "Which of the two objects is more appealing to you?";
        question.AddLeftRightUndecidedOptions();
        result.questions.Add(question);

        result.Save(filename);
        
        Debug.Log("Saved");
        
        var loaded = new ExperimentResult();
        loaded = ExperimentResult.Load(filename);
        
        Debug.Log("Start: " + loaded.experimentStartTime);
        Debug.Log("End: " + loaded.experimentEndTime);
        Debug.Log("QuestionSetSize: " + loaded.questions.Count);
        
    }

    public void DevButton()
    {
        Debug.Log("Dev button");
    }
}
