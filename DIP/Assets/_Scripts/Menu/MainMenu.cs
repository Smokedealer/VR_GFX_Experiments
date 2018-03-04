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

    public void StartPostProcessingExperiment(string effectName)
    {
        int ppeSceneNumber = 2;
        
        ExperimentSettings settings = new ExperimentSettings();
        settings.experimentEffect = effectName;
        settings.sceneNumber = ppeSceneNumber;

        ExperimentRunParameters.settings = settings;
        
        StartExperiment(ppeSceneNumber);
    }



    public void DevButton()
    {
        SteamVR_Fade.Start(Color.black, 2.5f);
        Invoke("unfade", 2.5f);
//        SteamVR_Fade.Start(Color.clear, 0.5f);
        
        Debug.Log("Dev button");
//        SerializePPProfile();
    }

    private void unfade()
    {
                SteamVR_Fade.Start(Color.clear, 0.5f);
   
    }


    private void SerializePPProfile()
    {
        PostProExperiment experiment = new PostProExperiment();
        experiment.tests = new List<PostProTest>();

        for (int i = 0; i < 4; i++)
        {
            PostProTest test = new PostProTest();
            test.experimentRoomName = "01";
            test.questions = new List<Question>();
            
            Question question1 = new Question
            {
                questionText = "Which of the two scenes is more appealing to you",
                experimentPart = i
            };
            question1.AddTwoSceneSelectOptions();
            
            Question question2 = new Question
            {
                questionText = "Do you see any flickering",
                experimentPart = i
            };
            question2.AddTwoSceneSelectOptions();

            test.questions = new List<Question>(1) {question1, question2};
            
            experiment.tests.Add(test);
        }
        
        experiment.Save("ppexperiment.xml");
    }

    private void CreateOOExperimentXML()
    {
        const string filename = "experimentOOSettings.xml";

        var experiment = new Experiment
        {
            tests = new List<Test>()
        };

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
