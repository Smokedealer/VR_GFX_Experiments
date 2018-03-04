using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PostProExperiment
{
    /** Collection of tests to be done */
    
    [XmlElement(ElementName = "ExperimentStart")]
    public DateTime experimentStartTime;
    
    [XmlElement(ElementName = "ExperimentEnd")]    
    public DateTime experimentEndTime;

    [XmlArray(ElementName = "Tests")]
    [XmlArrayItem("Test")]
    public List<PostProTest> tests;

//    [XmlElement(ElementName = "SystemInfo")]
//    public SystemInfoSerializable systemInfo;

//    [XmlElement(ElementName = "Profile")]
//    public PostProcessingProfile profile;
    
    public void Save(string filename)
    {
        var stream = new FileStream(filename, FileMode.Create);
        
        XmlSerializer serializer = new XmlSerializer(typeof(PostProExperiment));
        serializer.Serialize(stream, this);
    }
    
    public static PostProExperiment Load(string filename)
    {
        var stream = new FileStream(filename, FileMode.Open);
        XmlSerializer serializer = new XmlSerializer(typeof(PostProExperiment));
        return (PostProExperiment)serializer.Deserialize(stream);
    }
}
