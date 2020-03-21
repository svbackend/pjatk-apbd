using System;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using APBD2.Converter;
using APBD2.Exception;

namespace APBD2.Entity
{
    [Serializable]
    public class Student
    {
        [XmlElement(ElementName = "fname")]
        [JsonPropertyName("fname")]
        public string FirstName { get; set; }
        
        [XmlElement(ElementName = "lname")]
        [JsonPropertyName("lname")]
        public string LastName { get; set; }

        [XmlElement(ElementName = "studies")]
        [JsonPropertyName("studies")]
        public StudentStudies Studies { get; set; }
        
        [XmlAttribute(AttributeName = "indexNumber")]
        [JsonPropertyName("indexNumber")]
        public string StudentNumber { get; set; }
        
        [XmlElement(ElementName = "birthdate", DataType = "date")]
        [JsonPropertyName("birthdate")]
        [JsonConverter(typeof(DateFormatConverter))]
        public DateTime BirthdayDate { get; set; }
        
        [XmlElement(ElementName = "email")]
        public string Email { get; set; }
        
        [XmlElement(ElementName = "mothersName")]
        public string MothersName { get; set; }
        
        [XmlElement(ElementName = "fathersName")]
        public string FathersName { get; set; }

        /** Used to detect duplicated records */
        public string UniqueKey()
        {
            return FirstName + LastName + StudentNumber.ToString();
        }

        public static Student CreateFromCsvRow(string row)
        {
            var studentData = row.Split(',');

            if (studentData.Length != 9)
            {
                throw new InvalidNumberOfRows(9, studentData.Length);
            }

            foreach (var column in studentData)
            {
                if (column.Length == 0)
                {
                    throw new EmptyValueProvided();
                }
            }

            return new Student
            {
                FirstName = studentData[0],
                LastName = studentData[1],
                Studies = new StudentStudies {Name = studentData[2], Mode = studentData[3]},
                StudentNumber = $"s{studentData[4]}",
                BirthdayDate = DateTime.Parse(studentData[5]),
                Email = studentData[6],
                MothersName = studentData[7],
                FathersName = studentData[8]
            };
        }

        private Student()
        {
        }
    }
}