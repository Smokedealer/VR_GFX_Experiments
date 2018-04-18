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

    public GameObject defaultRoom;
    
    public UIContainer ui;
    public IntroOutroUI tui;

    public Recorder recorder;
 
    
    /********************************************************/

    private const string profilesPrefix = "PostProProfiles/ppp_";
    private const string roomsFolder = "ExperimentRooms/";
    private const float transitionDuration = 0.5f;
    
    private List<PostProcessingProfile> postProcessingProfiles;
    private List<Transform> roomSpawnPoints;
    private List<Transform> canvasAnchors;
    private List<PostProTest> tests;
    private List<GameObject> rooms;
    private List<Question> questions;

    private GameObject player;

    private bool experimentStarted = false;
    private bool experimentEnded = false;
    
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
        
        SetQuestionAndAnswers();
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
        player.transform.position = defaultRoom.transform.position;
        ui.wallUICanvas.transform.position = canvasAnchors[currentRoomNumber].transform.position;
    }

    public void MoveToFirstTest()
    {
        if (!experimentStarted)
        {
            StartCoroutine(RoomSwapRoutine());
            experimentStarted = true;
        }
        
        if(experimentEnded)
        {
            EndExperiment();
        }
    }

    /// <summary>
    /// Finds all available test rooms in the scene and creates lists of
    ///  Room Spawn Points and Canvas Anchors.  
    /// </summary>
    private void FindAndSortRooms()
    {
        rooms = new List<GameObject>();
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
            Transform spawnPoint = FindComponentInChildWithTag(room, "SpawnPoint");
            Transform canvasAnchor = FindComponentInChildWithTag(room, "CanvasAnchor");

            if (!spawnPoint)
            {
                Debug.LogError("Room " + room.name + " is missing a spawn point. Defaulting to the positions of the room");
                spawnPoint = room.transform;
            }

            if (!canvasAnchor)
            {
                canvasAnchor = room.transform;
                canvasAnchor.transform.position += Vector3.forward * 2f;
            }
            
            roomSpawnPoints.Add(spawnPoint);
            canvasAnchors.Add(canvasAnchor);
        }
    }
    
    /// <summary>
    /// Helper method to find <c>Transform</c> components in children by tag
    /// </summary>
    /// <param name="parent">the parenting gameobject</param>
    /// <param name="tag">the tag to look for</param>
    /// <returns><c>Transform</c> that has been found</returns>
    public static Transform FindComponentInChildWithTag(GameObject parent, string tag){
        Transform t = parent.transform;
        foreach(Transform tr in t)
        {
            if(tr.CompareTag(tag))
            {
                return tr;
            }
        }
        return null;
    }

    
    /// <summary>
    /// Compares two positions which is more to the left (less on X axis)
    /// </summary>
    /// <param name="a"><c>GameObject</c> a</param>
    /// <param name="b"><c>GameObject</c> b</param>
    /// <returns>-1 if a.x < b.x; 0 if a.x == b.x; 1 if a.x > b.x</returns>
    private static int ComparePositionsX(GameObject a, GameObject b)
    {
        return a.transform.position.x.CompareTo(b.transform.position.x);
    }

    
    /// <summary>
    /// Loads all post processing profiles that will be tested.
    /// Profiles have to be in <c>Resources/PostProProfiles/</c> folder and it is mandatory
    /// that they are named <c>ppp_</c> followed by an ascending number that is no more
    /// than 1 greater than the previous. In other words, they have to make a perfect sequence.
    /// </summary>
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

    
    /// <summary>
    /// Unity method called every frame
    /// </summary>
    void Update()
    {
        if (Input.GetButtonDown("SceneSwap"))
        {
            SceneSwap();
        }
    }

    
    /// <summary>
    /// Applies the next available post processing profile to be tested.
    /// </summary>
    public void SceneSwap()
    {
        StartCoroutine(EffectToggle());
        currentProfileIndex = (currentProfileIndex + 1) % postProcessingProfiles.Count; 
    }


    /// <summary>
    /// Loads the next test from the <c>Experiment</c> container.
    /// Loads coresponding questions and their answers.
    /// Question index is reset to 0.
    /// Next room is loaded.
    /// </summary>
    private void NextTest()
    {
        currentRoomNumber = (currentRoomNumber + 1) % roomSpawnPoints.Count;
        questions = experiment.tests[currentRoomNumber].questions;
        currentQuestionIndex = 0;

        StartCoroutine(RoomSwapRoutine());
    }


    /// <summary>
    /// Sets current question and coresponding answers
    /// </summary>
    private void SetQuestionAndAnswers()
    {
        SetQuestionText();
        SetOptions();
    }
    
    /// <summary>
    /// Handles answer selected by the user. Answer is recorded.
    /// If there are more questions withing the same test, the next one is loaded.
    /// If there are no more questions within the test, next test is laoded.
    /// If there are no more tests, user is teleported to the ending room.
    /// </summary>
    /// <param name="answerIndex">index of the selected answer</param>
    public void SelectAnswer(int answerIndex)
    {
        questions[currentQuestionIndex].answerIndex = answerIndex;

        if (currentQuestionIndex + 1 < questions.Count)
        {
            LoadNextQuestion();
        }
        else
        {
            if (currentRoomNumber + 1 < tests.Count)
            {
                NextTest();
                RefreshScene();
            }
            else
            {
                experimentEnded = true;
                tui.headline.text = "Done!";
                tui.sentence.alignment = TextAlignmentOptions.Center;
                tui.sentence.text = tui.outroString;
                TeleportToDefaultRoom();
            }
        }
    }

    private void RefreshScene()
    {
        SetQuestionText();
        SetOptions();
        ui.experimentPartText.text = currentRoomNumber + 1 + "/" + tests.Count;
        ui.questionValueText.text = currentQuestionIndex + 1 + "/" + questions.Count;
    }

    private void LoadNextQuestion()
    {
        currentQuestionIndex++;
        RefreshScene();
    }
    
    private void SetQuestionText()
    {
        ui.questionTextDisplay.text = questions[currentQuestionIndex].questionText;
    }

    private void SetOptions()
    {
        RemoveAnswersDisplay();

        int optionIndex = 0;
        
        foreach (var option in questions[currentQuestionIndex].questionOptions)
        {
            var button = Instantiate(ui.answerButtonPrefab, ui.answersLayout);
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
        foreach (Transform child in ui.answersLayout) {
            Destroy(child.gameObject);
        }
    }

    private void RemoveQuestionText()
    {
        ui.questionTextDisplay.text = "";
    }

    IEnumerator EffectToggle()
    {
        headsetFade.Fade(Color.black, transitionDuration); 
        yield return new WaitForSeconds(transitionDuration);
        postProcessingBehaviour.profile = postProcessingProfiles[currentProfileIndex];
        ui.sceneValueText.text = currentProfileIndex + 1 + "/" + postProcessingProfiles.Count;
        headsetFade.Unfade(transitionDuration);
    }
    

    IEnumerator RoomSwapRoutine()
    {
        headsetFade.Fade(Color.black, transitionDuration);
        yield return new WaitForSeconds(transitionDuration);
        player.transform.position = roomSpawnPoints[currentRoomNumber].transform.position;
        ui.wallUICanvas.transform.position = canvasAnchors[currentRoomNumber].transform.position;
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

    private void TeleportToDefaultRoom()
    {
        player.transform.position = defaultRoom.transform.position;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}