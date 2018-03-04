using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PostProTest
{
    [XmlElement(ElementName = "ExperimentRoomName")]
    public string experimentRoomName;

//    [XmlElement(ElementName = "ObjectOneSettings")]
//    public EffectSettings objectOneSettings;
//    
//    [XmlElement(ElementName = "ObjectTwoSettings")]
//    public EffectSettings objectTwoSettings;

    [XmlArray(ElementName = "Questions")]
    [XmlArrayItem("Question")]
    public List<Question> questions;

}
