using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using VRTK;

/// <summary>
/// Experiment class is a serializable class that keeps all the necessary information
/// about the experiment coupled with the system information. 
/// </summary>
[Serializable]
public class Experiment
{
    /// <summary>
    /// The point in time that the experiment started
    /// </summary>
    [XmlElement(ElementName = "ExperimentStart")]
    public DateTime experimentStartTime;
    
    
    
    /// <summary>
    /// The point in time that the experiment ended
    /// </summary>
    [XmlElement(ElementName = "ExperimentEnd")]    
    public DateTime experimentEndTime;

    
    /// <summary>
    /// A list of <c>Tests</c> to be performed
    /// </summary>
    /// <see cref="OnObjectTest"/>
    [XmlArray(ElementName = "Tests")]
    [XmlArrayItem("Test")]
    public List<OnObjectTest> tests;

    
    
    /// <summary>
    /// Information about the system the experiment was conducted on.
    /// See <see cref="SystemInfoSerializable"/>.
    /// </summary>
    [XmlElement(ElementName = "SystemInfo")]
    public SystemInfoSerializable systemInfo;

    
    
    /// <summary>
    /// Name of the headset used for the experiment.
    /// </summary>
    public string headsetInfo = VRTK_DeviceFinder.GetHeadsetType().ToString();

    
    
    /// <summary>
    /// Saves the experiment to an XML file on the given path.
    /// </summary>
    /// <param name="filename">File name (or path)</param>
    public void Save(string filename)
    {
        var stream = new FileStream(filename, FileMode.Create);
        
        XmlSerializer serializer = new XmlSerializer(typeof(Experiment));
        serializer.Serialize(stream, this);
    }
    
    
    /// <summary>
    /// Deserializes the <c>Experiment</c> class from given the file.
    /// Make sure the file charset is UTF-8, otherwise an error may occur.
    /// </summary>
    /// <param name="filename">File name (or path)</param>
    /// <returns><c>Experiment</c> object loaded from the file</returns>
    public static Experiment Load(string filename)
    {
        var stream = new FileStream(filename, FileMode.Open);
        XmlSerializer serializer = new XmlSerializer(typeof(Experiment));
        return (Experiment)serializer.Deserialize(stream);
    }
}
