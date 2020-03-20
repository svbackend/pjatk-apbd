using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using APBD2.Exception;

namespace APBD2
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFilePath = args.Length > 0 ? args[0] : "../../../data.csv";
            var outputFilePath = args.Length > 1 ? args[1] : "../../../result.xml";
            var format = args.Length > 2 ? args[2] : "xml";
            var logFilePath = "../../../log.xml";

            var students = new ListDictionary();
            var errorLog = "";
            var inputFileRows = File.ReadAllLines(@inputFilePath);
            foreach (var row in inputFileRows)
            {
                try
                {
                    var student = Student.CreateFromCsvRow(row);
                    if (!students.Contains(student.UniqueKey()))
                    {
                        students.Add(student.UniqueKey(), student);
                    }
                    else
                    {
                        throw new DuplicatedStudentRow();
                    }
                }
                catch (System.Exception e)
                {
                    errorLog += $"Exception thrown during processing row: {row}\r\n{e.Message}\r\n---\r\n";
                }
            }

            var test = new StudentsListDictionary();
            test.Students_ = students;
            using (MemoryStream ms = new MemoryStream())
            using (System.IO.TextWriter textWriter = new System.IO.StreamWriter(ms))
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(StudentsListDictionary));
                serializer.Serialize(textWriter, test);
                string text = Encoding.UTF8.GetString(ms.ToArray());
                Console.WriteLine(text);
            }
            
            /*
            XmlSerializer serializer = new XmlSerializer(typeof(ICollection<Student>), new XmlRootAttribute("university"));
            using(StringWriter textWriter = new StringWriter())
            {
                serializer.Serialize(textWriter, students.Values);
                System.Console.WriteLine(textWriter.ToString());
            }
            */
            
            Console.WriteLine(errorLog);
        }
    }
}