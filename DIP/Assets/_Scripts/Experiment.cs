using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
    /// Experiment type:
    /// 
    /// <c>OO</c> for On Object shader experiments
    /// <c>PP</c> for Post Processing stack experiments
    /// <c>PPC</c> for Post Processing experiments with Custom shaders
    /// 
    /// </summary>
    [XmlElement(ElementName = "ExperimentType")]
    public FileType experimentType;
    
    
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
    /// <see cref="PostProTest"/>
    /// <see cref="PostProCustomTest"/>
    [XmlArray(ElementName = "Tests")]
    [XmlArrayItem("TestOO", Type = typeof(OnObjectTest))]
    [XmlArrayItem("TestPPC", Type = typeof(PostProCustomTest))]
    [XmlArrayItem("TestPP", Type = typeof(PostProTest))]
    public List<Test> tests;

    
    
    /// <summary>
    /// Information about the system the experiment was conducted on.
    /// See <see cref="SystemInfoSerializable"/>.
    /// </summary>
//    [XmlElement(ElementName = "SystemInfo")]
//    public SystemInfoSerializable systemInfo;

    
    
    /// <summary>
    /// Name of the headset used for the experiment.
    /// </summary>
    //public string headsetInfo = VRTK_DeviceFinder.GetHeadsetType().ToString();

    
    
    /// <summary>
    /// Saves the experiment to an XML file on the given path.
    /// </summary>
    /// <param name="filename">File name (or path)</param>
    public void Save(string filename)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Experiment));
        
        using(StreamWriter sw = new StreamWriter(filename, false, Encoding.UTF8))
        {
            serializer.Serialize(sw, this);
        }
    }
    
    /// <summary>
    /// Saves the experiment to an XML file on the given path.
    /// </summary>
    /// <param name="filename">File name (or path)</param>
    public void SaveResult(string filename)
    {
        const string directory = "Recordings";
        string filePath = directory + "/" + filename;
        if(!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        
        Save(filePath);
    }
    
    
    /// <summary>
    /// Deserializes the <c>Experiment</c> class from given the file.
    /// Make sure the file charset is UTF-8, otherwise an error may occur.
    /// </summary>
    /// <param name="filename">File name (or path)</param>
    /// <returns><c>Experiment</c> object loaded from the file</returns>
    public static Experiment Load(string filename)
    {
        var stream = new StreamReader(filename, Encoding.UTF8, true);
        
        XmlSerializer serializer = new XmlSerializer(typeof(Experiment));
     
        var result = (Experiment)serializer.Deserialize(stream);
        stream.Close();
        return result;
    }
}
