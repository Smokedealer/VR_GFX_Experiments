using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExperimentManager : MonoBehaviour {

    public GameObject experimentObject;
    public TextMeshPro textDisplayObject;
    public string disabledFeatureKeyword;

    public Transform spawnPoint1;
    public Transform spawnPoint2;

    private Material originalMaterial;
    private Material experimentMaterial;

    void Start()
    {
        PrepareMaterials();

        DisableTestedFeature();
        
        SpawnTestingObjects();

        textDisplayObject.text = disabledFeatureKeyword;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Spawning experiment objects");
            Start();
        }
    }


    void PrepareMaterials()
    {
        //Get original material
        originalMaterial = new Material(experimentObject.GetComponent<Renderer>().sharedMaterial);

        //Create a copy to edit
        experimentMaterial = new Material(experimentObject.GetComponent<Renderer>().sharedMaterial);
    }


    void DisableTestedFeature()
    {
        //Disable the experiment feature
        experimentMaterial.SetTexture(disabledFeatureKeyword, null);
    }


    void SpawnTestingObjects()
    {
        GameObject original = Instantiate(experimentObject, spawnPoint1.position, spawnPoint1.rotation);
        GameObject copy = Instantiate(experimentObject, spawnPoint2.position, spawnPoint2.rotation);

        AddExperimentMaterialToObject(copy);

        Debug.Log("Experiment objects spawned.");
    }

    void AddExperimentMaterialToObject(GameObject experimentObject)
    {
        experimentObject.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(experimentMaterial);
    }

}
