using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using APBD3.Models;

namespace APBD3.DAL
{
    public class DbService : IDbService
    {
        private static string connectionString = "Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=s19319;Integrated Security=True;Trusted_Connection=true;";
        public IEnumerable<Student> GetStudents()
        {
            var students = new List<Student>();
            
            var sql = "SELECT FirstName, LastName, BirthDate, Studies.Name StudiesName, Semester, IndexNumber, StartDate FROM Student LEFT JOIN Enrollment E ON Student.IdEnrollment = E.IdEnrollment LEFT JOIN Studies on E.IdStudy = Studies.IdStudy";

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    students.Add(new Student
                    {
                        IndexNumber = dr["IndexNumber"].ToString(),
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        BirthDate = DateTime.Parse(dr["BirthDate"].ToString()),
                        Enrollment = new Enrollment
                        {
                            Semester = int.Parse(dr["Semester"].ToString()),
                            StartDate = DateTime.Parse(dr["StartDate"].ToString()),
                            Study = new Study
                            {
                                Name = dr["StudiesName"].ToString()
                            }
                        },
                    });
                }
            }
            
            return students;
        }
        
        public IEnumerable<Enrollment> GetStudentEntries(int indexNumber)
        {
            // name, surname, date of birth, name of studies and semester number
            var sql = "SELECT E.IdEnrollment IdEnrollment, Semester, StartDate, Studies.Name StudiesName FROM Enrollment E RIGHT JOIN Student S on S.IdEnrollment = E.IdEnrollment LEFT JOIN Studies ON E.IdStudy = Studies.IdStudy WHERE S.IndexNumber = @IndexNumber";

            var enrollments = new List<Enrollment>();
            
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("IndexNumber", indexNumber);
                
                conn.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    enrollments.Add(new Enrollment()
                    {
                            Semester = int.Parse(dr["Semester"].ToString()),
                            StartDate = DateTime.Parse(dr["StartDate"].ToString()),
                            Study = new Study
                            {
                                Name = dr["StudiesName"].ToString()
                            }
                        });
                }
            }
            
            return enrollments;
        }
    }
}