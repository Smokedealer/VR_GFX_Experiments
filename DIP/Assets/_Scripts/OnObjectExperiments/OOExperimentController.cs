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

/// <summary>
/// On object experiment controller handles the flow of the conducted experiment.
/// It also manages the recorder instace for replays and recording.
/// </summary>
public class OOExperimentController : MonoBehaviour, IExperimentController {
    
    /// <summary>
    /// Script with headset fade routine for smooth transitions.
    /// </summary>
    public VRTK_HeadsetFade headsetFade;
    
    /// <summary>
    /// Experiment Part text object.
    /// </summary>
    public TextMeshProUGUI experimentPartText;
    
    /// <summary>
    /// Question text object.
    /// </summary>
    public TextMeshProUGUI questionTextDisplay;
    
    /// <summary>
    /// Gane object of the button to be spawned (this will later contain answers)
    /// </summary>
    public GameObject answerButtonPrefab;
    
    /// <summary>
    /// The container for answers
    /// </summary>
    public Transform answersLayout;

    /// <summary>
    /// Spawn point for the left object
    /// </summary>
    public Transform spawnPoint1;
    
    /// <summary>
    /// Spawn point for the right object
    /// </summary>
    public Transform spawnPoint2;

    
    /// <summary>
    /// Tha play holder prefab. Used for teleportation and recording.
    /// </summary>
    public GameObject player;

    /// <summary>
    /// The recroder reference for recording and replaying.
    /// </summary>
    private Recorder recorder;
    
    
    /// <summary>
    /// Default object to show if the specified one was not found.
    /// </summary>
    public GameObject objectNotFoundDummy;

    public GameObject errorUI;
    public GameObject wallUI;

    /********************************************/
    
    /// <summary>
    /// Experiment object reference
    /// </summary>
    private GameObject experimentObject;
    
    /// <summary>
    /// Original object reference (can also be impared)
    /// </summary>
    private GameObject originalObject;
    
    /// <summary>
    /// Modified copy of the experimented object
    /// </summary>
    private GameObject copyObject;
    
    /// <summary>
    /// Original shader to be edited
    /// </summary>
    private Material originalMaterial;
    
    /// <summary>
    /// Edited shader 
    /// </summary>
    private Material experimentMaterial;
    
    /// <summary>
    /// Experiment structure reference.
    /// </summary>
    private Experiment experiment;
    
    /// <summary>
    /// Loaded tests to be conducted (extracted from the Experiment variable)
    /// </summary>
    private List<OnObjectTest> tests;
    
    /// <summary>
    /// List of all experiment objects 
    /// </summary>
    private List<GameObject> experimentObjects;
    
    /// <summary>
    /// Whether the original and the impaired stimulus are randomly swapped around
    /// </summary>
    private bool swapPositions;
    
    /// <summary>
    /// Current object that is being experimented on
    /// </summary>
    private int currentItemIndex;
    
    /// <summary>
    /// Current question to be displayed (index)
    /// </summary>
    private int currentQuestionIndex;
    
    /// <summary>
    /// List of object names - strings (for replaying)
    /// </summary>
    private List<string> objectNames;
   

    
    /// <summary>
    /// Default unity method called once at the start
    /// </summary>
    void Start()
    {
        //Load experiment info
        experiment = ApplicationDataContainer.loadedExperiment;

        recorder = FindObjectOfType<Recorder>();
      

        //If replay flag is on, run replay routine
        if (ApplicationDataContainer.replay)
        {
            objectNames = ApplicationDataContainer.loadedRecording.experimentGameObjects;
            
            var controllerSwapper = player.GetComponent<PlayerControllerSwapper>();
            controllerSwapper.activeController = Controller.Observer;
            controllerSwapper.RefreshActive();

            recorder.StartReplay();
        }
        else //Run normal experiment
        {
            if (experiment == null)
            {
                errorUI.active = true;
                wallUI.active = false;
                return;
            }
            
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

    /// <summary>
    /// Experiment objects are loaded, in case of missing ones a dummy is 
    /// put in place with warning message. 
    /// </summary>
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

    /// <summary>
    /// In case and object is missing the test also has to be altered.
    /// This method changes the question text to predefined error message
    /// and sets the only option to continue. 
    /// </summary>
    /// <param name="test">Test to be edited</param>
    private void ReplaceMissingWithError(OnObjectTest test)
    {
        test.questions.Clear();
        
        Question question = new Question();
        question.questionText = "Experiment object was not found. Please contact the creator of this experiment.";
        question.questionOptions = new List<string>();
        question.questionOptions.Add("Continue");
        
        test.questions.Add(question);
    }


    /// <summary>
    /// Next step in the experiment flow
    /// </summary>
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

    /// <summary>
    /// Resets the player position to the middle of the room
    /// </summary>
    /// <returns>IEnumerator for the fade routine</returns>
    IEnumerator ResetPlayerPosition()
    {
        float transitionDuration = 0.1f;
        headsetFade = FindObjectOfType<VRTK_HeadsetFade>();
        
        if (headsetFade == null)
        {
            player.transform.position = Vector3.zero;
        }
        else
        {
            headsetFade.Fade(Color.black, transitionDuration);
            yield return new WaitForSeconds(transitionDuration);
            player.transform.position = Vector3.zero;
            headsetFade.Unfade(transitionDuration * 3);
        }
    }

    /// <summary>
    /// Destroys both objects to make room for the next ones.
    /// </summary>
    private void DespawnOldObjects()
    {
        Destroy(originalObject);
        Destroy(copyObject);
    }


    /// <summary>
    /// UI question update
    /// </summary>
    private void SetQuestionText()
    {
        questionTextDisplay.text = experiment.tests[currentItemIndex].questions[currentQuestionIndex].questionText;
    }

    /// <summary>
    /// Loaded answers are put into the answers layout.
    /// </summary>
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

    /// <summary>
    /// Delegate for button clicks (for answers)
    /// </summary>
    /// <param name="optionIndex">number of answer to be recorded</param>
    /// <returns>Unity action that has been raised</returns>
    private UnityAction buttonClickDelegate(int optionIndex)
    {
        return delegate { SelectAnswer(optionIndex); };
    }

    /// <summary>
    /// Clears all answers to make space for new ones
    /// </summary>
    private void RemoveAnswersDisplay()
    {
        //Remove old answers
        foreach (Transform child in answersLayout) {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Clears the question text
    /// </summary>
    private void RemoveQuestionText()
    {
        questionTextDisplay.text = "";
    }
    
    /// <summary>
    /// This method applies properties to shaders and then the shaders are applied to the
    /// object itself.
    /// </summary>
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


    /// <summary>
    /// Once the experiment objects are prepared the can be spawned.
    /// This method also shuffles the objects if necessary.
    /// 
    /// </summary>
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

    /// <summary>
    /// Loads next question to be show to the user. The question
    /// contains a list of answers that are spawned as well.
    /// </summary>
    private void LoadNextQuestion()
    {
        if (currentQuestionIndex + 1 < experiment.tests[currentQuestionIndex].questions.Count)
        {
            currentQuestionIndex++;
            SetQuestionText();
            SetOptions();
        }
    }

    
    /// <summary>
    /// Once all questions are answered for one object, next object is loaded.
    /// If no more objects are available, experiment ends.
    /// </summary>
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

    /// <summary>
    /// Finalize experiment - save file, recording and return to main menu
    /// </summary>
    private void EndExperiment()
    {
        DateTime now = DateTime.Now;
        experiment.experimentEndTime = now;
        string filename = now.ToString("yyyyMMddhhmm");
        
        RemoveAnswersDisplay();
        RemoveQuestionText();
        DespawnOldObjects();
        
        experiment.SaveResult("OO-Result-" + filename + ".xml");

        if (recorder)
        {
            recorder.recordedData.experimentGameObjects = objectNames;
            recorder.StopRecording("OO-Recording-" + filename + ".rec");
        }
        
        ReturnToMainMenu();
    }

    /// <summary>
    /// Apply materials to tested objects
    /// </summary>
    /// <param name="originalObject">Original object</param>
    /// <param name="experimentObject">Impaired object</param>
    private void AddExperimentMaterialToObject(GameObject originalObject, GameObject experimentObject)
    {
        originalObject.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(originalMaterial);
        experimentObject.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(experimentMaterial);
    }

    /// <summary>
    /// Logs the answer chosen by the user
    /// </summary>
    /// <param name="answer">index of the answer</param>
    public void SelectAnswer(int answer)
    {
        if (swapPositions)
        {
            if (answer == 2) answer = 0;
            else if (answer == 0) answer = 2;
        }
        
        experiment.tests[currentItemIndex].questions[currentQuestionIndex].answerIndex = answer;
        
        LoadNextObject();
    }

    /// <summary>
    /// Loads the main menu scene
    /// </summary>
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Getter for the Experiment
    /// </summary>
    /// <returns>Experiment tha is being conducted</returns>
    public Experiment GetExperimentReference()
    {
        return experiment;
    }
}
