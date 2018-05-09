using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VRTK;
using Random = UnityEngine.Random;

public class OOExperimentController : MonoBehaviour, IExperimentController {
    
    public VRTK_HeadsetFade headsetFade;
    public TextMeshProUGUI experimentPartText;
    public TextMeshProUGUI questionTextDisplay;
    public GameObject answerButtonPrefab;
    public Transform answersLayout;

    public Transform spawnPoint1;
    public Transform spawnPoint2;

    public GameObject player;

    public Recorder recorder;
    
    public GameObject objectNotFoundDummy;

    /********************************************/
    
    private GameObject experimentObject;
    private GameObject originalObject;
    private GameObject copyObject;
    private Material originalMaterial;
    private Material experimentMaterial;
    private Experiment experiment;
    private List<OnObjectTest> tests;
    private List<GameObject> experimentObjects;
    private bool swapPositions;
    private int currentItemIndex;
    private int currentQuestionIndex;
    private List<string> objectNames;

    void Start()
    {
        //Load experiment info
        experiment = ApplicationDataContainer.loadedExperiment;

        if (experiment == null)
        {
            Debug.Log("Loading default experiment");
            experiment = Experiment.Load("OOExperimentTemplate.xml");
        }


        if (ApplicationDataContainer.replay)
        {
            objectNames = ApplicationDataContainer.loadedRecording.experimentGameObjects;
            
            var controllerSwapper = player.GetComponent<PlayerControllerSwapper>();
            controllerSwapper.controller = PlayerControllerSwapper.Controller.Observer;
            controllerSwapper.RefreshActive();

            recorder.StartReplay();
        }
        else
        {
            player.GetComponent<PlayerControllerSwapper>().RefreshActive();
            
            InitRecorder();
            
            recorder.StartRecording();
            
            PrepareExperimentObjects();
            //Set beginning
            currentItemIndex = 0;
        
            //Fill in experiment start time
            experiment.experimentStartTime = DateTime.Now;
        
            NextRound();
        }

        
    }
    
    /// <summary>
    /// If the recorder is not explicitly set this method will attempt to find it in the scene.
    /// </summary>
    private void InitRecorder()
    {
        if (!recorder)
        {
            recorder = GameObject.FindGameObjectWithTag("Recorder").GetComponent<Recorder>();

            if (!recorder)
            {
                Debug.LogError("No gameobject with tag Recorder was found in the scene. Will not record.");
                return;
            }

            //Sets which kind of recording this will be
            recorder.SetType(this);
        }
    }

    private void PrepareExperimentObjects()
    {
        tests = new List<OnObjectTest>();
        experimentObjects = new List<GameObject>();
        objectNames = new List<string>();

        foreach (var experimentTest in experiment.tests)
        {
            var testConverted = experimentTest as OnObjectTest;
            tests.Add(testConverted);
            objectNames.Add(testConverted.experimentObjectName);

            var loadedObject = Resources.Load(testConverted.experimentObjectName) as GameObject;

            if (loadedObject == null)
            {
                ReplaceMissingWithError(testConverted);
                loadedObject = objectNotFoundDummy;
            }

            experimentObjects.Add(loadedObject);
        }
    }

    private void ReplaceMissingWithError(OnObjectTest test)
    {
        test.questions.Clear();
        
        Question question = new Question();
        question.questionText = "Experiment object was not found. Please contact the creator of this experiment.";
        question.questionOptions = new List<string>();
        question.questionOptions.Add("Continue");
        
        test.questions.Add(question);
        
        
    }


    private void NextRound()
    {
        //Despawn old (if any)
        DespawnOldObjects();
        
        
        //Reset player position
        StartCoroutine(ResetPlayerPosition());

        //Set new object
        experimentObject = experimentObjects[currentItemIndex];

        //Display part of the experiment
        experimentPartText.text = currentItemIndex + 1 + "/" + experiment.tests.Count;
       
        //Prepare materials
        PrepareMaterials();
        
        //Spawn both objects
        SpawnExperimentObjects();
        
        //Display question
        SetQuestionText();
        
        //Display available options
        SetOptions();
    }

    
    IEnumerator ResetPlayerPosition()
    {
        float transitionDuration = 0.1f;
        
        headsetFade.Fade(Color.black, transitionDuration);
        yield return new WaitForSeconds(transitionDuration);
        player.transform.position = Vector3.zero;
        headsetFade.Unfade(transitionDuration * 3);
    }

    private void DespawnOldObjects()
    {
        Destroy(originalObject);
        Destroy(copyObject);
    }


    private void SetQuestionText()
    {
        questionTextDisplay.text = experiment.tests[currentItemIndex].questions[currentQuestionIndex].questionText;
    }

    private void SetOptions()
    {
        RemoveAnswersDisplay();

        int optionIndex = 0;
        
        foreach (var option in experiment.tests[currentItemIndex].questions[currentQuestionIndex].questionOptions)
        {
            var button = Instantiate(answerButtonPrefab, answersLayout);
            button.GetComponentInChildren<TextMeshProUGUI>().text = option;
            button.GetComponent<Button>().onClick.AddListener(
                buttonClickDelegate(optionIndex)
            );
            optionIndex++;
        }
    }

    private UnityAction buttonClickDelegate(int optionIndex)
    {
        return delegate { SelectAnswer(optionIndex); };
    }


    private void RemoveAnswersDisplay()
    {
        //Remove old answers
        foreach (Transform child in answersLayout) {
            Destroy(child.gameObject);
        }
    }

    private void RemoveQuestionText()
    {
        questionTextDisplay.text = "";
    }
    
    private void PrepareMaterials()
    {
        //Get original material
        originalMaterial = new Material(experimentObject.GetComponent<Renderer>().sharedMaterial);
      
        foreach (var settings in tests[currentItemIndex].objectOneSettings)
        {
            settings.SetPropertyByType(settings, originalMaterial);
        }
        
        //Create a copy to edit
        experimentMaterial = new Material(experimentObject.GetComponent<Renderer>().sharedMaterial);

        foreach (var settings in tests[currentItemIndex].objectTwoSettings)
        {
           settings.SetPropertyByType(settings, experimentMaterial);
        }
    }


    private void SpawnExperimentObjects()
    {
        swapPositions = Random.value < 0.5f;

        //Swap the positions
        Transform actualSpawnPoint1 = swapPositions ? spawnPoint2 : spawnPoint1;
        Transform actualSpawnPoint2 = swapPositions ? spawnPoint1 : spawnPoint2;

        originalObject = Instantiate(experimentObject, actualSpawnPoint1.position, actualSpawnPoint1.rotation);
        copyObject = Instantiate(experimentObject, actualSpawnPoint2.position, actualSpawnPoint2.rotation);

        AddExperimentMaterialToObject(originalObject, copyObject);

    }

    private void LoadNextQuestion()
    {
        if (currentQuestionIndex + 1 < experiment.tests[currentQuestionIndex].questions.Count)
        {
            currentQuestionIndex++;
            SetQuestionText();
            SetOptions();
        }
    }

    public void LoadNextObject()
    {
        if (currentQuestionIndex + 1 < experiment.tests[currentItemIndex].questions.Count)
        {
            LoadNextQuestion();
        }
        else if (currentItemIndex + 1 >= experiment.tests.Count) //Experiment is done
        {
            experimentPartText.text = "done";
            EndExperiment();
        }
        else
        {
            currentItemIndex++;
            NextRound();
        }

    }

    private void EndExperiment()
    {
        RemoveAnswersDisplay();
        RemoveQuestionText();
        DespawnOldObjects();
        SaveExperimentResults();

        if (recorder)
        {
            recorder.recordedData.experimentGameObjects = objectNames;
            recorder.StopRecording();
        }
        
        ReturnToMainMenu();
    }

    private void SaveExperimentResults()
    {
        DateTime now = DateTime.Now;
        experiment.experimentEndTime = now;
        string filename = now.ToString("yyyyMMddhhmm");
        experiment.Save("result-" + filename + ".xml");
    }

    private void AddExperimentMaterialToObject(GameObject originalObject, GameObject experimentObject)
    {
        originalObject.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(originalMaterial);
        experimentObject.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(experimentMaterial);
    }

    public void SelectAnswer(int answer)
    {
        if (swapPositions)
        {
            if (answer == 2) answer = 0;
            else if (answer == 0) answer = 2;
        }
        
        experiment.tests[currentItemIndex].questions[currentQuestionIndex].answerIndex = answer;
        experiment.tests[currentItemIndex].questions[currentQuestionIndex].experimentPart = currentItemIndex;
        
        LoadNextObject();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public Experiment GetExperimentReference()
    {
        return experiment;
    }
}
