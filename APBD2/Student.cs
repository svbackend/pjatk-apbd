using System;
using APBD2.Exception;

namespace APBD2
{
    [Serializable]
    public class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StudiesName { get; set; }
        public string StudiesMode { get; set; }
        public int StudentId { get; set; }
        public DateTime BirthdayDate { get; set; }
        public string Email { get; set; }
        public string MothersName { get; set; }
        public string FathersName { get; set; }

        /** Used to detect duplicated records */
        public string UniqueKey()
        {
            return FirstName + LastName + StudentId.ToString();
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
                StudiesName = studentData[2],
                StudiesMode = studentData[3],
                StudentId = int.Parse(studentData[4]),
                BirthdayDate = DateTime.Parse(studentData[5]),
                Email = studentData[6],
                MothersName = studentData[7],
                FathersName = studentData[8]
            };
        }
    }
}