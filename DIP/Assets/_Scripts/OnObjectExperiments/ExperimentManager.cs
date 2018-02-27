using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ExperimentManager : MonoBehaviour {

    public GameObject experimentObject;
    public TextMeshProUGUI textDisplayObject;
    public TextMeshProUGUI questionTextDisplay;
    public GameObject answerButtonPrefab;
    public Transform answersLayout;
    public string disabledFeatureKeyword;

    public Transform spawnPoint1;
    public Transform spawnPoint2;

    private GameObject originalObject;
    private GameObject copyObject;

    private Material originalMaterial;
    private Material experimentMaterial;

//    public ExperimentResult experimentResult;
    public Experiment experiment;
    public GameObject FPC;  // Assign the First Person Controller to this in the Editor.

    private bool swapPositions;

    public List<GameObject> experimentObjects;
    private int currentItemIndex;
    private int currentQuestionIndex;

    void Start()
    {
        //Load experiment info
        experiment = Experiment.Load("experimentOOSettings.xml");

        //Set beginning
        currentItemIndex = 0;
        
        //Fill in experiment start time
        experiment.experimentStartTime = DateTime.Now;
        
        //Load all aobjects from resources
        foreach (var test in experiment.tests)
        {
            experimentObjects.Add(Resources.Load<GameObject>(test.experimentObejctName));
            Debug.Log("Experiment object sucessfully loaded.");
        }
        
        SpawnExperimentObject();
    }


    private void SpawnExperimentObject()
    {
        //Despawn old (if any)
        DespawnOldObjects();

        //Set new object
        experimentObject = experimentObjects[currentItemIndex];

        //Display part of the experiment
        textDisplayObject.text = (currentItemIndex + 1) + "/" + experimentObjects.Count;

        //Display question
        SetQuestionText();
        
        //Display available options
        SetOptions();
        
        //Prepare materials
        PrepareMaterials();

        //Disable experiment feature
        DisableExperimentFeatureOnMaterial();

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
        
        foreach (var option in experiment.tests[currentItemIndex].questions[currentQuestionIndex].questionOptions)
        {
            var button = Instantiate(answerButtonPrefab, answersLayout);
            button.GetComponentInChildren<TextMeshProUGUI>().text = option;
            //TODO přidat eventy na talčítka, jinak to nic nedělá
        }

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
        setPropertyByType(experiment.tests[currentItemIndex].objectOneSettings, originalMaterial);
        
        //Create a copy to edit
        experimentMaterial = new Material(experimentObject.GetComponent<Renderer>().sharedMaterial);
        setPropertyByType(experiment.tests[currentItemIndex].objectTwoSettings, experimentMaterial);
    }

    private void setPropertyByType(EffectSettings effectSettings, Material effectMaterial)
    {
        switch (effectSettings.propertyType)
        {
            case "texture":
                if(!effectSettings.effectActive) effectMaterial.SetTexture(effectSettings.propertyName, null);
                break;
            case "float":
                if(!effectSettings.effectActive) effectMaterial.SetFloat(effectSettings.propertyName, effectSettings.effectIntensity);
                break;
        }
        
        //TODO dodělat ostatní case
            
    }


    private void DisableExperimentFeatureOnMaterial()
    {
        //Disable the experiment feature
        experimentMaterial.SetTexture(disabledFeatureKeyword, null);
        
        //TODO vypnout 
    }


    private void SpawnExperimentObjects()
    {
        swapPositions = Random.value < 0.5f;

        Transform actualSpawnPoint1 = swapPositions ? spawnPoint2 : spawnPoint1;
        Transform actualSpawnPoint2 = swapPositions ? spawnPoint1 : spawnPoint2;

        originalObject = Instantiate(experimentObject, actualSpawnPoint1.position, actualSpawnPoint1.rotation);
        copyObject = Instantiate(experimentObject, actualSpawnPoint2.position, actualSpawnPoint2.rotation);

        AddExperimentMaterialToObject(copyObject);

        Debug.Log("Experiment objects spawned.");
    }

    public void LoadNextObject()
    {
        if (currentItemIndex + 1 >= experimentObjects.Length) //Experiment is done
        {
            textDisplayObject.text = "done";
            RemoveAnswersDisplay();
            RemoveQuestionText();
            DespawnOldObjects();
            SaveExperimentResults();
            ReturnToMainMenu();
        }
        else
        {
            currentItemIndex++;
            experimentObject = experimentObjects[currentItemIndex];
            
            SpawnExperimentObject();
        }

    }

    private void SaveExperimentResults()
    {
        DateTime now = DateTime.Now;
        experimentResult.experimentEndTime = now;
        string filename = now.ToString("yyyyMMddhhmm");
        experimentResult.Save(filename + ".xml");
    }

    private void AddExperimentMaterialToObject(GameObject experimentObject)
    {
        experimentObject.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(experimentMaterial);
    }

    public void LogAnswer(int answer)
    {
        if (swapPositions)
        {
            if (answer == 2) answer = 0;
            else if (answer == 0) answer = 2;
        }
        
        experimentResult.questions[currentItemIndex].answerIndex = answer;
        experimentResult.questions[currentItemIndex].experimentPart = currentItemIndex;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

}
