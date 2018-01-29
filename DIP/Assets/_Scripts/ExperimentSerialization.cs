
using System.Xml.Serialization;

public class ExperimentSerialization
{
    public void TrySerialize()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ExperimentRunParameters));
        
    }
}
