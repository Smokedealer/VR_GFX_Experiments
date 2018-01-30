using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class ExperimentResult
{
    [XmlElement(ElementName = "ExperimentStart")]
    public DateTime experimentStartTime;
    
    [XmlElement(ElementName = "ExperimentEnd")]    
    public DateTime experimentEndTime;

    [XmlArray(ElementName = "Questions")]
    public List<Question> questions;
    
    [XmlElement(ElementName = "SystemInfo")]
    public SystemInfoSerializable systemInfo;


    public ExperimentResult()
    {
        experimentStartTime = DateTime.Now;
        experimentEndTime = new DateTime();
        questions = new List<Question>();
        systemInfo = new SystemInfoSerializable();
    }


    public void Save(string filename)
    {
        using (var stream = new FileStream(filename, FileMode.Create))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ExperimentResult));
            serializer.Serialize(stream, this);
        }
    }
    
    public static ExperimentResult Load(string filename)
    {
        using (var stream = new FileStream(filename, FileMode.Open))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ExperimentResult));
            return (ExperimentResult)serializer.Deserialize(stream);
        }
    }
}
