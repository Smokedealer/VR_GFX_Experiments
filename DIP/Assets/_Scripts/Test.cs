using System.Collections.Generic;
using System.Xml.Serialization;

public class Test
{
    [XmlArray(ElementName = "Questions")]
    [XmlArrayItem("Question")]
    public List<Question> questions;
}
