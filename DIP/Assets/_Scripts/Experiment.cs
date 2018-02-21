using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class Experiment
{
    /** Collection of tests to be done */

    public List<Test> tests;

    
    
    public void Save(string filename)
    {
        var stream = new FileStream(filename, FileMode.Create);
        
        XmlSerializer serializer = new XmlSerializer(typeof(Experiment));
        serializer.Serialize(stream, this);
    }
    
    public static Experiment Load(string filename)
    {
        var stream = new FileStream(filename, FileMode.Open);
        XmlSerializer serializer = new XmlSerializer(typeof(Experiment));
        return (Experiment)serializer.Deserialize(stream);
    }
}
