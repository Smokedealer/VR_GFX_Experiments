using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ExperimentManager : MonoBehaviour {

    public TextMeshProUGUI experimentPartText;
    public TextMeshProUGUI questionTextDisplay;
    public GameObject answerButtonPrefab;
    public Transform answersLayout;
    public string disabledFeatureKeyword;

    public Transform spawnPoint1;
    public Transform spawnPoint2;

    /********************************************/
    
    private GameObject experimentObject;
    private GameObject originalObject;
    private GameObject copyObject;

    private Material originalMaterial;
    private Material experimentMaterial;

    private Experiment experiment;

    private bool swapPositions;


//    public StringGameObjectDictionary experimentObjects;
    
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
        
                
        SpawnExperimentObject();
    }


    private void SpawnExperimentObject()
    {
        //Despawn old (if any)
        DespawnOldObjects();

        //Set new object
        string objectTag = experiment.tests[currentItemIndex].experimentObejctName;

        var loadedGameObject = Resources.Load<GameObject>(objectTag);
        
        if (loadedGameObject != null)
        {
            
        }
        else
        {
            //TODO načítá objekt ze souboru, který neexistuje
            Debug.Log("Object with tag " + objectTag + " is not in the object dictionary.");
            //
        }

        //Display part of the experiment
        experimentPartText.text = currentItemIndex + 1 + "/" + experiment.tests.Count;

        //Display question
        SetQuestionText();
        
        //Display available options
        SetOptions();
        
        //Prepare materials
        PrepareMaterials();

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
        SetPropertyByType(experiment.tests[currentItemIndex].objectOneSettings, originalMaterial);
        
        //Create a copy to edit
        experimentMaterial = new Material(experimentObject.GetComponent<Renderer>().sharedMaterial);
        SetPropertyByType(experiment.tests[currentItemIndex].objectTwoSettings, experimentMaterial);
    }

    private void SetPropertyByType(EffectSettings effectSettings, Material effectMaterial)
    {
        Debug.Log(effectSettings.propertyName + " " + effectSettings.effectActive);
        
        switch (effectSettings.propertyType)
        {
            case "texture":
                if(!effectSettings.effectActive) effectMaterial.SetTexture(effectSettings.propertyName, null);
                effectMaterial.SetFloat("_BumpScale", 10f);
                break;
            case "float":
                if(!effectSettings.effectActive) effectMaterial.SetFloat(effectSettings.propertyName, effectSettings.effectIntensity);
                break;
        }
        
        //TODO dodělat ostatní case
            
    }


    private void SpawnExperimentObjects()
    {
        swapPositions = Random.value < 0.5f;

        Transform actualSpawnPoint1 = swapPositions ? spawnPoint2 : spawnPoint1;
        Transform actualSpawnPoint2 = swapPositions ? spawnPoint1 : spawnPoint2;

        originalObject = Instantiate(experimentObject, actualSpawnPoint1.position, actualSpawnPoint1.rotation);
        copyObject = Instantiate(experimentObject, actualSpawnPoint2.position, actualSpawnPoint2.rotation);

        AddExperimentMaterialToObject(originalObject, copyObject);

    }

    private bool LoadNextQuestion()
    {
        if (currentQuestionIndex + 1 < experiment.tests[currentQuestionIndex].questions.Count)
        {
            currentQuestionIndex++;
            SetQuestionText();
            SetOptions();
            return true;
        }

        return false;
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
            
            SpawnExperimentObject();
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
