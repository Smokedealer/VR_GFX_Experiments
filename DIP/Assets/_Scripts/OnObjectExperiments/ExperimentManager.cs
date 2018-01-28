using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ExperimentManager : MonoBehaviour {

    public GameObject experimentObject;
    public TextMeshProUGUI textDisplayObject;
    public string disabledFeatureKeyword;

    public Transform spawnPoint1;
    public Transform spawnPoint2;

    private GameObject originalObject;
    private GameObject copyObject;

    private Material originalMaterial;
    private Material experimentMaterial;

    private bool swapPositions;

    public GameObject[] experimentObjects;
    private int currentItemIndex;

    void Start()
    {
        currentItemIndex = 0;
        disabledFeatureKeyword = ExperimentRunParameters.experimentPart;

        SpawnExperimentObject();
    }


    private void SpawnExperimentObject()
    {
        //Despawn old
        DespawnOldObjects();

        //Set new object
        experimentObject = experimentObjects[currentItemIndex];

        textDisplayObject.text = (currentItemIndex + 1) + "/" + experimentObjects.Length;

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


    private void PrepareMaterials()
    {
        //Get original material
        originalMaterial = new Material(experimentObject.GetComponent<Renderer>().sharedMaterial);

        //Create a copy to edit
        experimentMaterial = new Material(experimentObject.GetComponent<Renderer>().sharedMaterial);
    }


    private void DisableExperimentFeatureOnMaterial()
    {
        //Disable the experiment feature
        experimentMaterial.SetTexture(disabledFeatureKeyword, null);
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

        if (currentItemIndex + 1 >= experimentObjects.Length)
        {
            textDisplayObject.text = "done";
            DespawnOldObjects();
        }
        else
        {
            currentItemIndex++;
            experimentObject = experimentObjects[currentItemIndex];
            SpawnExperimentObject();
        }

    }

    private void AddExperimentMaterialToObject(GameObject experimentObject)
    {
        experimentObject.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(experimentMaterial);
    }

}
