using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PostProCustomTest : PostProTest
{
    [XmlArray(ElementName = "ObjectOneSettings")]
    [XmlArrayItem(ElementName = "Settings")]
    public List<EffectSettings> objectOneSettings;
    
    [XmlArray(ElementName = "ObjectTwoSettings")]
    [XmlArrayItem(ElementName = "Settings")]
    public List<EffectSettings> objectTwoSettings;
}
