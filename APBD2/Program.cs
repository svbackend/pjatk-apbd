using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using APBD2.Entity;
using APBD2.Exception;
using APBD2.Serializer;

namespace APBD2
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFilePath = args.Length > 0 ? args[0] : "../../../data.csv";
            var outputFilePath = args.Length > 1 ? args[1] : "../../../result.xml";
            var format = args.Length > 2 ? args[2] : "xml";
            var logFilePath = "../../../log.txt";

            var students = new Dictionary<string, Student>();
            var errorLog = "";

            string[] inputFileRows;

            try
            {
                inputFileRows = File.ReadAllLines(@inputFilePath);
            }
            catch (FileNotFoundException e)
            {
                File.WriteAllText(logFilePath, e.Message);
                return;
            }

            foreach (var row in inputFileRows)
            {
                try
                {
                    var student = Student.CreateFromCsvRow(row);
                    if (!students.ContainsKey(student.UniqueKey()))
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

            var university = University.CreateFromStudentsList(students.Values.ToList());

            string serializedData;
            switch (format)
            {
                case "xml":
                    serializedData = new XmlUniversitySerializer().Serialize(university);
                    break;
                case "json":
                    serializedData = new JsonUniversitySerializer().Serialize(university);
                    break;
                default: throw new ArgumentException("Unsupported format provided, please use either xml or json");
            }

            try
            {
                File.WriteAllText(outputFilePath, serializedData);
                File.WriteAllText(logFilePath, errorLog);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(errorLog);
            }
        }
    }
}