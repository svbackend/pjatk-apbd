using System;

namespace APBD2
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFilePath = args.Length > 0 ? args[0] : "data.csv";
            var outputFilePath = args.Length > 1 ? args[1] : "result.xml";
            var format = args.Length > 2 ? args[2] : "xml";
            
            // todo
            var students = System.IO.File.ReadAllLines(@inputFilePath);
        }
    }
}