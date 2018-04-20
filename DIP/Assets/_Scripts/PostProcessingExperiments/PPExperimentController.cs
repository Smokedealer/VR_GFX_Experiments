using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using VRTK;

public class PPExperimentController : MonoBehaviour
{
    public PostProExperiment experiment;
    public PostProcessingBehaviour postProcessingBehaviour;
    public VRTK_HeadsetFade headsetFade;

    public Canvas wallUICanvas;
    
    public TextMeshProUGUI experimentPartText;
    public TextMeshProUGUI questionTextDisplay;
    public TextMeshProUGUI sceneValueText;
    public TextMeshProUGUI questionValueText;
    public Transform answersLayout;
    public GameObject answerButtonPrefab;
    
    public string profilesPrefix = "PostProProfiles/ppp_";
    public string roomsFolder = "ExperimentRooms/";
    public float transitionDuration = 0.5f;
    
    /********************************************************/
    
    private List<PostProcessingProfile> postProcessingProfiles;
    private List<Transform> roomSpawnPoints;
    private List<Transform> canvasAnchors;
    private List<PostProTest> tests;
    private List<GameObject> rooms;
    private List<Question> questions;

    private GameObject player;

    private int currentRoomNumber = 0;
    private int currentProfileIndex = 0;
    private int currentQuestionIndex = 0;
    
    void Start()
    {
        experiment = PostProExperiment.Load("experimentPPSettings.xml");
        //TODO Handle file not found
        
        tests = experiment.tests;
        questions = tests[currentRoomNumber].questions;
        
        LoadProfiles();
        SpawnRooms();
        FindAndSortRooms();

        SetInitilaPositions();
        
        StartExperiment();
        RefreshScene();
    }

    private void SpawnRooms()
    {
        float x = 0f;
        
        foreach (var test in tests)
        {
            string roomName = test.experimentRoomName;
            var room = Resources.Load<GameObject>(roomsFolder + roomName);

            if (room == null)
            {
                Debug.Log("Room " + roomName + " was not found in Resources.");
                
                //Default error room
                room = Resources.Load<GameObject>(roomsFolder + "error_room");
                //TODO set text and option to error
            }

            Instantiate(room, new Vector3(x, 0, 0), room.transform.rotation);

            x += room.GetComponent<Renderer>().bounds.size.x * 2;
        }
    }

    private void SetInitilaPositions()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = roomSpawnPoints[currentRoomNumber].transform.position;
        wallUICanvas.transform.position = canvasAnchors[currentRoomNumber].transform.position;
    }


    private void FindAndSortRooms()
    {
        List<GameObject> rooms = new List<GameObject>();
        roomSpawnPoints = new List<Transform>();
        canvasAnchors = new List<Transform>();
        
        //Find all experiment rooms
        foreach (var experimentRoom in GameObject.FindGameObjectsWithTag("ExperimentRoom"))
        {
            rooms.Add(experimentRoom);
        }

        //Sort them from the lefmost first
        rooms.Sort(ComparePositionsX);

        //Find spawn points and canvas anchors in each room
        foreach (var room in rooms)
        {
            Transform spawnPoint = FindComponentInChildWithTag<Transform>(room, "SpawnPoint");
            Transform canvasAnchor = FindComponentInChildWithTag<Transform>(room, "CanvasAnchor");
            
            roomSpawnPoints.Add(spawnPoint);
            canvasAnchors.Add(canvasAnchor);
        }
    }
    
    
    public static Transform FindComponentInChildWithTag<T>(GameObject parent, string tag){
        Transform t = parent.transform;
        foreach(Transform tr in t)
        {
            if(tr.tag == tag)
            {
                return tr;
            }
        }
        return null;
    }

    private static int ComparePositionsX(GameObject a, GameObject b)
    {
        return a.transform.position.x.CompareTo(b.transform.position.x);
    }

    private void LoadProfiles()
    {
        postProcessingProfiles = new List<PostProcessingProfile>();

        
        int count = 0;

        while (Resources.Load(profilesPrefix + count) != null)
        {
            var profile = Resources.Load<PostProcessingProfile>(profilesPrefix + count);
            postProcessingProfiles.Add(profile);
            count++;
        }
    }

    
    void Update()
    {
        if (Input.GetButtonDown("SceneSwap"))
        {
            SceneSwap();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentRoomNumber = (currentRoomNumber + 1) % roomSpawnPoints.Count; 
            StartCoroutine(RoomSwapRoutine());
        }
        
        //TODO UI updates
        
    }

    public void SceneSwap()
    {
        StartCoroutine(EffectToggle());
        currentProfileIndex = (currentProfileIndex + 1) % postProcessingProfiles.Count; 
    }



    private void LoadNextTest()
    {
        currentRoomNumber = (currentRoomNumber + 1) % roomSpawnPoints.Count;
        questions = experiment.tests[currentRoomNumber].questions;
        currentQuestionIndex = 0;

        StartCoroutine(RoomSwapRoutine());
    }


    private void StartExperiment()
    {
        SetQuestionText();
        SetOptions();
    }
    

    public void SelectAnswer(int answerIndex)
    {
        
        Debug.Log("Answer" + answerIndex);
        
        questions[currentQuestionIndex].answerIndex = answerIndex;

        if (currentQuestionIndex + 1 < questions.Count)
        {
            LoadNextQuestion();
        }
        else
        {
            if (currentRoomNumber + 1 < tests.Count)
            {
                LoadNextTest();   
                RefreshScene();
            }
            else
            {
                EndExperiment();
            }
               
        }

    }

    private void RefreshScene()
    {
        SetQuestionText();
        SetOptions();
        experimentPartText.text = currentRoomNumber + 1 + "/" + tests.Count;
        questionValueText.text = currentQuestionIndex + 1 + "/" + questions.Count;
    }

    private void LoadNextQuestion()
    {
        currentQuestionIndex++;
        RefreshScene();
    }
    
    private void SetQuestionText()
    {
        questionTextDisplay.text = questions[currentQuestionIndex].questionText;
    }

    private void SetOptions()
    {
        RemoveAnswersDisplay();

        int optionIndex = 0;
        
        foreach (var option in questions[currentQuestionIndex].questionOptions)
        {
            var button = Instantiate(answerButtonPrefab, answersLayout);
            button.GetComponentInChildren<TextMeshProUGUI>().text = option;
            button.GetComponent<Button>().onClick.AddListener
            (
                    ButtonClickDelegate(optionIndex)
            );

            optionIndex++;
        }

    }
    
    private UnityAction ButtonClickDelegate(int optionIndex)
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

    IEnumerator EffectToggle()
    {
        headsetFade.Fade(Color.black, transitionDuration); 
        yield return new WaitForSeconds(transitionDuration);
        postProcessingBehaviour.profile = postProcessingProfiles[currentProfileIndex];
        sceneValueText.text = currentProfileIndex + 1 + "/" + postProcessingProfiles.Count;
        headsetFade.Unfade(transitionDuration);
    }
    

    IEnumerator RoomSwapRoutine()
    {
        headsetFade.Fade(Color.black, transitionDuration);
        yield return new WaitForSeconds(transitionDuration);
        player.transform.position = roomSpawnPoints[currentRoomNumber].transform.position;
        wallUICanvas.transform.position = canvasAnchors[currentRoomNumber].transform.position;
        headsetFade.Unfade(transitionDuration);

        
    }

    public void EndExperiment()
    {
        DateTime now = DateTime.Now;
        experiment.experimentEndTime = now;
        string filename = now.ToString("yyyyMMddhhmm");
        experiment.Save("result-p-" + filename + ".xml");
        
        ReturnToMainMenu();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}