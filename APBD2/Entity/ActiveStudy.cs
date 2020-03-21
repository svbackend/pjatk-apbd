using System.Xml.Serialization;

namespace APBD2.Entity
{
    [XmlRoot(ElementName = "studies")]
    public class ActiveStudy
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public int NumberOfStudents { get; set; }
    }
}