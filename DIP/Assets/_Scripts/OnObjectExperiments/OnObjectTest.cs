using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class OnObjectTest : Test
{
    [XmlElement(ElementName = "ExperimentObjectName")]
    public string experimentObjectName;

    [XmlElement(ElementName = "ObjectOneSettings")]
    public EffectSettings objectOneSettings;
    
    [XmlElement(ElementName = "ObjectTwoSettings")]
    public EffectSettings objectTwoSettings;

}
