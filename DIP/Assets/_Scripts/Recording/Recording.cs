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

    public FileType FileType;

    public Recording()
    {
        cameraPositions = new List<PointInTime>();
        leftControllerPositions = new List<PointInTime>();
        rightControllerPositions = new List<PointInTime>();
    }
    
    public void SaveRecording(string fileName = "testRecording.rec")
    {
        string directory = "Recordings";
        string filePath = directory + "/" + fileName;
        if(!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
		
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream stream = new FileStream(filePath, FileMode.Create);		
		
        binaryFormatter.Serialize(stream, this);
        stream.Close();
	
    }

    public static Recording LoadRecording(string filePath = "Recordings/testRecording.rec")
    {
		
        if (File.Exists(filePath))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Open);			
			
            var recording = binaryFormatter.Deserialize(stream) as Recording;
            stream.Close();

            return recording;
        }
        
        
        Debug.LogError("File " + filePath + " does not exist.");
        return null;
    }
}