using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class Recording
{
    public List<PointInTime> cameraPositions;
    public List<PointInTime> leftControllerPositions;
    public List<PointInTime> rightControllerPositions;

    public FileType fileType;

    public List<string> experimentGameObjects;

    public List<int> theOOObjectSwapTimes;

    public List<int> thePPEffectSwapTimes;

    public Experiment experiment;
    
    public Recording()
    {
        cameraPositions = new List<PointInTime>();
        leftControllerPositions = new List<PointInTime>();
        rightControllerPositions = new List<PointInTime>();
        
        theOOObjectSwapTimes = new List<int>();
        thePPEffectSwapTimes = new List<int>();
    }
    
    public void SaveRecording(string fileName)
    {
        const string directory = "Recordings";
        string filePath = directory + "/" + fileName;
        
        var dir = new FileInfo(fileName).Directory;
        
        dir.Create();
        
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream stream = new FileStream(filePath, FileMode.Create);	
        
        Debug.Log("Saving " + cameraPositions.Count + " recorded frames");
		
        binaryFormatter.Serialize(stream, this);
        stream.Close();
    }

    public static Recording LoadRecording(string filePath)
    {
        if (File.Exists(filePath))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Open);			
			
            var recording = binaryFormatter.Deserialize(stream) as Recording;
            stream.Close();
            
            Debug.Log("Loaded " + recording.cameraPositions.Count + " frames");

            return recording;
        }
        
        Debug.LogError("File " + filePath + " does not exist.");
        return null;
    }
}