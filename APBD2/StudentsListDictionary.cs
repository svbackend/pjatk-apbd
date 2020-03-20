using System.Collections.Specialized;
using System.Xml.Serialization;

namespace APBD2
{
    [XmlRoot("students")]
    public class StudentsListDictionary
    {
        public ListDictionary Students_ { set; get; }

        [XmlElement("student")]
        public ListDictionary Students
        {
            get { return Students_; }
        }
    }
}