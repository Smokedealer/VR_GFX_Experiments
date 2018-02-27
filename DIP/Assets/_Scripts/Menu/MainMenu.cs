using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class MainMenu : MonoBehaviour
{

    public string[] experimentObjects;

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


//    public void LoadExperimentDetails()
//    {
//        var file = new ExperimentSettings();
//        file.sceneNumber = 1;
//        file.Save("experiment.xml");
//        
//        var settings = ExperimentSettings.Load("experiment.xml");
//
//        if (settings == null) Debug.LogError("File not found.");
//
//        //TODO: Zkontrolovat důležité údaje
//
//        ExperimentRunParameters.settings = settings;
//    }
//
//    public void SaveAndLoadTest(string filename)
//    {
//        var result = new ExperimentResult();
//        var question = new Question();
//        question.questionText = "Which of the two objects is more appealing to you?";
//        question.AddLeftRightUndecidedOptions();
//        result.questions.Add(question);
//
//        result.Save(filename);
//        
//        Debug.Log("Saved");
//        
//        var loaded = new ExperimentResult();
//        loaded = ExperimentResult.Load(filename);
//        
//        Debug.Log("Start: " + loaded.experimentStartTime);
//        Debug.Log("End: " + loaded.experimentEndTime);
//        Debug.Log("QuestionSetSize: " + loaded.questions.Count);
//        
//    }

    public void DevButton()
    {
        Debug.Log("Dev button");
        SettingsFile();
    }

    private void SettingsFile()
    {
        string filename = "experimentOOSettings.xml";

        var experiment = new Experiment();
        experiment.tests = new List<Test>();

        int i = 1;

        foreach (var experimentObject in experimentObjects)
        {
            Test test = new Test
            {
                experimentObejctName = experimentObject
            };

            var experimentSettings1 = new EffectSettings
            {
                effectName = "Normal Map",
                propertyName = "_BumpMap",
                propertyType = "texture",
                effectIntensity = 1,
                effectActive = true
            };

            var experimentSettings2 = new EffectSettings
            {
                effectName = "Normal Map",
                propertyName = "_BumpMap",
                propertyType = "texture",
                effectIntensity = 0,
                effectActive = false
            };

            test.objectOneSettings = experimentSettings1;
            test.objectTwoSettings = experimentSettings2;

            Question question = new Question
            {
                questionText = "Which of the two objects is more appealing to you?",
                experimentPart = i
            };
            question.AddLeftRightUndecidedOptions();

            test.questions = new List<Question>(1) {question};

            experiment.tests.Add(test);
            i++;
        }
        
        experiment.Save(filename);
        
    }
}
