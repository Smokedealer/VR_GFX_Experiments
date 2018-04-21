using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
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
        CreateOOExperimentXML();
    }

    private static void SerializePPProfile()
    {
        Experiment experiment = new Experiment();
        experiment.tests = new List<Test>();


        PostProTest test = new PostProTest
        {
            experimentRoomName = "junk_room",
            questions = new List<Question>()
        };

        Question question1 = new Question
        {
            questionText = "[Sample question 1]"
        };
        question1.AddSampleOptions();
        
        test.questions = new List<Question>{question1};
        
        experiment.tests.Add(test);
       
        
        experiment.Save("PPExperimentTemplate.xml");
    }

    private void CreateOOExperimentXML()
    {
        const string filename = "OOExperimentTemplate.xml";

        var experiment = new Experiment();
        experiment.tests = new List<Test>();
        

        int i = 1;

        
        OnObjectTest onObjectTest = new OnObjectTest
        {
            experimentObjectName = "wooden_box"
        };

        List<EffectSettings> settingsOne = new List<EffectSettings>();
        List<EffectSettings> settingsTwo = new List<EffectSettings>();
        
        var experimentSettings1 = new EffectSettings
        {
            propertyName = "_BumpMap",
            propertyType = "texture",
            propertyValue = "1",
            effectActive = true
        };

        var experimentSettings2 = new EffectSettings
        {
            propertyName = "_BumpMap",
            propertyType = "texture",
            propertyValue = "0",
            effectActive = false
        };


        settingsOne.Add(experimentSettings1);
        settingsTwo.Add(experimentSettings2);
        
        onObjectTest.objectOneSettings = settingsOne;
        onObjectTest.objectTwoSettings = settingsTwo;

        Question question = new Question
        {
            questionText = "[This text can be whatever you want.]",
            experimentPart = i
        };
        question.AddSampleOptions();

        onObjectTest.questions = new List<Question>(1) {question};

        experiment.tests.Add(onObjectTest);
        i++;
        
        
        experiment.Save(filename);
        
    }
}
