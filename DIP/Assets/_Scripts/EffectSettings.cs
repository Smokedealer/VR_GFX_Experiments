using System.Xml.Serialization;


public class EffectSettings
{
    [XmlElement(ElementName = "EffectActive")]
    public bool effectActive;
    
    [XmlElement(ElementName = "EffectName")]
    public string effectName;
    
    [XmlElement(ElementName = "PropertyName")]
    public string propertyName;

    [XmlElement(ElementName = "PropertyType")]
    public string propertyType;
    
    [XmlElement(ElementName = "EffectIntensity")]
    public float propertyValue;
}
