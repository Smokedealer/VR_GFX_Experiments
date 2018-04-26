using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FileLoader : MonoBehaviour
{

	public enum FileLoadType
	{
		Experiment,
		Recording
	}
	
	public Button listItem;
	public Transform contentLayout;

	public FileLoadType fileType = FileLoadType.Experiment;

	private string filePattern;

	public string folder = ".";
	
	// Use this for initialization
	private void OnEnable()
	{
		filePattern = (fileType == FileLoadType.Experiment) ? "*.xml" : "*.rec";
		
		var info = new DirectoryInfo(folder);
		var fileInfo = info.GetFiles(filePattern);

		//Remove all buttons if any
		foreach (GameObject child in contentLayout)
		{
			Destroy(child);
		}
		
		foreach (var file in fileInfo)
		{
			Debug.Log(file.ToString());
			var button = Instantiate(listItem, contentLayout);
			
			button.onClick.AddListener(ButtonClickDelegate(file.ToString()));

			var buttonText = button.GetComponentInChildren<Text>();

			buttonText.text = file.Name;
		}
	}

	
	private UnityAction ButtonClickDelegate(string path)
	{
		return delegate { LoadFile(path); };
	}

	private void LoadFile(string path)
	{
		if (fileType == FileLoadType.Experiment)
		{
			LoadExperimentFromFile(path);
		}
		else if (fileType == FileLoadType.Recording)
		{
			LoadRecordingFromFile(path);
		}
	}

	private void LoadRecordingFromFile(string path)
	{
		var recording = Recording.LoadRecording(path);
		
		ApplicationDataContainer.replay = true;
		ApplicationDataContainer.loadedRecording = recording;

		switch (recording.fileType)
		{
			case FileType.OO:
				Debug.Log("OO recording loaded.");
				
				break;
			case FileType.PP:
				Debug.Log("PP recording loaded.");
				break;
			case FileType.PPC:
				Debug.Log("PPC recording loaded.");
				break;
		}
	}

	private void LoadExperimentFromFile(string path)
	{
		var experiment = Experiment.Load(path);
		
		ApplicationDataContainer.replay = false;
		ApplicationDataContainer.loadedExperiment = experiment;
		
		
		switch (experiment.experimentType)
		{
			case FileType.OO:
				Debug.Log("OO recording loaded.");
				break;
			case FileType.PP:
				Debug.Log("PP recording loaded.");
				break;
			case FileType.PPC:
				Debug.Log("PPC recording loaded.");
				break;
		}
	}
}
