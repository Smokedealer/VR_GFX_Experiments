using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

/// <summary>
/// On object experiment specific definition part
/// </summary>
public class OnObjectTest : Test
{
    /// <summary>
    /// Name of the object that will be loaded from the Resources folder
    /// </summary>
    [XmlElement(ElementName = "ExperimentObjectName")]
    public string experimentObjectName;

    
    /// <summary>
    /// First shader settings to be edited.
    /// </summary>
    [XmlArray(ElementName = "ObjectOneSettings")]
    [XmlArrayItem(ElementName = "Settings")]
    public List<EffectSettings> objectOneSettings;
    
    
    
    /// <summary>
    /// Second shader settings to be edited.
    /// </summary>
    [XmlArray(ElementName = "ObjectTwoSettings")]
    [XmlArrayItem(ElementName = "Settings")]
    public List<EffectSettings> objectTwoSettings;

}
