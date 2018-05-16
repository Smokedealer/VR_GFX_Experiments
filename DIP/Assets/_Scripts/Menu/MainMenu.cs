using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRTK;

/// <summary>
/// Class for handling main manu user interface. It has some additional featrues like
/// creating template files for experiment definition.
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Creates necessary folders for the application runtime.
    /// </summary>
    private void Awake()
    {
        new DirectoryInfo("Experiments").Create();
        new DirectoryInfo("Results").Create();
        new DirectoryInfo("Recordings").Create();
    }

    /// <summary>
    /// Loads given experiment scene
    /// </summary>
    /// <param name="sceneNumber">The number of the screen in build settings</param>
    public void StartExperiment(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    /// <summary>
    /// Application exit method. Only prints out <i>"Application Exit"</i> in Edit mode.
    /// </summary>
    public void quitApplication()
    {
        Application.Quit();
        Debug.Log("Application exit");
    }

    /// <summary>
    /// Handler for post-processing screen swap.
    /// </summary>
    public void StartPostProcessingExperiment()
    {
        StartExperiment(2);
    }

    /// <summary>
    /// Handler for on object experiment scene swap
    /// </summary>
    public void StartOnObjectExperiment()
    {
        StartExperiment(1);
    }

    /// <summary>
    /// Handler for creating template files
    /// </summary>
    public void CreateTemplateFiles()
    {
        CreateExamplePPExperiment();
        CreateExampleOOExperiment();
    }

    /// <summary>
    /// Creates default post processing template that can be edited and used.
    /// </summary>
    private static void CreateExamplePPExperiment()
    {
        Experiment experiment = new Experiment();
        experiment.experimentType = FileType.PP;
        experiment.tests = new List<Test>();


        PostProTest test = new PostProTest
        {
            experimentRoomName = "furniture_room",
            questions = new List<Question>()
        };

        Question question1 = new Question
        {
            questionText = "[Sample question 1]"
        };
        question1.AddSampleOptions();
        
        test.questions = new List<Question>{question1};
        
        experiment.tests.Add(test);
       
        
        experiment.Save("Experiments/PPExperimentTemplate.xml");
    }

    /// <summary>
    /// Creates default on object experiment template that can be edited and used.
    /// </summary>
    private void CreateExampleOOExperiment()
    {
        const string filename = "Experiments/OOExperimentTemplate.xml";

        var experiment = new Experiment();

        experiment.experimentType = FileType.OO;
        experiment.tests = new List<Test>();

        
        OnObjectTest onObjectTest = new OnObjectTest
        {
            experimentObjectName = "WoodenCrate"
        };

        List<EffectSettings> settingsOne = new List<EffectSettings>();
        List<EffectSettings> settingsTwo = new List<EffectSettings>();
        
        var experimentSettings1 = new EffectSettings
        {
            propertyName = "_BumpMap",
            propertyType = "texture",
            propertyValue = "1"
        };

        var experimentSettings2 = new EffectSettings
        {
            propertyName = "_BumpMap",
            propertyType = "texture",
            propertyValue = "0"
        };


        settingsOne.Add(experimentSettings1);
        settingsTwo.Add(experimentSettings2);
        
        onObjectTest.objectOneSettings = settingsOne;
        onObjectTest.objectTwoSettings = settingsTwo;

        Question question = new Question
        {
            questionText = "[This text can be whatever you want.]"
        };
        question.AddSampleOptions();

        onObjectTest.questions = new List<Question>(1) {question};

        experiment.tests.Add(onObjectTest);
        
        experiment.Save(filename);
        
    }
}
