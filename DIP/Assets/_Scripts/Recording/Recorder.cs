using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Xsl;
using UnityEngine;
using Object = System.Object;


public class Recorder : MonoBehaviour
{
	/// <summary>
	/// Reference to the "head" or simply the main camera to be recorded
	/// </summary>
	public Transform playerCamera;
	
	/// <summary>
	/// Reference to the left controller
	/// </summary>
	public GameObject leftController;
	
	
	/// <summary>
	/// Reference to the right controller
	/// </summary>
	public GameObject rightController;
	
	/// <summary>
	/// Dummy head object for replays
	/// </summary>
	public GameObject playerCameraDummy;
	
	/// <summary>
	/// Dummy left controller object for replays
	/// </summary>
	public GameObject leftControllerDummy;
	
	
	/// <summary>
	/// Dummy left controller object for replays
	/// </summary>
	public GameObject rightControllerDummy;

	/// <summary>
	/// What kind of recording this class is working with
	/// </summary>
	public FileType FileType;

	/// <summary>
	/// Collection of recorded data.
	/// </summary>
	public Recording recordedData;

	/// <summary>
	/// State boolean showing that the recorder is recording
	/// </summary>
	private bool recording;
	
	/// <summary>
	/// State variable to tell if the recorder is replaying 
	/// </summary>
	private bool replaying;
	
	/// <summary>
	/// Current frame to be played
	/// </summary>
	private int replayIndex = 0;

	public int OOitemIndex = 0;

	public int PPefectIndex = 0;

	/// <summary>
	/// List of recorded camera positions
	/// </summary>
	private List<PointInTime> cameraRecording;
	
	/// <summary>
	/// List of left controller positions
	/// </summary>
	private List<PointInTime> leftControllerRecording;
	
	/// <summary>
	/// List of right controller positions
	/// </summary>
	private List<PointInTime> rightControllerRecording;
	
	/// <summary>
	/// On start Recorder checks if the operation
	/// </summary>
	void Start ()
	{
		
		if (ApplicationDataContainer.replay)
		{
			recordedData = ApplicationDataContainer.loadedRecording;
			
			cameraRecording = recordedData.cameraPositions;
			leftControllerRecording = recordedData.leftControllerPositions;
			rightControllerRecording = recordedData.rightControllerPositions;
		}
		else
		{
			//if (ApplicationDataContainer.runMode == Controller.NonVR) playerCamera = Camera.main.transform;
			
			recordedData = new Recording
			{
				fileType = FileType
			};
		}
		
	}


	public void StartRecording()
	{
        recording = true;
		replaying = false;
	}

	public void StopRecording(string filename = "testRecording.rec")
	{
		recording = false;
		recordedData.SaveRecording(filename);
	}

	public void StartReplay()
	{
		recording = false;
		replaying = true;
	}

	public void StopReplay()
	{
		replaying = false;
	}

	public void PauseReplay()
	{
		replaying = !replaying;
	}

	public void SetReplayToFrame(int replayIndex = 0)
	{
		this.replayIndex = replayIndex;
	}

	private bool click;
	
	void FixedUpdate ()
	{
		click = !click;
		
		if (click)
		{
			return;
		} 
		
		if (recording)
		{
			RecordFrame();
		}	
		
		
		if(replaying)
		{	
			replayIndex++;
			ReplayFrame();
		}

	}

	public void SetType(Object caller)
	{
		if (caller is PPExperimentController) FileType = FileType.PP;
		if (caller is OOExperimentController) FileType = FileType.OO;
	}

	public int GetRecordingSize()
	{
		return recordedData.cameraPositions.Count;
	}

	public int GetCurrentFrameIndex()
	{
		return replayIndex;
	}
	

	public void ReplayFrame()
	{
		if (recordedData == null)
		{
			Debug.LogError("No data to replay");
			return;
		}
		

		if (replayIndex >= cameraRecording.Count)
		{
			Debug.Log("No more to replay.");
			StopReplay();
			return;
		}

		PointInTime pointInTime = cameraRecording[replayIndex];
		playerCameraDummy.transform.position = pointInTime.GetPosition();
		playerCameraDummy.transform.rotation = pointInTime.GetRotation();

		pointInTime = leftControllerRecording[replayIndex];
		leftControllerDummy.transform.position = pointInTime.GetPosition();
		leftControllerDummy.transform.rotation = pointInTime.GetRotation();

		pointInTime = rightControllerRecording[replayIndex];
		rightControllerDummy.transform.position = pointInTime.GetPosition();
		rightControllerDummy.transform.rotation = pointInTime.GetRotation();
	}


	private void RecordFrame()
	{
		recordedData.cameraPositions.Add(new PointInTime(playerCamera.position, playerCamera.rotation));
		if (leftController != null) recordedData.leftControllerPositions.Add(new PointInTime(leftController.transform.position, leftController.transform.rotation));
		if (rightController != null) recordedData.rightControllerPositions.Add(new PointInTime(rightController.transform.position, rightController.transform.rotation));
		
		if(FileType == FileType.OO) recordedData.theOOObjectSwapTimes.Add(OOitemIndex);
		if(FileType == FileType.PP) recordedData.thePPEffectSwapTimes.Add(PPefectIndex);
	}

	public bool IsReplaying()
	{
		return replaying;
	}

	
}

public interface IExperimentController
{
	Experiment GetExperimentReference();
}
