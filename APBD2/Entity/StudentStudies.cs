using System;
using System.Xml.Serialization;

namespace APBD2.Entity
{
    [Serializable]
    public class StudentStudies
    {
        [XmlElement (ElementName = "name")]
        public string Name { get; set; }
        
        [XmlElement (ElementName = "mode")]
        public string Mode { get; set; }
    }
}