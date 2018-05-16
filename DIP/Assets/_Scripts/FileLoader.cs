using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
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
		foreach (Transform child in contentLayout)
		{
			Destroy(child.gameObject);
		}
		
		foreach (var file in fileInfo)
		{
			var button = Instantiate(listItem, contentLayout);
			
			button.onClick.AddListener(ButtonClickDelegate(file.FullName));

			var buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

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
				SceneManager.LoadScene("OnObjectExperiments");
				break;
			case FileType.PP:
				SceneManager.LoadScene("PostProExperiments");
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
				SceneManager.LoadScene("OnObjectExperiments");
				break;
			case FileType.PP:
				SceneManager.LoadScene("PostProExperiments");
				break;
		}
	}
}
