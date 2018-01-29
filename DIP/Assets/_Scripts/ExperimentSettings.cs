
using System.IO;
using System.Xml.Serialization;

public class ExperimentSettings
{
    [XmlElement(ElementName = "SceneNumber")]
    public int sceneNumber;
    
    [XmlElement(ElementName = "ExperimentEffect")]
    public string experimentEffect;
    
    public void Save(string filename)
    {
        using (var stream = new FileStream(filename, FileMode.Create))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ExperimentSettings));
            serializer.Serialize(stream, this);
        }
    }
    
    public static ExperimentSettings Load(string filename)
    {
        using (var stream = new FileStream(filename, FileMode.Open))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ExperimentSettings));
            return (ExperimentSettings)serializer.Deserialize(stream);
        }
    }
}
