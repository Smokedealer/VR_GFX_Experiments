using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.PostProcessing;
using VRTK;


public class MainMenu : MonoBehaviour
{

    public string[] experimentObjects;

    public PostProcessingProfile profile;

    public SteamVR_Fade _fade;
    
    public void StartExperiment(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    public void quitApplication()
    {
        Application.Quit();
        Debug.Log("Application exit");
    }

    public void StartPostProcessingExperiment()
    {
        StartExperiment(2);
    }


    public void StartOnObjectExperiment()
    {
        StartExperiment(1);
    }


    public void DevButton()
    {
        Debug.Log("Dev button");
        SerializePPProfile();
    }

    private void SerializePPProfile()
    {
        PostProExperiment experiment = new PostProExperiment();
        experiment.tests = new List<PostProTest>();

        for (int i = 0; i < 4; i++)
        {
            PostProTest test = new PostProTest();
            test.experimentRoomName = "junk_room";
            test.questions = new List<Question>();
            
            Question question1 = new Question
            {
                questionText = "[Sample question 1]",
                experimentPart = i
            };
            question1.AddTwoSceneSelectOptions();
            
            Question question2 = new Question
            {
                questionText = "[Sample question 2]",
                experimentPart = i
            };
            question2.AddTwoSceneSelectOptions();

            test.questions = new List<Question>(2) {question1, question2};
            
            experiment.tests.Add(test);
        }
        
        experiment.Save("ppexperiment.xml");
    }

    private void CreateOOExperimentXML()
    {
        const string filename = "experimentOOSettings.xml";

        var experiment = new Experiment
        {
            tests = new List<OnObjectTest>()
        };

        int i = 1;

        foreach (var experimentObject in experimentObjects)
        {
            OnObjectTest onObjectTest = new OnObjectTest
            {
                experimentObjectName = experimentObject
            };

            var experimentSettings1 = new EffectSettings
            {
                effectName = "Normal Map",
                propertyName = "_BumpMap",
                propertyType = "texture",
                propertyValue = 1,
                effectActive = true
            };

            var experimentSettings2 = new EffectSettings
            {
                effectName = "Normal Map",
                propertyName = "_BumpMap",
                propertyType = "texture",
                propertyValue = 0,
                effectActive = false
            };

            onObjectTest.objectOneSettings = experimentSettings1;
            onObjectTest.objectTwoSettings = experimentSettings2;

            Question question = new Question
            {
                questionText = "Which of the two objects is more appealing to you?",
                experimentPart = i
            };
            question.AddLeftRightUndecidedOptions();

            onObjectTest.questions = new List<Question>(1) {question};

            experiment.tests.Add(onObjectTest);
            i++;
        }
        
        experiment.Save(filename);
        
    }
}
