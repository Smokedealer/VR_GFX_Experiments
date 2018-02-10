using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using TMPro;

public class PPExperimentController : MonoBehaviour
{
    public PostProcessingProfile processingProfile;
    public Material cameraMetrial;
    public TextMeshPro questionTextDisplay;

    private bool effectEnabled = true;

    private GameObject[] roomSpawnPoints;
    private GameObject player;

    private int currentRoomNumber = 0;

    void Start()
    {
        roomSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log(roomSpawnPoints.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("SceneSwap"))
        {
            string effect;
            
            if (ExperimentRunParameters.settings == null)
            {
                Debug.LogError("Effect not loaded. Using SSAO.");
                effect = "SSAO";
            }
            else
            {
                effect = ExperimentRunParameters.settings.experimentEffect;
            }
            
            EffectToggle(effect);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentRoomNumber = (currentRoomNumber + 1) % roomSpawnPoints.Length; 
            NextRoom(currentRoomNumber);
        }
        
    }


    public void NextRoom(int roomNumber)
    {
        Debug.Log("Entering room: " + roomNumber);
        player.transform.position = roomSpawnPoints[roomNumber].transform.position;
    }

    public void EffectToggle(string effect)
    {
        effectEnabled = !effectEnabled;

        switch (effect)
        {
            case "SSAO":
                processingProfile.ambientOcclusion.enabled = effectEnabled;
                break;
            case "AA":
                processingProfile.antialiasing.enabled = effectEnabled;
                break;
            case "Bloom":
                processingProfile.bloom.enabled = effectEnabled;
                break;
            case "EyeAdapt":
                processingProfile.eyeAdaptation.enabled = effectEnabled;
                break;
        }
    }
}