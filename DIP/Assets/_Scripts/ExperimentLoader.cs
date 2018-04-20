using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ExperimentLoader : MonoBehaviour
{

	public Button listItem;
	public Transform contentLayout;
	
	// Use this for initialization
	private void OnEnable()
	{
		var info = new DirectoryInfo(".");
		var fileInfo = info.GetFiles("*.xml");

		foreach (var file in fileInfo)
		{
			Debug.Log(file.Name);
			var button = Instantiate(listItem, contentLayout);
			
			button.onClick.AddListener(ButtonClickDelegate(file.ToString()));
		}
	}
	
	private UnityAction ButtonClickDelegate(string path)
	{
		return delegate { LoadExperimentFromFile(path); };
	}

	private void LoadExperimentFromFile(string path)
	{
		var experiment = Experiment.Load(path);

		if (experiment.tests[0] is PostProExperiment)
		{
			Debug.Log("Post Pro");
		}
		
	}

	
	// Update is called once per frame
	void Update () {
		
	}
}
