using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class OOExperimentController : MonoBehaviour {

    public TextMeshProUGUI experimentPartText;
    public TextMeshProUGUI questionTextDisplay;
    public GameObject answerButtonPrefab;
    public Transform answersLayout;

    public Transform spawnPoint1;
    public Transform spawnPoint2;

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

    void Start()
    {
        //Load experiment info
        experiment = Experiment.Load("OOExperimentTemplate.xml");

        tests = new List<OnObjectTest>();
        experimentObjects = new List<GameObject>();
        
        foreach (var experimentTest in experiment.tests)
        {
            var testConverted = experimentTest as OnObjectTest;
            tests.Add(testConverted);

            var loadedObject = Resources.Load(testConverted.experimentObjectName) as GameObject;
            experimentObjects.Add(loadedObject);
            
            if (loadedObject == null)
            {
                //TODO show error
                break;
            }
        }

        //Set beginning
        currentItemIndex = 0;
        
        //Fill in experiment start time
        experiment.experimentStartTime = DateTime.Now;
        
                
        NextRound();
    }


    private void NextRound()
    {
        //Despawn old (if any)
        DespawnOldObjects();

        //Set new object
        experimentObject = experimentObjects[currentItemIndex];

        //Display part of the experiment
        experimentPartText.text = currentItemIndex + 1 + "/" + experiment.tests.Count;

        //Prepare materials
        PrepareMaterials();
        
        //Display question
        SetQuestionText();
        
        //Display available options
        SetOptions();
       
        //Spawn both objects
        SpawnExperimentObjects();
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
            RemoveAnswersDisplay();
            RemoveQuestionText();
            DespawnOldObjects();
            SaveExperimentResults();
            ReturnToMainMenu();
        }
        else
        {
            currentItemIndex++;
//            experimentObject = experimentObjects[currentItemIndex];
            
            NextRound();
        }

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
        Debug.Log("Same? " + originalMaterial.Equals(experimentMaterial));
        
        originalObject.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(originalMaterial);
        experimentObject.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(experimentMaterial);
    }

    public void SelectAnswer(int answer)
    {
        Debug.Log("Answer selected: " + answer);
        
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

}
