using System.Collections.Generic;
using System.Xml.Serialization;

/// <summary>
/// This class is a sample class for other types of experiment.
/// It is currently unused.
///
/// This class allows for post-processing testing that are deprecated
/// (made with old workflow) 
/// </summary>
public class PostProCustomTest : PostProTest
{
    /// <summary>
    /// Camera shader 1 settings
    /// </summary>
    [XmlArray(ElementName = "ObjectOneSettings")]
    [XmlArrayItem(ElementName = "Settings")]
    public List<EffectSettings> objectOneSettings;
    
    /// <summary>
    /// Canera shader 2 settings 
    /// </summary>
    [XmlArray(ElementName = "ObjectTwoSettings")]
    [XmlArrayItem(ElementName = "Settings")]
    public List<EffectSettings> objectTwoSettings;
}
