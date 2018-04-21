using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class OnObjectTest : Test
{
    [XmlElement(ElementName = "ExperimentObjectName")]
    public string experimentObjectName;

    [XmlArray(ElementName = "ObjectOneSettings")]
    [XmlArrayItem(ElementName = "Settings")]
    public List<EffectSettings> objectOneSettings;
    
    [XmlArray(ElementName = "ObjectTwoSettings")]
    [XmlArrayItem(ElementName = "Settings")]
    public List<EffectSettings> objectTwoSettings;

}
