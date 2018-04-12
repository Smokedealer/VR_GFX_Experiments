using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PostProTest : Test
{
    [XmlElement(ElementName = "ExperimentRoomName")]
    public string experimentRoomName;
}
