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

	public string extensionLookup = "*.xml";

	public string folder = ".";
	
	// Use this for initialization
	private void OnEnable()
	{
		var info = new DirectoryInfo(folder);
		var fileInfo = info.GetFiles(extensionLookup);

		//Remove all buttons if any
		foreach (GameObject child in contentLayout.transform)
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

		switch (recording.FileType)
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

		if (experiment.tests.Count > 0)
		{
			var sampleTest = experiment.tests[0];
			
			if (experiment.experimentType == FileType.PP)
			{
				Debug.Log("Custom Post Pro loaded");
			}
			else if (sampleTest is PostProTest)
			{
				Debug.Log("Post Pro loaded");
			}
			else if (sampleTest is OnObjectTest)
			{
				Debug.Log("On Object loaded");
			}
		}
		else
		{
			Debug.Log("Loaded test is not supported.");
		}
			
		
	}
}
