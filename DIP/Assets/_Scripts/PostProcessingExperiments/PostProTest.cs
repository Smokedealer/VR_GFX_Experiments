using System.Xml.Serialization;

/// <summary>
/// Post-processing specific experiment definition part
/// </summary>
public class PostProTest : Test
{
    [XmlElement(ElementName = "ExperimentRoomName")]
    public string experimentRoomName;
}
