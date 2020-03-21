using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace APBD2.Entity
{
    [Serializable]
    [XmlRoot(ElementName = "university")]
    public class University
    {
        [XmlAttribute (AttributeName = "createdAt", DataType = "date")]
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
        
        [XmlAttribute (AttributeName = "author")]
        [JsonPropertyName("author")]
        public string Author { get; set; }

        [XmlArray(ElementName = "students")]
        [XmlArrayItem(typeof(Student), ElementName = "student")]
        [JsonPropertyName("students")]
        public List<Student> Students { get; set; }

        [XmlArray(ElementName = "activeStudies")]
        [XmlArrayItem(typeof(ActiveStudy), ElementName = "studies")]
        [JsonPropertyName("activeStudies")]
        public List<ActiveStudy> ActiveStudies { get; set; }

        public static University CreateFromStudentsList(List<Student> students)
        {
            return new University
            {
                Students = students,
                ActiveStudies = GetActiveStudiesByStudents(students)
            };
        }

        private static List<ActiveStudy> GetActiveStudiesByStudents(List<Student> students)
        {
            var studies = new Dictionary<string, ActiveStudy>();

            foreach (var student in students)
            {
                if (studies.ContainsKey(student.Studies.Name))
                {
                    studies[student.Studies.Name].NumberOfStudents++;
                }
                else
                {
                    var study = new ActiveStudy
                    {
                        Name = student.Studies.Name,
                        NumberOfStudents = 1
                    };

                    studies.Add(student.Studies.Name, study);
                }
            }

            return studies.Values.ToList();
        }
    }
}